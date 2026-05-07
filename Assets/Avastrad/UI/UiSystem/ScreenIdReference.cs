using System;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    [Serializable]
    public class ScreenIdReference
    {
        [SerializeField] private ScreenIdConfig config;
        [SerializeField] private string id;

        public string Id => HasValidId ? id : string.Empty;
        public bool HasValidId => config != null && !string.IsNullOrEmpty(id) && config.ContainsId(id);
    }
}