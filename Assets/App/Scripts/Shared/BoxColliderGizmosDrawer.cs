using UnityEngine;

namespace App
{
    public class BoxColliderGizmosDrawer : MonoBehaviour
    {
        private BoxCollider _boxCollider;
        
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnDrawGizmos()
        {
            var boxCollider = _boxCollider;
            if (!Application.isPlaying)
                boxCollider = GetComponent<BoxCollider>();
            
            transform.DrawCube(boxCollider);
        }
    }
}