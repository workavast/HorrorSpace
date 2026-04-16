using System;
using UnityEngine;

namespace App.Doors
{
    public class DoorController : MonoBehaviour
    {
        [field: SerializeField] public bool IsLocked { get; private set; }
        [SerializeField] private DoorAnimator doorAnimator;
        [SerializeField] private bool isOpen;
        [Header("Check obstacles in door")]
        [SerializeField] private BoxCollider closeCheckCollider;
        [SerializeField] private LayerMask checkLayers;
        
        public bool IsOpen => isOpen;
        
        public event Action OnUse;
        public event Action<bool> OnTryUse;
        public event Action OnLockStateChanged;

        public void Initialize()
        {
            doorAnimator.SetOpen(isOpen);
        }
        
        public void SetLockState(bool isLocked)
        {
            IsLocked = isLocked;
            OnLockStateChanged?.Invoke();
        }
        
        public bool TryToggle()
        {
            if (IsLocked)
            {
                OnTryUse?.Invoke(false);
                return false;
            }

            var targetIsOpen = !isOpen;
            if (!targetIsOpen && !CanBeClosed())
                return false;
            
            isOpen = targetIsOpen;
            doorAnimator.SetOpen(isOpen);
            
            OnUse?.Invoke();
            OnTryUse?.Invoke(true);
            return true;
        }
        
        private bool CanBeClosed() 
            => !Physics.CheckBox(closeCheckCollider.transform.position, closeCheckCollider.size / 2, 
                closeCheckCollider.transform.rotation, checkLayers, QueryTriggerInteraction.Ignore);
    }
}
