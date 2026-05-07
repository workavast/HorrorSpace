using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    [CreateAssetMenu(fileName = nameof(ScreenIdConfig), menuName = "Avastrad/UI/" + nameof(ScreenIdConfig))]
    public sealed class ScreenIdConfig : ScriptableObject
    {
        [SerializeField] private string[] ids = Array.Empty<string>();

        public IReadOnlyList<string> Ids => ids;
        public bool HasIds => ids != null && ids.Length > 0;

        public bool ContainsId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            for (var i = 0; i < ids.Length; i++)
                if (string.Equals(ids[i], id, StringComparison.Ordinal))
                    return true;

            return false;
        }
    }
}