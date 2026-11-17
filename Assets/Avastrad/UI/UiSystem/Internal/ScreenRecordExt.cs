using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Avastrad.UI.UiSystem
{
    internal static class ScreenRecordExt
    {
        public static async UniTask LoadPrefabAsync(this ScreenRecord record, CancellationToken ct)
        {
            if (record.Prefab != null)
                return;

            var handle = Addressables.LoadAssetAsync<GameObject>(record.Reference);
            record.Handle = handle;
            record.HasHandle = true;
            try
            {
                await UniTask.WaitUntil(() => handle.IsDone, cancellationToken: ct);
                record.TransferPrefabFromAddressables();
            }
            catch
            {
                record.Release();
                throw;
            }
        }

        public static void LoadPrefab(this ScreenRecord record)
        {
            if (record.Prefab != null)
                return;

#if UNITY_WEBGL && !UNITY_EDITOR
            throw new InvalidOperationException("Screen can't be loaded synced on the WebGL. Use LoadAsync on WebGL.");
#else
            var handle = Addressables.LoadAssetAsync<GameObject>(record.Reference);
            record.Handle = handle;
            record.HasHandle = true;
            try
            {
                handle.WaitForCompletion();
                record.TransferPrefabFromAddressables();
            }
            catch
            {
                record.Release();
                throw;
            }
#endif
        }
        
        public static void Release(this ScreenRecord record)
        {
            record.DestroyInstance();

            if (record.HasHandle && record.Handle.IsValid())
                Addressables.Release(record.Handle);
            
            record.Handle = default;
            record.HasHandle = false;
            record.Prefab = null;
            record.IsActive = false;
        }
        
        public static void CreateInstance(this ScreenRecord record, IUIScreenFactory factory, Transform parent)
        {
            if (record.Instance != null)
                return;
            if (record.Prefab == null)
                throw new InvalidOperationException($"Screen prefab '{record.Id}' is not loaded");

            var instance = factory.Create(record.Prefab, parent);
            if (instance == null)
                throw new InvalidOperationException($"Factory returned no screen for '{record.Id}'");
            
            record.Instance = instance;
            record.Factory = factory;
            try
            {
                instance.AssignScreenId(record.Id);
                instance.PrepareHidden();
                instance.Initialize();
            }
            catch
            {
                record.DestroyInstance();
                throw;
            }
        }
        
        public static void DestroyInstance(this ScreenRecord record)
        {
            if (record.Instance == null)
                return;

            try
            {
                record.Instance.PrepareHidden();
            }
            finally
            {
                UnityEngine.Object.Destroy(record.Instance.gameObject);
                record.Instance = null;
                record.IsActive = false;
            }
        }
        
        public static void ShowInstance(this ScreenRecord record, int order)
        {
            var canvas = record.Instance.GetComponentInChildren<Canvas>(true);
            record.Order = order;
            record.Instance.transform.SetAsLastSibling();
            
            if (!record.IsActive)
                record.Instance.SetActive(true);
            
            canvas.overrideSorting = true;
            canvas.sortingOrder = order;
            record.IsActive = true;
        }

        public static void HideInstance(this ScreenRecord record, ScreenReleaseMode mode)
        {
            if (record.IsActive)
                record.Instance.SetActive(false);
            
            record.IsActive = false;
            
            if (mode == ScreenReleaseMode.Release) 
                record.Release();
        }
        
        public static void ValidateCanvas(this ScreenRecord record)
        {
            if (record.Instance.GetComponentInChildren<Canvas>(true) == null)
                throw new InvalidOperationException($"Screen '{record.Id}' has no Canvas");
        }

        public static void ValidateFactory(this ScreenRecord record, IUIScreenFactory factory)
        {
            if (factory != null && record.Instance != null && !ReferenceEquals(factory, record.Factory))
                throw new InvalidOperationException($"Screen '{record.Id}' was created by another factory; release it before changing factory");
        }

        private static void TransferPrefabFromAddressables(this ScreenRecord record)
        {
            if (record.Handle.Status != AsyncOperationStatus.Succeeded || record.Handle.Result == null)
                throw new InvalidOperationException($"Failed to load screen '{record.Id ?? record.Reference.AssetGUID}'", record.Handle.OperationException);

            record.Prefab = record.Handle.Result.GetComponent<ScreenBase>();
            if (record.Prefab == null)
                throw new InvalidOperationException($"Addressable '{record.Reference.AssetGUID}' has no ScreenBase on its root");
        }
    }
}
