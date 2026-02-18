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
            // Disable physics while holding
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            if (col != null)
            {
                col.enabled = false;
            }
        }

        public void OnDrop()
        {
            // Re-enable physics
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            if (col != null)
            {
                col.enabled = true;
            }
        }
    }
}