using System.Collections.Generic;
using UnityEngine;

namespace App.InteractiveObjects
{
    public class InteractiveObj : MonoBehaviour, IInteractiveObj
    {
        [SerializeField] private bool isActive = true;
        [SerializeField] private bool isInteractable = true;
        [SerializeField] private List<MeshFilter> meshFilters;

        public bool IsActive => isActive;
        public bool IsInteractable => isInteractable;
        public GameObject Go => gameObject;
        public IReadOnlyList<MeshFilter> MeshFilters => meshFilters;

        public void SetActivityState(bool isActiveState)
        {
            isActive = isActiveState;
        }
    }
}