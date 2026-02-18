using UnityEngine;

namespace App.ObjectHandling
{
    public class HandleableObject : MonoBehaviour, IHandleObject
    {
        [SerializeField] private bool canBeHandled = true;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider col;

        public bool CanBeHandled => canBeHandled;

        public GameObject GetGameObject() => gameObject;

        public void OnPickup()
        {
            // Disable gravity but keep physics active for more realistic behavior
            if (rb != null)
            {
                rb.useGravity = false;
                rb.velocity = Vector3.zero; // Reset velocity
                rb.angularVelocity = Vector3.zero; // Reset angular velocity
            }
            // Keep collider enabled for collision detection
        }

        public void OnDrop()
        {
            // Re-enable gravity
            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
    }
}