using System;
using System.Collections.Generic;
using Avastrad.UI.UiSystem.Ids;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Avastrad.UI.UiSystem
{
    [CreateAssetMenu(fileName = nameof(ScreenCatalog), menuName = "Avastrad/UI/Screen Catalog")]
    public sealed class ScreenCatalog : ScriptableObject
    {
        [SerializeField] private Entry[] _entries = Array.Empty<Entry>();

        public IReadOnlyList<Entry> Entries => _entries;

        public bool TryResolve(string id, out AssetReferenceGameObject reference)
        {
            foreach (var entry in _entries)
            {
                if (entry.Id != null && string.Equals(entry.Id.Id, id, StringComparison.Ordinal))
                {
                    reference = entry.Prefab;
                    return reference != null && reference.RuntimeKeyIsValid();
                }
            }

            reference = null;
            return false;
        }
        
        [Serializable]
        public struct Entry
        {
            public ScreenIdReference Id;
            public AssetReferenceGameObject Prefab;
        }
    }
}
