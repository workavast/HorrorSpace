using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Avastrad.UI.UiSystem
{
    internal sealed class ScreenRecord
    {
        public readonly string Id;
        public readonly AssetReferenceGameObject Reference;
        public AsyncOperationHandle<GameObject> Handle;
        public bool HasHandle;
        public ScreenBase Prefab;
        public ScreenBase Instance;
        public IUIScreenFactory Factory;
        public bool IsActive;
        public int Order;

        public ScreenRecord(string id, AssetReferenceGameObject reference)
        {
            Id = id;
            Reference = reference;
        }
    }
}
