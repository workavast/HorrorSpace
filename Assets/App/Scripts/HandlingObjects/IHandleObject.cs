using UnityEngine;

namespace App.HandlingObjects
{
    public interface IHandleObject
    {
        public bool CanBeHandled { get; }
        
        public Transform Transform { get; }
        public Rigidbody Rb { get; }

        public void Drop();
    }
}