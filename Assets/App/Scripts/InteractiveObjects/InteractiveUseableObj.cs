using App.Usable;
using UnityEngine;

namespace App.InteractiveObjects
{
    public class InteractiveUsableObj : MonoBehaviour, IInteractiveObj
    {
        [SerializeField] private InterfaceReference<IUsable> usable;
        
        public bool IsActive => usable.Value.IsUsable;
        public bool IsInteractable => usable.Value.IsUsable;
        public GameObject Go => gameObject;
    }
}