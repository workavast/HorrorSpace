using System.Collections.Generic;
using UnityEngine;

namespace App.InteractiveObjects
{
    public interface IInteractiveObj
    {
        /// <summary>
        /// should be shown or not interaction view
        /// </summary>
        public bool IsActive { get; }
        
        /// <summary>
        /// interaction will be success or not
        /// </summary>
        public bool IsInteractable { get; }
        
        public GameObject Go { get; }
        public IReadOnlyList<MeshFilter> MeshFilters { get; }
    }
}