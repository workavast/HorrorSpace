using UnityEngine;
using App.Character;

namespace App.Interaction
{
    public class InteractButton : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public bool IsInteractable { get; private set; } = true;
        [SerializeField] private DoorController[] targetDoors;

        public void Interact(PlayerContext playerContext)
        {
            if (!IsInteractable)
                return;
            
            foreach (var door in targetDoors) 
                door.Toggle();
        }

        public GameObject GetGameObject() 
            => null;
    }
}
