using UnityEngine;

namespace App.InteractiveObjects
{
    public interface IInteractiveObj
    {
        public bool IsInteractable { get; }
        public bool IsActive { get; }
        public GameObject Go { get; }
    }
}