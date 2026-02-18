using App.Character;
using UnityEngine;

namespace App.Interaction
{
    public interface IInteractable
    {
        public bool IsInteractable { get; }
        
        public void Interact(PlayerContext playerContext);
        public GameObject GetGameObject();
    }
}
