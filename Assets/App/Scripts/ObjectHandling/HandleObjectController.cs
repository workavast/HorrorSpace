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

        private IHandleObject _currentHandledObject;
        private float _currentDistance;
        private Vector3 _localOffset;
        private bool _isRotatingObject;

        // Input actions - assuming we add them to PlayerInputManager
        // For now, we'll use mouse buttons directly, but ideally integrate with Input System

        private void Update()
        {
            if (_currentHandledObject != null)
            {
                HandleHeldObject();
            }
            else
            {
                CheckForPickup();
            }
        }

        private void CheckForPickup()
        {
            // Check if pickup button is pressed
            if (inputManager.IsPickupPressed)
            {
                TryPickupObject();
            }
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
                {
                    PickupObject(handleObject, hit.point);
                }
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

            // Don't instantly position object - let physics forces handle attraction
            // Object will naturally move to target position while keeping its orientation

            handleObject.OnPickup();

            // Disable interaction system - we'll need to notify PlayerCharacterController
            // For now, assume we have a way to disable it
        }

        private void HandleHeldObject()
        {
            var camera = cameraController.GetCamera();
            var objTransform = _currentHandledObject.GetGameObject().transform;

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

                // Rotate object based on mouse movement
                var mouseDelta = inputManager.LookInput;
                var objTransform2 = _currentHandledObject.GetGameObject().transform;
                var rb = _currentHandledObject.GetGameObject().GetComponent<Rigidbody>();
                
                if (rb != null)
                {
                    // Use physics-based rotation for smoother movement
                    var rotation = objTransform2.rotation;
                    rotation *= Quaternion.Euler(-mouseDelta.y * rotationSensitivity, mouseDelta.x * rotationSensitivity, 0);
                    rb.MoveRotation(rotation);
                }
                else
                {
                    objTransform2.Rotate(Vector3.up, mouseDelta.x * rotationSensitivity, Space.World);
                    objTransform2.Rotate(Vector3.right, -mouseDelta.y * rotationSensitivity, Space.Self);
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
            var rb2 = _currentHandledObject.GetGameObject().GetComponent<Rigidbody>();
            if (rb2 != null)
            {
                var direction = targetPosition - objTransform.position;
                var distance = direction.magnitude;
                
                if (distance > 0.01f)
                {
                    // Apply attraction force proportional to distance
                    var force = direction.normalized * attractionForce * distance;
                    
                    // Limit maximum speed
                    if (rb2.velocity.magnitude > maxAttractionSpeed)
                    {
                        rb2.velocity = rb2.velocity.normalized * maxAttractionSpeed;
                    }
                    
                    rb2.AddForce(force * Time.deltaTime);
                    
                    // Apply damping to prevent oscillation
                    rb2.velocity *= dampingFactor;
                }
            }
            else
            {
                objTransform.position = targetPosition;
            }

            // Drop object if pickup button released
            if (!inputManager.IsPickupPressed)
            {
                DropObject();
            }
        }

        public bool IsHoldingObject()
        {
            return _currentHandledObject != null;
        }

        private void DropObject()
        {
            if (_currentHandledObject != null)
            {
                _currentHandledObject.OnDrop();
                _currentHandledObject = null;
                _isRotatingObject = false;
                // Re-enable interaction system
            }
        }
    }
}