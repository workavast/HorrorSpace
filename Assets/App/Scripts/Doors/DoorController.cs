using System;
using App.Character;
using UnityEngine;

namespace App.Doors
{
    public class DoorController : MonoBehaviour
    {
        [field: SerializeField] public bool IsUnLocked { get; private set; } = true;
        [SerializeField] private DoorAnimator[] targetDoors;

        public event Action OnUse;
        public event Action<bool> OnTryUse;
        public event Action OnLockStateChanged; 
        
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
            
            foreach (var door in targetDoors) 
                door.Toggle();
            
            OnUse?.Invoke();
            OnTryUse?.Invoke(true);
            return true;
        }
    }
}
