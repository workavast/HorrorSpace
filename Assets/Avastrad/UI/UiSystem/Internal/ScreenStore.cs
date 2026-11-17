using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace Avastrad.UI.UiSystem
{
    internal sealed class ScreenStore
    {
        private readonly List<ScreenRecord> _records = new();
        private readonly ScreenCatalog _catalog;

        public IReadOnlyList<ScreenRecord> Records => _records;

        public ScreenStore(ScreenCatalog catalog) => _catalog = catalog;

        public ScreenRecord Get(string id)
        {
            foreach (var record in _records)
                if (string.Equals(record.Id, id, StringComparison.Ordinal))
                    return record;
            return null;
        }
        
        public ScreenRecord GetOrCreate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Screen ID is empty", nameof(id));

            var existing = Get(id);
            if (existing != null)
                return existing;

            if (_catalog == null || !_catalog.TryResolve(id, out var reference))
                throw new KeyNotFoundException($"Screen ID '{id}' is not in the catalog");

            return GetOrCreate(id, reference);
        }

        public ScreenRecord GetOrCreate(string id, AssetReferenceGameObject reference)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Screen ID is invalid", nameof(id));
            if (reference == null || !reference.RuntimeKeyIsValid())
                throw new ArgumentException("Screen reference is invalid", nameof(reference));

            var existing = Get(id);
            if (existing != null)
            {
                if (existing.Reference.AssetGUID != reference.AssetGUID)
                    throw new InvalidOperationException($"Screen ID '{id}' is already bound to another prefab");
                return existing;
            }

            var created = new ScreenRecord(id, reference);
            _records.Add(created);
            return created;
        }
    }
}
