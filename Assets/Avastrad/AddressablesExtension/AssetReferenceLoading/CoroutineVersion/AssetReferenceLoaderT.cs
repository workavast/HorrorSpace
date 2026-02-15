using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Avastrad.AddressablesExtension.AssetReferenceLoading.CoroutineVersion
{
    public class AssetReferenceLoaderT<T> : IDisposable
        where T : class
    {
        private readonly MonoBehaviour _parent;
        private readonly List<CancelMarker> _cancelMarkers = new();

        public T Asset { get; private set; }
        public bool IsUnLoadProcess { get; private set; }

        public event Action OnLoaded; 
        public event Action OnUnLoaded;
        
        public AssetReferenceLoaderT(MonoBehaviour parent)
        {
            _parent = parent;
        }
        
        public void Load(AssetReference targetAssetReference, Action onLoadedCallback = null)
        {
            if (IsUnLoadProcess)
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

            _parent.StartCoroutine(LoadAsset(targetAssetReference, onLoadedCallback));
        }

        public void Release(Action onAllUnLoaded = null)
        {
            if(_parent != null)
                _parent.StartCoroutine(ReleaseAssets(onAllUnLoaded));
        }

        private IEnumerator LoadAsset(AssetReference skinsConfigReference, Action onLoadedCallback)
        {
            var cancelMarker = new CancelMarker(skinsConfigReference);
            _cancelMarkers.Add(cancelMarker);

            var handle = Addressables.LoadAssetAsync<T>(skinsConfigReference);
            yield return handle;

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
                Debug.Log(handle.Status);
            }
            
            _cancelMarkers.Remove(cancelMarker);
            onLoadedCallback?.Invoke();
            OnLoaded?.Invoke();
        }

        private IEnumerator ReleaseAssets(Action onAllUnLoaded = null)
        {
            IsUnLoadProcess = true;
            
            foreach (var cancelMarker in _cancelMarkers) 
                cancelMarker.IsCanceled = true;

            while (_cancelMarkers.Count > 0)
                yield return new WaitForEndOfFrame();

            if (!IsAnyNull(Asset))
            {
                Debug.Log($"{Asset == null} {IsAnyNull(Asset)} \n {Asset}");
                Addressables.Release(Asset);
                Asset = null;
            }

            IsUnLoadProcess = false;
            onAllUnLoaded?.Invoke();
            OnUnLoaded?.Invoke();
        }
        
        public void Dispose()
        {
            if (!IsAnyNull(Asset))
            {
                Debug.Log($"{Asset == null} {IsAnyNull(Asset)} \n {Asset}");
                Addressables.Release(Asset);
                Asset = null;
            }

            if (!IsUnLoadProcess)
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