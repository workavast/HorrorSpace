using App.Character;
using UnityEngine;

namespace App.ObjectHandling
{
    public class HandleObjectController : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float pickupRange = 3f;
        [SerializeField] private LayerMask handleLayerMask = ~0;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxDistance = 5f;
        [SerializeField] private float zoomSpeed = 1f;
        [SerializeField] private float rotationSensitivity = 1f;
        [SerializeField] private float attractionForce = 100f;
        [SerializeField] private float maxAttractionSpeed = 10f;
        [SerializeField] private float dampingFactor = 0.9f;
        [SerializeField] private float angularDampingPickup = 5f;
        [SerializeField] private float rotationReturnSpeed = 2f;
        [SerializeField] private float manualRotationSpeed = 5f;
        [SerializeField] private float torqueMultiplier = 1f;

        private IHandleObject _currentHandledObject;
        private float _currentDistance;
        private Vector3 _localOffset;
        private bool _isRotatingObject;
        private Quaternion _targetRotationRelativeToPlayer;

        // Input actions - assuming we add them to PlayerInputManager
        // For now, we'll use mouse buttons directly, but ideally integrate with Input System

        private void Update()
        {
            if (_currentHandledObject != null)
                HandleHeldObject();
            else
                CheckForPickup();
        }

        private void CheckForPickup()
        {
            // Check if pickup button is pressed
            if (inputManager.IsPickupPressed)
                TryPickupObject();
        }

        private void TryPickupObject()
        {
            var camera = cameraController.GetCamera();
            var ray = new Ray(camera.transform.position, camera.transform.forward);

            if (Physics.Raycast(ray, out var hit, pickupRange, handleLayerMask))
            {
                var handleObject = hit.collider.GetComponent<IHandleObject>() ??
                                   hit.collider.GetComponentInParent<IHandleObject>();

                if (handleObject != null && handleObject.CanBeHandled)
                    PickupObject(handleObject, hit.point);
            }
        }

        private void PickupObject(IHandleObject handleObject, Vector3 pickupPoint)
        {
            _currentHandledObject = handleObject;
            var objTransform = handleObject.GetGameObject().transform;

            // Calculate initial distance and offset
            var camera = cameraController.GetCamera();
            _currentDistance = Vector3.Distance(camera.transform.position, pickupPoint);
            _currentDistance = Mathf.Clamp(_currentDistance, minDistance, maxDistance);

            // Store target rotation relative to player (not camera)
            _targetRotationRelativeToPlayer = Quaternion.Inverse(playerTransform.rotation) * objTransform.rotation;

            // Don't instantly position object - let physics forces handle attraction
            // Object will naturally move to target position while keeping its orientation

            handleObject.OnPickup();

            // Increase angular damping for picked up object
            var rb = handleObject.GetGameObject().GetComponent<Rigidbody>();
            if (rb != null)
                rb.angularDamping = angularDampingPickup;

            // Disable interaction system - we'll need to notify PlayerCharacterController
            // For now, assume we have a way to disable it
        }

        private void HandleHeldObject()
        {
            var camera = cameraController.GetCamera();
            var objTransform = _currentHandledObject.GetGameObject().transform;
            var rb = _currentHandledObject.GetGameObject().GetComponent<Rigidbody>();

            // Handle zoom with mouse wheel
            var scrollDelta = inputManager.ZoomDelta;
            if (scrollDelta != 0)
            {
                _currentDistance -= scrollDelta * zoomSpeed * Time.deltaTime;
                _currentDistance = Mathf.Clamp(_currentDistance, minDistance, maxDistance);
            }

            // Handle rotation with rotate button
            if (inputManager.IsRotatePressed)
            {
                if (!_isRotatingObject)
                {
                    _isRotatingObject = true;
                    cameraController.SetCameraRotationEnabled(false);
                }

                // Rotate object based on mouse movement and update target rotation
                var mouseDelta = inputManager.LookInput;
                
                if (rb != null)
                {
                    // Apply manual rotation with higher speed
                    var torque = new Vector3(-mouseDelta.y, mouseDelta.x, 0) * manualRotationSpeed;
                    rb.AddTorque(torque);
                    
                    // Update target rotation based on new orientation
                    _targetRotationRelativeToPlayer = Quaternion.Inverse(playerTransform.rotation) * objTransform.rotation;
                }
                else
                {
                    objTransform.Rotate(Vector3.up, mouseDelta.x * rotationSensitivity, Space.World);
                    objTransform.Rotate(Vector3.right, -mouseDelta.y * rotationSensitivity, Space.Self);
                }
            }
            else
            {
                if (_isRotatingObject)
                {
                    _isRotatingObject = false;
                    cameraController.SetCameraRotationEnabled(true);
                }
            }

            // Apply physics forces to attract object to target position
            var targetPosition = camera.transform.position + camera.transform.forward * _currentDistance;
            if (rb != null)
            {
                var direction = targetPosition - objTransform.position;
                var distance = direction.magnitude;
                
                if (distance > 0.01f)
                {
                    // Apply attraction force proportional to distance
                    var force = direction.normalized * (attractionForce * distance);
                    
                    // Limit maximum speed
                    if (rb.velocity.magnitude > maxAttractionSpeed)
                        rb.velocity = rb.velocity.normalized * maxAttractionSpeed;
                    
                    rb.AddForce(force * Time.deltaTime);
                    
                    // Apply damping to prevent oscillation
                    rb.velocity *= dampingFactor;
                }
            }
            else
            {
                objTransform.position = targetPosition;
            }

            // Apply rotation return to target rotation relative to player
            if (rb != null)
            {
                // Compute target rotation in world space from stored relative-to-player quaternion
                var targetWorld = playerTransform.rotation * _targetRotationRelativeToPlayer;
                var currentWorld = objTransform.rotation;

                // Delta that rotates current -> target: delta = target * inv(current)
                var delta = targetWorld * Quaternion.Inverse(currentWorld);

                delta.ToAngleAxis(out float angleDeg, out Vector3 axis);
                if (axis == Vector3.zero || float.IsNaN(axis.x))
                {
                    // no meaningful rotation
                }
                else
                {
                    // Ensure shortest angle (-180..180)
                    if (angleDeg > 180f) angleDeg -= 360f;

                    var angleRad = angleDeg * Mathf.Deg2Rad;

                    // Proportional target angular velocity (PD-style)
                    var desiredAngVel = axis.normalized * (angleRad * rotationReturnSpeed);

                    // Compute corrective torque: proportional to velocity error and some damping
                    var angVelError = desiredAngVel - rb.angularVelocity;

                    // Scale torque by approximate inertia (use inertiaTensor average) to respect object's shape
                    var inertiaAvg = (rb.inertiaTensor.x + rb.inertiaTensor.y + rb.inertiaTensor.z) * 0.3333333f;
                    var torque = angVelError * inertiaAvg * torqueMultiplier;

                    // Apply torque
                    rb.AddTorque(torque);
                }
            }

            // Drop object if pickup button released
            if (!inputManager.IsPickupPressed)
                DropObject();
        }

        public bool IsHoldingObject()
        {
            return _currentHandledObject != null;
        }

        private void DropObject()
        {
            if (_currentHandledObject != null)
            {
                // Reset angular damping before dropping
                var rb = _currentHandledObject.GetGameObject().GetComponent<Rigidbody>();
                if (rb != null)
                    rb.angularDamping = 0.05f; // Default Unity value

                _currentHandledObject.OnDrop();
                _currentHandledObject = null;
                _isRotatingObject = false;
                // Re-enable interaction system
            }
        }

        private Vector3 CalculateTorqueForRotation(Rigidbody rb, Quaternion rotationDifference, float speed)
        {
            // Convert rotation difference to axis-angle representation
            rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);
            
            // Ensure angle is in range [0, 180]
            if (angle > 180f)
                angle -= 360f;
            
            // Calculate torque magnitude based on angle
            float torqueMagnitude = Mathf.Abs(angle) * speed;
            
            // Apply torque in the direction of the axis
            return axis * torqueMagnitude;
        }
    }
}