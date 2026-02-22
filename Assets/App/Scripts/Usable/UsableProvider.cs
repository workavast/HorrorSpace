using System;
using App.Character;
using UnityEngine;

namespace App.Usable
{
    public class UsableProvider : MonoBehaviour, IUsable
    {
        [SerializeField] private InterfaceReference<IUsable> interactableSource;
        
        public bool IsUsable => interactableSource.Value.IsUsable;

        public event Action OnUse;
        public event Action<bool> OnTryUse;
        public event Action OnUsableStateChanged;

        private void Awake()
        {
            interactableSource.Value.OnUsableStateChanged += UsableStateChanged;
            interactableSource.Value.OnTryUse += TryUse;
            interactableSource.Value.OnUse += Use;
        }

        private void OnDestroy()
        {
            interactableSource.Value.OnUsableStateChanged -= UsableStateChanged;
            interactableSource.Value.OnTryUse -= TryUse;
            interactableSource.Value.OnUse -= Use;
        }
        
        public void Interact(PlayerContext playerContext) 
            => interactableSource.Value.Interact(playerContext);
        
        private void Use()
            => OnUse?.Invoke();
        
        private void TryUse(bool obj) 
            => OnTryUse?.Invoke(obj);

        private void UsableStateChanged() 
            => OnUsableStateChanged?.Invoke();
    }
}