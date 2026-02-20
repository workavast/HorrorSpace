using App.Character;
using UnityEngine;

namespace App.Usable
{
    public class UsableProvider : MonoBehaviour, IUsable
    {
        [SerializeField] private InterfaceReference<IUsable> interactableSource;
        
        public bool IsInteractable => interactableSource.Value.IsInteractable;
        
        public void Interact(PlayerContext playerContext) 
            => interactableSource.Value.Interact(playerContext);
    }
}