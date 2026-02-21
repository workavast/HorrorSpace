using App.Character;
using App.Usable;
using UnityEngine;

namespace App.Doors
{
    public class DoorButton : MonoBehaviour, IUsable
    {
        [field: SerializeField] public bool IsInteractable { get; private set; } = true;
        [SerializeField] private DoorController[] targetDoors;

        public void SetState(bool isInteractable)
        {
            IsInteractable = isInteractable;
        }
        
        public void Interact(PlayerContext playerContext)
        {
            if (!IsInteractable)
                return;
            
            foreach (var door in targetDoors) 
                door.Toggle();
        }
    }
}
