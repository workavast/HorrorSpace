using UnityEngine;

namespace App.InteractiveObjects
{
    public class InteractiveObj : MonoBehaviour, IInteractiveObj
    {
        [SerializeField] private bool isActive = true;
        [SerializeField] private bool isInteractable = true;

        public bool IsActive => isActive;
        public bool IsInteractable => isInteractable;
        public GameObject Go => gameObject;

        public void SetActivityState(bool isActiveState)
        {
            isActive = isActiveState;
        }
    }
}