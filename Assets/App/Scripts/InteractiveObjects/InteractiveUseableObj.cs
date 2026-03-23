using System.Collections.Generic;
using App.Usable;
using UnityEngine;

namespace App.InteractiveObjects
{
    public class InteractiveUsableObj : MonoBehaviour, IInteractiveObj
    {
        [SerializeField] private bool isActive = true;
        [SerializeField] private InterfaceReference<IUsable> usable;
        [SerializeField] private List<MeshFilter> meshFilters;
        
        public bool IsActive => isActive;
        public bool IsInteractable => usable.Value.IsUsable;
        public GameObject Go => gameObject;
        public IReadOnlyList<MeshFilter> MeshFilters => meshFilters;
    }
}