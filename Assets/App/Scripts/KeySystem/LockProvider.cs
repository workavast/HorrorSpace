using System;
using UnityEngine;

namespace App.KeySystem
{
    public class LockProvider : MonoBehaviour
    {
        [SerializeField] private KeyConfig requiredConfig;
        [SerializeField] private bool isActive;

        public event Action OnInvalidType;
        public event Action OnInvalidKey;
        public event Action OnUnlocked;

        public void SetActivityState(bool state)
        {
            isActive = state;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!isActive)
                return;
            
            var keyObject = other.GetComponent<IKeyObject>();
            if (keyObject == null) 
                return;
            
            if (keyObject.GetKeyType() != requiredConfig.KeyType)
            {
                OnInvalidType?.Invoke();
                return;
            }
            
            if (keyObject.GetKey() != requiredConfig.Key)
            {
                OnInvalidKey?.Invoke();
                return;
            }
            
            Debug.Log("Unlocked!");
            OnUnlocked?.Invoke();
        }
    }
}