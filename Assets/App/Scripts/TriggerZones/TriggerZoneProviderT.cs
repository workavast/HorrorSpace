using System;
using UnityEngine;

namespace App.TriggerZones
{
    public class TriggerZoneProvider<TComponent> : MonoBehaviour
    {
        public event Action<TComponent> OnTriggerEnterEvent;
        public event Action<TComponent> OnTriggerExitEvent;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TComponent component)) 
                OnTriggerEnterEvent?.Invoke(component);
        }

        private void OnTriggerExit(Collider other)
        { 
            if (other.TryGetComponent(out TComponent component)) 
                OnTriggerExitEvent?.Invoke(component);
        }
    }
}