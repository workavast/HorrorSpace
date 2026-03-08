using App.DraggedObjects;
using App.ObjectHandling.FSM;
using UnityEngine;

namespace App.ObjectHandling
{
    public class HandEmptyState : HandState
    {
        public HandEmptyState(HandContext context, IHandParams parameters) 
            : base(context, parameters)
        {
        }

        public override void Enter(string[] args)
        {
            _context.CursorViewToggler.SetState(false);
        }

        public override void Exit()
        {
        }

        public override void ManualUpdate()
        {
            if (_context.InputManager.IsPickupPressed)
            {
                TryPickupObject();
                if (_context.HasHandledObject)
                {
                    // Check if the object can be dragged
                    if (IsDraggable(_context.HandledObject))
                        SetState<HandDragState>();
                    else
                        SetState<HandHoldState>();
                }
            }
        }

        public override void ManualFixedUpdate()
        {
        }

        private void TryPickupObject()
        {
            var camera = _context.CameraController.GetCamera();
            var ray = new Ray(camera.transform.position, camera.transform.forward);

            if (Physics.Raycast(ray, out var hit, _parameters.PickupRange, _parameters.HandleLayerMask))
            {
                var handleObject = hit.collider.GetComponent<IHandleObject>() ??
                                   hit.collider.GetComponentInParent<IHandleObject>();

                if (handleObject != null && handleObject.CanBeHandled)
                    PickupObject(handleObject, hit.point);
            }
        }

        private void PickupObject(IHandleObject handleObject, Vector3 pickupPoint)
        {
            _context.HandledObject = handleObject;
            handleObject.OnPickup();
            
            var objTransform = handleObject.Transform;

            // store pickup point and local offset for dragging
            _context.PickupPoint = pickupPoint;
            _context.PickupOffsetLocal = objTransform.InverseTransformPoint(pickupPoint);

            // Calculate initial distance and offset
            var camera = _context.CameraController.GetCamera();
            var currentDistance = Vector3.Distance(camera.transform.position, pickupPoint);
            currentDistance = Mathf.Clamp(currentDistance, _parameters.MinDistance, _parameters.MaxDistance);
            _context.CurrentDistance = currentDistance;

            // Store target rotation relative to player (not camera)
            var targetRotationRelativeToPlayer = Quaternion.Inverse(_context.PlayerTransform.rotation) * objTransform.rotation;
            _context.TargetRotationRelativeToPlayer = targetRotationRelativeToPlayer;
            
            // Increase angular damping for picked up object
            var rb = handleObject.Rb;
            _context.SetDefaultObjectDampings(rb.linearDamping, rb.angularDamping);
            rb.angularDamping = _parameters.AngularDampingPickup;
            rb.linearDamping = _parameters.LinerDampingPickup;
        }

        private static bool IsDraggable(IHandleObject handleObject)
        {
            var draggable = handleObject.Transform.GetComponent<IDraggable>() ??
                           handleObject.Transform.GetComponentInParent<IDraggable>();
            return draggable != null && draggable.CanBeDragged;
        }
    }
}