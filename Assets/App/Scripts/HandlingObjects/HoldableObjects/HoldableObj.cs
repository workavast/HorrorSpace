using UnityEngine;

namespace App.HandlingObjects.HoldableObjects
{
    public class HoldableObj : MonoBehaviour, IHoldableObj
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

        public void Drop()
        {
            // Re-enable gravity
            rb.useGravity = true;
        }
    }
}