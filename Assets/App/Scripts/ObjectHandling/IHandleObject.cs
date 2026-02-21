using UnityEngine;

namespace App.ObjectHandling
{
    public interface IHandleObject
    {
        public bool CanBeHandled { get; }
        
        public Transform Transform { get; }
        public Rigidbody Rb { get; }

        public void OnPickup();
        public void OnDrop();
    }
}