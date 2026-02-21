using System;
using UnityEngine;

namespace App.TriggerZones
{
    public class TriggerZoneProvider : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action<Collider> OnTriggerExitEvent;
        
        private void OnTriggerEnter(Collider other) 
            => OnTriggerEnterEvent?.Invoke(other);

        private void OnTriggerExit(Collider other) 
            => OnTriggerExitEvent?.Invoke(other);
    }
}