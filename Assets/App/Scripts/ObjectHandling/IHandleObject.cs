using UnityEngine;

namespace App.ObjectHandling
{
    public interface IHandleObject
    {
        public bool CanBeHandled { get; }
        public GameObject GetGameObject();
        public void OnPickup();
        public void OnDrop();
    }
}