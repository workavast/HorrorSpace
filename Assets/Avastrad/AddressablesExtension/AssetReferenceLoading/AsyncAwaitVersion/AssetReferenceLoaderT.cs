using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Avastrad.AddressablesExtension.AssetReferenceLoading.AsyncAwaitVersion
{
    public class AssetReferenceLoaderT<T> : IDisposable
        where T : class
    {
        private readonly List<Task> _loadTasks = new();
        private readonly List<CancelMarker> _cancelMarkers = new();

        public T Asset { get; private set; }
        public bool IsReleaseProcess { get; private set; }

        public event Action OnLoaded; 
        public event Action OnReleased;
        
        /// <summary>
        /// same as TaskLoad, but without task return
        /// </summary>
        public void Load(AssetReference targetAssetReference, Action onLoadedCallback = null) 
            => TaskLoad(targetAssetReference, onLoadedCallback);
        
        /// <summary>
        /// same as Load, but with task return
        /// </summary>
        public async Task TaskLoad(AssetReference targetAssetReference, Action onLoadedCallback = null)
        {
            if (IsReleaseProcess)
            {
                Debug.LogWarning("You are trying load an asset while releasing an asset");
                return;
            }
            
            if (_cancelMarkers.Count > 0)
            {
                var res = _cancelMarkers.Find(cm => cm.AssetReference == targetAssetReference);

                foreach (var cancelMarker in _cancelMarkers) 
                    cancelMarker.IsCanceled = true;
                
                if (res != null)
                    res.IsCanceled = false;
            }

            var loadTask = LoadAsset(targetAssetReference, onLoadedCallback);
            _loadTasks.Add(loadTask);
            await loadTask;
            _loadTasks.Remove(loadTask);
        }

        /// <summary>
        /// release all asset that in process of loading and asset that already loaded
        /// </summary>
        public void Release(Action onReleaseCallback = null) 
            => ReleaseAssets(onReleaseCallback);

        private async Task LoadAsset(AssetReference skinsConfigReference, Action onLoadedCallback)
        {
            var cancelMarker = new CancelMarker(skinsConfigReference);
            _cancelMarkers.Add(cancelMarker);

            var handle = Addressables.LoadAssetAsync<T>(skinsConfigReference);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (cancelMarker.IsCanceled)
                {
                    Debug.Log($"release handle");
                    Addressables.Release(handle);
                }
                else
                {
                    var prevAsset = Asset;
                    Asset = handle.Result;

                    if (!IsAnyNull(prevAsset))
                    {
                        Debug.Log($"{prevAsset == null} {IsAnyNull(prevAsset)} \n {prevAsset}");
                        Addressables.Release(prevAsset);
                    }
                }
            }
            else
            {
                Debug.LogError($"asset doesnt loaded. Status: {handle.Status}");
            }
            
            _cancelMarkers.Remove(cancelMarker);
            onLoadedCallback?.Invoke();
            OnLoaded?.Invoke();
        }

        private async void ReleaseAssets(Action onReleasedCallback = null)
        {
            IsReleaseProcess = true;
            
            foreach (var cancelMarker in _cancelMarkers) 
                cancelMarker.IsCanceled = true;

            await Task.WhenAll(_loadTasks);

            if (!IsAnyNull(Asset))
            {
                Addressables.Release(Asset);
                Asset = null;
            }

            IsReleaseProcess = false;
            onReleasedCallback?.Invoke();
            OnReleased?.Invoke();
        }
        
        public void Dispose()
        {
            if (!IsAnyNull(Asset))
            {
                Addressables.Release(Asset);
                Asset = null;
            }

            if (!IsReleaseProcess)
                Release();
        }
        
        private static bool IsAnyNull<TValue>(TValue value) 
            => value == null || ((value is UnityEngine.Object) && (value as UnityEngine.Object) == null);

        private class CancelMarker
        {
            public bool IsCanceled;
            public readonly AssetReference AssetReference;

            public CancelMarker(AssetReference assetReference)
            {
                IsCanceled = false;
                AssetReference = assetReference;
            }
        }
    }
}