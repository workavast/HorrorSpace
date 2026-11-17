using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Avastrad.UI.UiSystem
{
    internal sealed class ScreenCoordinator
    {
        private readonly ScreenStore _store;
        private readonly ScreenHistory _history = new();
        private readonly Transform _parent;
        private readonly IUIScreenFactory _defaultFactory;
        private bool _busy;

        public ScreenCoordinator(ScreenCatalog catalog, Transform parent, IUIScreenFactory defaultFactory)
        {
            _store = new ScreenStore(catalog);
            _parent = parent;
            _defaultFactory = defaultFactory;
        }
        
        public void Dispose()
        {
            foreach (var record in _store.Records)
                record.Release();
        }

        public async UniTask PreloadAsync(IReadOnlyList<Request> requests, bool createHidden,
            IUIScreenFactory factory, CancellationToken ct)
        {
            await PlaceInQueueAsync(ct);
            try { await PrepareAsync(requests, createHidden, factory, ct); }
            finally { _busy = false; }
        }

        public void Preload(IReadOnlyList<Request> requests, bool createHidden, IUIScreenFactory factory)
        {
            PlaceInQueue();
            try { Prepare(requests, createHidden, factory); }
            finally { _busy = false; }
        }

        public async UniTask ChangeScreensAsync(IReadOnlyList<Request> requests, bool replace, int? order,
            ScreenReleaseMode releaseMode, IUIScreenFactory factory, CancellationToken ct)
        {
            await PlaceInQueueAsync(ct);
            try
            {
                var prepared = await PrepareAsync(requests, true, factory, ct);
                ApplyPreparedScreens(prepared, replace, order, releaseMode);
            }
            finally { _busy = false; }
        }

        public void ChangeScreens(IReadOnlyList<Request> requests, bool replace, int? order,
            ScreenReleaseMode releaseMode, IUIScreenFactory factory)
        {
            PlaceInQueue();
            try
            {
                var prepared = Prepare(requests, true, factory);
                ApplyPreparedScreens(prepared, replace, order, releaseMode);
            }
            finally { _busy = false; }
        }

        public void Remove(IReadOnlyList<Request> requests, ScreenReleaseMode releaseMode)
        {
            PlaceInQueue();
            try
            {
                var removed = new HashSet<ScreenRecord>();
                foreach (var request in requests)
                    removed.Add(Resolve(request));
                
                var before = Snapshot();
                var changed = false;
                foreach (var record in removed)
                {
                    changed |= record.IsActive;
                    record.HideInstance(releaseMode);
                }
                
                if (changed)
                    _history.Push(before);
            }
            finally { _busy = false; }
        }

        public void Clear(ScreenReleaseMode releaseMode)
        {
            PlaceInQueue();
            try
            {
                var before = Snapshot();
                foreach (var record in _store.Records)
                    if (record.IsActive || releaseMode == ScreenReleaseMode.Release)
                        record.HideInstance(releaseMode);
                
                if (before.Count > 0)
                    _history.Push(before);
            }
            finally { _busy = false; }
        }

        public async UniTask UndoAsync(CancellationToken ct)
        {
            await PlaceInQueueAsync(ct);
            try
            {
                if (!_history.CanUndo)
                    return;
                
                var snapshot = _history.Peek();
                await RestoreAssetsAsync(snapshot, ct);
                Restore(snapshot);
                _history.Pop();
            }
            finally { _busy = false; }
        }

        public void Undo()
        {
            PlaceInQueue();
            try
            {
                if (!_history.CanUndo)
                    return;
                
                var snapshot = _history.Peek();
                var loadedNow = new List<ScreenRecord>();
                var createdNow = new List<ScreenRecord>();
                try
                {
                    foreach (var item in snapshot)
                    {
                        var record = item.Record;
                        if (record.Prefab == null)
                            loadedNow.Add(record);
                        
                        record.LoadPrefab();
                        
                        if (record.Instance == null)
                        {
                            record.CreateInstance(record.Factory ?? _defaultFactory, _parent);
                            createdNow.Add(record);
                        }
                        
                        record.ValidateCanvas();
                    }
                }
                catch
                {
                    Rollback(loadedNow, createdNow);
                    throw;
                }
                Restore(snapshot);
                _history.Pop();
            }
            finally { _busy = false; }
        }

        public bool TryGetActive<T>(string id, out T screen) where T : ScreenBase
        {
            var record = _store.Get(id);
            screen = record?.IsActive == true ? record.Instance as T : null;
            return screen != null;
        }

        private ScreenRecord Resolve(Request request)
            => request.HasReference ? _store.GetOrCreate(request.Id, request.Reference) : _store.GetOrCreate(request.Id);

        private async UniTask<List<ScreenRecord>> PrepareAsync(IReadOnlyList<Request> requests,
            bool create, IUIScreenFactory factory, CancellationToken ct)
        {
            var prepared = new List<ScreenRecord>(requests.Count);
            var loadedNow = new List<ScreenRecord>();
            var createdNow = new List<ScreenRecord>();
            try
            {
                foreach (var request in requests)
                {
                    var record = Resolve(request);
                    
                    if (prepared.Contains(record))
                        throw new ArgumentException("Duplicate screen in one request");
                    
                    if (record.Prefab == null)
                        loadedNow.Add(record);
                    
                    await record.LoadPrefabAsync(ct);
                    
                    record.ValidateFactory(factory);
                    
                    if (create && record.Instance == null)
                    {
                        record.CreateInstance(factory ?? _defaultFactory, _parent);
                        createdNow.Add(record);
                    }
                    
                    if (create)
                        record.ValidateCanvas();
                    
                    prepared.Add(record);
                }
                return prepared;
            }
            catch
            {
                Rollback(loadedNow, createdNow);
                throw;
            }
        }

        private List<ScreenRecord> Prepare(IReadOnlyList<Request> requests, bool create, IUIScreenFactory factory)
        {
            var prepared = new List<ScreenRecord>(requests.Count);
            var loadedNow = new List<ScreenRecord>();
            var createdNow = new List<ScreenRecord>();
            try
            {
                foreach (var request in requests)
                {
                    var record = Resolve(request);
                    if (prepared.Contains(record))
                        throw new ArgumentException("Duplicate screen in one request");
                    
                    if (record.Prefab == null) 
                        loadedNow.Add(record);
                    
                    record.LoadPrefab();
                    record.ValidateFactory(factory);
                    if (create && record.Instance == null)
                    {
                        record.CreateInstance(factory ?? _defaultFactory, _parent);
                        createdNow.Add(record);
                    }
                    if (create)
                        record.ValidateCanvas();
                    
                    prepared.Add(record);
                }
                return prepared;
            }
            catch
            {
                Rollback(loadedNow, createdNow);
                throw;
            }
        }
        
        private void ApplyPreparedScreens(List<ScreenRecord> requested, bool replace, int? firstScreenOrder,
            ScreenReleaseMode releaseMode)
        {
            var snapshot = Snapshot();
            var requestedSet = new HashSet<ScreenRecord>(requested);
            var desired = new List<ScreenHistory.Item>();
            
            if (!replace)
                foreach (var item in snapshot)
                    if (!requestedSet.Contains(item.Record))
                        desired.Add(item);

            var start = firstScreenOrder ?? (replace ? 0 : GetNextOrder(desired));
            for (var i = 0; i < requested.Count; i++)
                desired.Add(new ScreenHistory.Item(requested[i], checked(start + i)));

            if (Equal(snapshot, desired))
                return;
            
            CheckCollisions(desired);
            var desiredSet = new HashSet<ScreenRecord>();
            
            foreach (var item in desired)
                desiredSet.Add(item.Record);
            
            foreach (var item in snapshot)
                if (!desiredSet.Contains(item.Record))
                    item.Record.HideInstance(releaseMode);
            
            foreach (var item in desired)
                item.Record.ShowInstance(item.Order);
            
            _history.Push(snapshot);
        }
        
        private List<ScreenHistory.Item> Snapshot()
        {
            var snapshot = new List<ScreenHistory.Item>();
            foreach (var record in _store.Records)
                if (record.IsActive)
                    snapshot.Add(new ScreenHistory.Item(record, record.Order));
            
            snapshot.Sort((a, b) =>
            {
                var order = a.Order.CompareTo(b.Order);
                return order != 0 ? order : a.Record.Instance.transform.GetSiblingIndex()
                    .CompareTo(b.Record.Instance.transform.GetSiblingIndex());
            });
            
            return snapshot;
        }

        private void Restore(List<ScreenHistory.Item> snapshot)
        {
            var keep = new HashSet<ScreenRecord>();
            foreach (var item in snapshot)
                keep.Add(item.Record);
            
            foreach (var record in _store.Records)
                if (record.IsActive && !keep.Contains(record))
                    record.HideInstance(ScreenReleaseMode.KeepLoaded);
            
            CheckCollisions(snapshot);
            
            foreach (var item in snapshot)
                item.Record.ShowInstance(item.Order);
        }

        private async UniTask RestoreAssetsAsync(List<ScreenHistory.Item> snapshot, CancellationToken ct)
        {
            var loadedNow = new List<ScreenRecord>();
            var createdNow = new List<ScreenRecord>();
            try
            {
                foreach (var item in snapshot)
                {
                    var record = item.Record;
                    if (record.Prefab == null)
                    {
                        loadedNow.Add(record);
                        await record.LoadPrefabAsync(ct);
                    }
                    
                    if (record.Instance == null)
                    {
                        record.CreateInstance(record.Factory ?? _defaultFactory, _parent);
                        createdNow.Add(record);
                    }
                    
                    record.ValidateCanvas();
                }
            }
            catch
            {
                Rollback(loadedNow, createdNow);
                throw;
            }
        }
        
        private void PlaceInQueue()
        {
            if (_busy) 
                throw new InvalidOperationException("Another UI operation is running");
            
            _busy = true;
        }

        private async UniTask PlaceInQueueAsync(CancellationToken ct)
        {
            while (_busy)
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            
            _busy = true;
        }
        
        private static void Rollback(List<ScreenRecord> loadedNow, List<ScreenRecord> createdNow)
        {
            foreach (var record in createdNow)
                if (!loadedNow.Contains(record)) 
                    record.DestroyInstance();
            
            foreach (var record in loadedNow)
                record.Release();
        }

        private static int GetNextOrder(List<ScreenHistory.Item> items)
        {
            var max = -1;
            foreach (var item in items)
                max = Math.Max(max, item.Order);
            
            return checked(max + 1);
        }

        private static bool Equal(List<ScreenHistory.Item> left, List<ScreenHistory.Item> right)
        {
            if (left.Count != right.Count)
                return false;
            
            for (var i = 0; i < left.Count; i++)
                if (left[i].Record != right[i].Record || left[i].Order != right[i].Order) return false;
            
            return true;
        }

        private static void CheckCollisions(List<ScreenHistory.Item> items)
        {
            var used = new HashSet<int>();
            foreach (var item in items)
                if (!used.Add(item.Order))
                    Debug.LogError($"UI sorting order {item.Order} is already occupied; screens will overlap");
        }
        
        internal readonly struct Request
        {
            public readonly string Id;
            public readonly AssetReferenceGameObject Reference;
            public readonly bool HasReference;

            public Request(string id) { Id = id; Reference = null; HasReference = false; }
            public Request(string id, AssetReferenceGameObject reference) { Id = id; Reference = reference; HasReference = true; }
        }
    }
}
