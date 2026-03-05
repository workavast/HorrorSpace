using UnityEngine;

namespace App
{
    [RequireComponent(typeof(Rigidbody))]
    public class Gizmo_RigidBodyCenterOfMass : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private const float SphereRadius = 0.2f; 
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (Application.isPlaying)
                Gizmos.DrawSphere(transform.position + _rigidbody.centerOfMass, SphereRadius);                
            else
                Gizmos.DrawSphere(transform.position + GetComponent<Rigidbody>().centerOfMass, SphereRadius);                
        }
    }
}