using App.Character;
using UnityEngine;

namespace App.Interaction
{
    public class InteractableProvider : MonoBehaviour, IInteractable
    {
        [SerializeField] private InterfaceReference<IInteractable> interactableSource;
        
        public bool IsInteractable => interactableSource.Value.IsInteractable;
        
        public void Interact(PlayerContext playerContext) 
            => interactableSource.Value.Interact(playerContext);
    }
}