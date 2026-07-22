using System;
using UnityEngine;

namespace App.KeySystem
{
    public class KeyReader : MonoBehaviour
    {
        [SerializeField] private bool isActive;
        [SerializeField] private KeyConfig requiredConfig;
        [SerializeField] private KeyObjectTriggerZone[] triggerZones;

        public event Action<IKeyObject> OnInvalidType;
        public event Action<IKeyObject> OnInvalidKey;
        public event Action<IKeyObject> OnValidKey;

        private void Awake()
        {
            foreach (var triggerZone in triggerZones) 
                triggerZone.OnEnter += TryUnlock;
        }

        private void OnDestroy()
        {
            foreach (var triggerZone in triggerZones) 
                triggerZone.OnEnter -= TryUnlock;
        }

        public void SetActivityState(bool state)
        {
            isActive = state;
        }

        private void TryUnlock(IKeyObject keyObject)
        {
            if (!isActive)
                return;
            
            if (keyObject.GetKeyType() != requiredConfig.KeyType)
            {
                OnInvalidType?.Invoke(keyObject);
                return;
            }
            
            if (keyObject.GetKey() != requiredConfig.Key)
            {
                OnInvalidKey?.Invoke(keyObject);
                return;
            }
            
            OnValidKey?.Invoke(keyObject);
        }
    }
}