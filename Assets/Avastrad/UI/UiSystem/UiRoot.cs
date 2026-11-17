using System;
using System.Collections.Generic;
using System.Threading;
using Avastrad.UI.UiSystem.Ids;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Avastrad.UI.UiSystem
{
    [DisallowMultipleComponent]
    public sealed class UiRoot : MonoBehaviour
    {
        [SerializeField] private ScreenCatalog _catalog;
        [SerializeField] private MonoBehaviour _defaultFactory;

        private ScreenCoordinator _coordinator;
        private CancellationTokenSource _lifetimeCts;

        private ScreenCoordinator Coordinator
        {
            get
            {
                if (_coordinator == null) 
                    Initialize();
                return _coordinator;
            }
        }

        private void Awake()
            => Initialize();

        private void Initialize()
        {
            if (_coordinator != null)
                return;
            if (_defaultFactory != null && _defaultFactory is not IUIScreenFactory)
                throw new InvalidOperationException("UiRoot factory component must implement IUIScreenFactory");
            
            _lifetimeCts = new CancellationTokenSource();
            _coordinator = new ScreenCoordinator(_catalog, transform, _defaultFactory as IUIScreenFactory ?? new UnityScreenFactory());
        }

        private void OnDestroy()
        {
            _lifetimeCts?.Cancel();
            _coordinator?.Dispose();
            _lifetimeCts?.Dispose();
        }

        public void Add(string id, int? order = null, IUIScreenFactory factory = null)
            => Add(new[] { id }, order, factory);
        public void Add(string id, AssetReferenceGameObject reference, int? order = null, IUIScreenFactory factory = null)
            => Add(new[] { new ScreenDefinition(id, reference) }, order, factory);
        public void Add(IReadOnlyList<string> ids, int? order = null, IUIScreenFactory factory = null)
            => Coordinator.ChangeScreens(ToRequests(ids), false, order, ScreenReleaseMode.KeepLoaded, factory);
        public void Add(IReadOnlyList<ScreenIdReference> ids, int? order = null, IUIScreenFactory factory = null)
            => Coordinator.ChangeScreens(ToRequests(ids), false, order, ScreenReleaseMode.KeepLoaded, factory);
        public void Add(IReadOnlyList<ScreenDefinition> screens, int? order = null, IUIScreenFactory factory = null)
            => Coordinator.ChangeScreens(ToRequests(screens), false, order, ScreenReleaseMode.KeepLoaded, factory);

        public UniTask AddAsync(string id, int? order = null, IUIScreenFactory factory = null, CancellationToken ct = default)
            => AddAsync(new[] { id }, order, factory, ct);
        public UniTask AddAsync(string id, AssetReferenceGameObject reference, int? order = null, IUIScreenFactory factory = null, CancellationToken ct = default)
            => AddAsync(new[] { new ScreenDefinition(id, reference) }, order, factory, ct);
        public UniTask AddAsync(IReadOnlyList<string> ids, int? order = null, IUIScreenFactory factory = null, CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.ChangeScreensAsync(ToRequests(ids), false, order, ScreenReleaseMode.KeepLoaded, factory, ct2), ct);
        public UniTask AddAsync(IReadOnlyList<ScreenIdReference> ids, int? order = null, IUIScreenFactory factory = null, CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.ChangeScreensAsync(ToRequests(ids), false, order, ScreenReleaseMode.KeepLoaded, factory, ct2), ct);
        public UniTask AddAsync(IReadOnlyList<ScreenDefinition> screens, int? order = null, IUIScreenFactory factory = null, CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.ChangeScreensAsync(ToRequests(screens), false, order, ScreenReleaseMode.KeepLoaded, factory, ct2), ct);

        public void Set(string id, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null)
            => Set(new[] { id }, order, releaseMode, factory);
        public void Set(string id, AssetReferenceGameObject reference, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null)
            => Set(new[] { new ScreenDefinition(id, reference) }, order, releaseMode, factory);
        public void Set(IReadOnlyList<string> ids, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null)
            => Coordinator.ChangeScreens(ToRequests(ids), true, order, releaseMode, factory);
        public void Set(IReadOnlyList<ScreenIdReference> ids, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null)
            => Coordinator.ChangeScreens(ToRequests(ids), true, order, releaseMode, factory);
        public void Set(IReadOnlyList<ScreenDefinition> screens, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null)
            => Coordinator.ChangeScreens(ToRequests(screens), true, order, releaseMode, factory);

        public UniTask SetAsync(string id, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null, CancellationToken ct = default)
            => SetAsync(new[] { id }, order, releaseMode, factory, ct);
        public UniTask SetAsync(string id, AssetReferenceGameObject reference, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null, CancellationToken cancellationToken = default)
            => SetAsync(new[] { new ScreenDefinition(id, reference) }, order, releaseMode, factory, cancellationToken);
        public UniTask SetAsync(IReadOnlyList<string> ids, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null, CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.ChangeScreensAsync(ToRequests(ids), true, order, releaseMode, factory, ct2), ct);
        public UniTask SetAsync(IReadOnlyList<ScreenIdReference> ids, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null, CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.ChangeScreensAsync(ToRequests(ids), true, order, releaseMode, factory, ct2), ct);
        public UniTask SetAsync(IReadOnlyList<ScreenDefinition> screens, int? order = null, ScreenReleaseMode releaseMode = ScreenReleaseMode.KeepLoaded, IUIScreenFactory factory = null,
            CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.ChangeScreensAsync(ToRequests(screens), true, order, releaseMode, factory, ct2), ct);

        public void Remove(string id, ScreenReleaseMode mode = ScreenReleaseMode.KeepLoaded)
            => Remove(new[] { id }, mode);
        public void Remove(string id, AssetReferenceGameObject reference, ScreenReleaseMode mode = ScreenReleaseMode.KeepLoaded)
            => Remove(new[] { new ScreenDefinition(id, reference) }, mode);
        public void Remove(IReadOnlyList<string> ids, ScreenReleaseMode mode = ScreenReleaseMode.KeepLoaded)
            => Coordinator.Remove(ToRequests(ids), mode);
        public void Remove(IReadOnlyList<ScreenIdReference> ids, ScreenReleaseMode mode = ScreenReleaseMode.KeepLoaded)
            => Coordinator.Remove(ToRequests(ids), mode);
        public void Remove(IReadOnlyList<ScreenDefinition> screens,
            ScreenReleaseMode mode = ScreenReleaseMode.KeepLoaded)
            => Coordinator.Remove(ToRequests(screens), mode);

        public void Preload(string id, bool createHidden = false, IUIScreenFactory factory = null)
            => Preload(new[] { id }, createHidden, factory);
        public void Preload(string id, AssetReferenceGameObject reference, bool createHidden = false, IUIScreenFactory factory = null)
            => Preload(new[] { new ScreenDefinition(id, reference) }, createHidden, factory);
        public void Preload(IReadOnlyList<string> ids, bool createHidden = false, IUIScreenFactory factory = null)
            => Coordinator.Preload(ToRequests(ids), createHidden, factory);
        public void Preload(IReadOnlyList<ScreenIdReference> ids, bool createHidden = false, IUIScreenFactory factory = null)
            => Coordinator.Preload(ToRequests(ids), createHidden, factory);
        public void Preload(IReadOnlyList<ScreenDefinition> screens, bool createHidden = false, IUIScreenFactory factory = null)
            => Coordinator.Preload(ToRequests(screens), createHidden, factory);

        public UniTask PreloadAsync(string id, bool createHidden = false, IUIScreenFactory factory = null, CancellationToken ct = default)
            => PreloadAsync(new[] { id }, createHidden, factory, ct);
        public UniTask PreloadAsync(string id, AssetReferenceGameObject reference, bool createHidden = false, IUIScreenFactory factory = null, CancellationToken cancellationToken = default)
            => PreloadAsync(new[] { new ScreenDefinition(id, reference) }, createHidden, factory, cancellationToken);
        public UniTask PreloadAsync(IReadOnlyList<string> ids, bool createHidden = false, IUIScreenFactory factory = null, CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.PreloadAsync(ToRequests(ids), createHidden, factory, ct2), ct);
        public UniTask PreloadAsync(IReadOnlyList<ScreenIdReference> ids, bool createHidden = false, IUIScreenFactory factory = null, CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.PreloadAsync(ToRequests(ids), createHidden, factory, ct2), ct);
        public UniTask PreloadAsync(IReadOnlyList<ScreenDefinition> screens, bool createHidden = false, IUIScreenFactory factory = null, CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.PreloadAsync(ToRequests(screens), createHidden, factory, ct2), ct);

        public async UniTask PreloadCatalogAsync(bool createHidden = false, CancellationToken ct = default)
        {
            if (_catalog == null)
                throw new InvalidOperationException("UiRoot has no ScreenCatalog");
            
            var ids = new List<string>(_catalog.Entries.Count);
            foreach (var entry in _catalog.Entries)
                ids.Add(entry.Id?.Id);
            
            await PreloadAsync(ids, createHidden, ct: ct);
        }
        
        public void Clear(ScreenReleaseMode mode = ScreenReleaseMode.KeepLoaded) 
            => Coordinator.Clear(mode);
        
        public void Undo() 
            => Coordinator.Undo();
        
        public UniTask UndoAsync(CancellationToken ct = default)
            => RunAsync(ct2 => Coordinator.UndoAsync(ct2), ct);

        public bool TryGetActive<T>(string id, out T screen) where T : ScreenBase
            => Coordinator.TryGetActive(id, out screen);

        private async UniTask RunAsync(Func<CancellationToken, UniTask> action, CancellationToken supplied)
        {
            _ = Coordinator;
            if (!supplied.CanBeCanceled)
            {
                await action(_lifetimeCts.Token);
                return;
            }

            using var linked = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeCts.Token, supplied);
            await action(linked.Token);
        }
        
        private static ScreenCoordinator.Request[] ToRequests(IReadOnlyList<string> ids)
        {
            if (ids == null) 
                throw new ArgumentNullException(nameof(ids));
            
            var requests = new ScreenCoordinator.Request[ids.Count];
            for (var i = 0; i < ids.Count; i++)
                requests[i] = new ScreenCoordinator.Request(ids[i]);
            
            return requests;
        }

        private static ScreenCoordinator.Request[] ToRequests(IReadOnlyList<ScreenIdReference> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));
            
            var requests = new ScreenCoordinator.Request[ids.Count];
            for (var i = 0; i < ids.Count; i++)
                requests[i] = new ScreenCoordinator.Request(ids[i]?.Id);
            
            return requests;
        }

        private static ScreenCoordinator.Request[] ToRequests(IReadOnlyList<ScreenDefinition> screens)
        {
            if (screens == null)
                throw new ArgumentNullException(nameof(screens));
            
            var requests = new ScreenCoordinator.Request[screens.Count];
            for (var i = 0; i < screens.Count; i++)
                requests[i] = new ScreenCoordinator.Request(screens[i].Id, screens[i].Reference);
            
            return requests;
        }
    }
}
