using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.TriggerZones
{
    public class TriggerZoneProviderT<TComponent> : MonoBehaviour
    {
        private readonly List<TComponent> _enters = new();
        
        public event Action<TComponent> OnEnter;
        public event Action<TComponent> OnExit;

        private void OnDisable()
        {
            for (var i = 0; i < _enters.Count; i++)
                OnTriggerExit(_enters[0]);
        }
        
        public void SetActive(bool isActive) 
            => gameObject.SetActive(isActive);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TComponent component))
            {
                _enters.Add(component);
                OnEnter?.Invoke(component);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out TComponent component))
                OnTriggerExit(component);
        }
        
        private void OnTriggerExit(TComponent component)
        {
            _enters.Remove(component);
            OnExit?.Invoke(component);
        }
    }
}