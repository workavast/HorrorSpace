using App.Character;
using UnityEngine;

namespace App.Usable
{
    public class UseButton : MonoBehaviour, IUsable
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
    }
}
