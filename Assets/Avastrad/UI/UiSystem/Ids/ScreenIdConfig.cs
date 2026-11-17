using System;
using System.Collections.Generic;
using UnityEngine;

namespace Avastrad.UI.UiSystem.Ids
{
    [CreateAssetMenu(fileName = nameof(ScreenIdConfig), menuName = "Avastrad/UI/" + nameof(ScreenIdConfig))]
    public sealed class ScreenIdConfig : ScriptableObject
    {
        [SerializeField] private string[] _ids = Array.Empty<string>();

        public IReadOnlyList<string> Ids => _ids;
        public bool HasIds => _ids != null && _ids.Length > 0;

        public bool ContainsId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            for (var i = 0; i < _ids.Length; i++)
                if (string.Equals(_ids[i], id, StringComparison.Ordinal))
                    return true;

            return false;
        }
    }
}
