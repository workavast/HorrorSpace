using UnityEngine;

namespace App.InteractiveObjects
{
    public class InteractiveObj : MonoBehaviour, IInteractiveObj
    {
        [SerializeField] private bool isInteractable = true;

        public bool IsInteractable => isInteractable;
        public GameObject Go => gameObject;
    }
}