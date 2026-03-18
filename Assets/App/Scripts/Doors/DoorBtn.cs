using System;
using App.Character;
using App.Usable;
using UnityEngine;

namespace App.Doors
{
    public class DoorBtn : MonoBehaviour, IUsable
    {
        [SerializeField] private DoorController door;
        [SerializeField] private bool isSelfUsable = true;

        public bool IsUsable => door.IsUnLocked && isSelfUsable;

        public event Action OnUse;
        public event Action<bool> OnTryUse;
        public event Action OnUsableStateChanged;

        private void Awake() 
            => door.OnLockStateChanged += UsableStateChanged;

        private void OnDestroy() 
            => door.OnLockStateChanged -= UsableStateChanged;

        public void Interact(PlayerContext playerContext)
        {
            if (!isSelfUsable)
            {
                OnTryUse?.Invoke(false);
                return;
            }
            
            var result = door.TryToggle();

            OnTryUse?.Invoke(result);
            if (result) 
                OnUse?.Invoke();
        }
        
        public void SetSelfInteractable(bool isUsable)
        {
            isSelfUsable = isUsable;
            OnUsableStateChanged?.Invoke();
        }
        
        private void UsableStateChanged() 
            => OnUsableStateChanged?.Invoke();
    }
}