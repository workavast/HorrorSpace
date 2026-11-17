using UnityEngine.AddressableAssets;

namespace Avastrad.UI.UiSystem
{
    public readonly struct ScreenDefinition
    {
        public string Id { get; }
        public AssetReferenceGameObject Reference { get; }

        public ScreenDefinition(string id, AssetReferenceGameObject reference)
        {
            Id = id;
            Reference = reference;
        }
    }
}
