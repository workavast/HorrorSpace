using System;
using UnityEngine;

namespace Avastrad.UI.UiSystem.Ids
{
    [Serializable]
    public class ScreenIdReference
    {
#if UNITY_EDITOR || DEBUG
        [SerializeField] private ScreenIdConfig _config;
#endif
        [SerializeField] private string _id;

        public string Id => IsValid ? _id : string.Empty;
        public bool IsValid => !string.IsNullOrEmpty(_id) && IdValidInConfig();

        public static implicit operator string(ScreenIdReference reference) 
            => reference == null ? string.Empty : reference.Id;

        private bool IdValidInConfig()
        {
#if UNITY_EDITOR || DEBUG
            if (_config == null)
            {
                Debug.LogError($"IdsConfig is null");
                return false;
            }

            if (!_config.ContainsId(_id))
            {
                Debug.LogError($"IdsConfig [{_config.name}] do not contains id [{_id}]");
                return false;
            }
#endif
            return true;
        }
    }
}
