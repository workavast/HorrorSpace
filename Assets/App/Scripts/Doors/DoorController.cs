using System;
using App.Audio;
using App.Character;
using UnityEngine;

namespace App.Doors
{
    public class DoorController : MonoBehaviour
    {
        [field: SerializeField] public bool IsUnLocked { get; private set; } = true;
        [SerializeField] private DoorAnimator[] targetDoors;
        [SerializeField] private bool isOpen;
        [SerializeField] private BoxCollider closeCheckCollider;
        [SerializeField] private LayerMask checkLayers;
        
        public event Action OnUse;
        public event Action<bool> OnTryUse;
        public event Action OnLockStateChanged;

        public void Initialize()
        {
            foreach (var door in targetDoors) 
                door.SetOpen(isOpen);
        }
        
        public void SetUnLockState(bool isUnLocked)
        {
            IsUnLocked = isUnLocked;
            OnLockStateChanged?.Invoke();
        }
        
        public bool Interact(PlayerContext playerContext)
        {
            if (!IsUnLocked)
            {
                OnTryUse?.Invoke(false);
                return false;
            }

            var targetIsOpen = !isOpen;
            if (!targetIsOpen && !CanBeLocked())
                return false;
            
            isOpen = targetIsOpen;
            foreach (var door in targetDoors) 
                door.SetOpen(isOpen);
            
            OnUse?.Invoke();
            OnTryUse?.Invoke(true);
            return true;
        }
        
        private bool CanBeLocked() 
            => !Physics.CheckBox(closeCheckCollider.transform.position, closeCheckCollider.size / 2, Quaternion.identity, checkLayers, QueryTriggerInteraction.Ignore);
    }
}
