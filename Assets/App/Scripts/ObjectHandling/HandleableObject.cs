using UnityEngine;

namespace App.ObjectHandling
{
    public class HandleableObject : MonoBehaviour, IHandleObject
    {
        [SerializeField] private bool canBeHandled = true;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider col;

        public bool CanBeHandled => canBeHandled;
        public Transform Transform => transform;
        public Rigidbody Rb => rb;

        public void OnPickup()
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        public void OnDrop()
        {
            // Re-enable gravity
            rb.useGravity = true;
        }
    }
}