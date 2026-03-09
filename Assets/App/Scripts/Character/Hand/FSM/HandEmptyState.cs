using App.DraggedObjects;
using App.ObjectHandling;
using UnityEngine;

namespace App.Character.Hand.FSM
{
    public class HandEmptyState : HandState
    {
        public HandEmptyState(HandContext ctx, IHandParams parameters) 
            : base(ctx, parameters)
        {
        }

        public override void Enter(string[] args)
        {
            _ctx.CursorViewToggler.SetState(false);
            _ctx.DraggedObjMass = 0;
        }

        public override void Exit()
        {
        }

        public override void ManualUpdate()
        {
            if (_ctx.InputManager.IsPickupPressed)
            {
                TryPickupObject();
                if (_ctx.HasHandledObject)
                {
                    // Check if the object can be dragged
                    if (IsDraggable(_ctx.HandledObject))
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
            var camera = _ctx.CameraController.GetCamera();
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
            _ctx.HandledObject = handleObject;
            handleObject.OnPickup();
            
            var objTransform = handleObject.Transform;

            // store pickup point and local offset for dragging
            _ctx.DragPoint = pickupPoint;

            // Calculate initial distance and offset
            var camera = _ctx.CameraController.GetCamera();
            var currentDistance = Vector3.Distance(camera.transform.position, pickupPoint);
            currentDistance = Mathf.Clamp(currentDistance, _parameters.MinDistance, _parameters.MaxDistance);
            _ctx.CurrentDistance = currentDistance;

            // Store target rotation relative to player (not camera)
            var targetRotationRelativeToPlayer = Quaternion.Inverse(_ctx.Transform.rotation) * objTransform.rotation;
            _ctx.TargetRotationRelativeToPlayer = targetRotationRelativeToPlayer;
            
            // Increase angular damping for picked up object
            var rb = handleObject.Rb;
            _ctx.SetDefaultObjectDampings(rb.linearDamping, rb.angularDamping);
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