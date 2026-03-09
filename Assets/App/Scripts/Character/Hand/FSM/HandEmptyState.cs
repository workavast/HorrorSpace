using System.Collections.Generic;
using App.HandlingObjects;
using App.HandlingObjects.DraggedObjects;
using App.HandlingObjects.HoldableObjects;
using UnityEngine;

namespace App.Character.Hand.FSM
{
    public class HandEmptyState : HandState
    {
        public HandEmptyState(HandContext ctx, IHandParams parameters) 
            : base(ctx, parameters)
        {
        }

        public override void Enter(Dictionary<string, object> args)
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
                if (TryHoldObj())
                {
                    SetState<HandHoldState>();
                    return;
                }

                if (TryDragObj(out var pickupPoint))
                {
                    SetState<HandDragState>(new() { { HandDragState.DragPointKey, pickupPoint } });
                    return;
                }
            }
        }

        public override void ManualFixedUpdate()
        {
        }


        

        private bool TryHoldObj()
        {
            var camera = _ctx.CameraController.GetCamera();
            var ray = new Ray(camera.transform.position, camera.transform.forward);

            if (Physics.Raycast(ray, out var hit, _parameters.PickupRange, _parameters.HandleLayerMask))
            {
                var handleObject = hit.collider.GetComponent<IHoldableObj>() ??
                                   hit.collider.GetComponentInParent<IHoldableObj>();

                if (handleObject != null && handleObject.CanBeHandled)
                {
                    PickupObject(handleObject, hit.point);
                    return true;
                }
            }

            return false;
        }

        private bool TryDragObj(out Vector3 pickupPoint)
        {
            var camera = _ctx.CameraController.GetCamera();
            var ray = new Ray(camera.transform.position, camera.transform.forward);

            if (Physics.Raycast(ray, out var hit, _parameters.PickupRange, _parameters.HandleLayerMask))
            {
                var handleObject = hit.collider.GetComponent<IDraggable>() ??
                                   hit.collider.GetComponentInParent<IDraggable>();

                if (handleObject != null && handleObject.CanBeHandled)
                {
                    pickupPoint = hit.point;
                    PickupObject(handleObject, pickupPoint);
                    return true;
                }
            }

            pickupPoint = default;
            return false;
        }
        
        private IHandleObject TryHandleObj(out Vector3 hitPopint)
        {
            var camera = _ctx.CameraController.GetCamera();
            var ray = new Ray(camera.transform.position, camera.transform.forward);

            if (Physics.Raycast(ray, out var hit, _parameters.PickupRange, _parameters.HandleLayerMask))
            {
                var handleObject = hit.collider.GetComponent<IHoldableObj>() ??
                                   hit.collider.GetComponentInParent<IHoldableObj>();

                if (handleObject != null && handleObject.CanBeHandled)
                {
                    hitPopint = hit.point;
                    return handleObject;
                }
            }

            hitPopint = Vector3.zero;
            return null;
        }
        
        private void PickupObject(IHoldableObj holdableObj, Vector3 pickupPoint)
        {
            _ctx.HandledObj = holdableObj;
            holdableObj.OnPickup();
            
            var objTransform = holdableObj.Transform;

            // Calculate initial distance and offset
            var camera = _ctx.CameraController.GetCamera();
            var currentDistance = Vector3.Distance(camera.transform.position, pickupPoint);
            currentDistance = Mathf.Clamp(currentDistance, _parameters.MinDistance, _parameters.MaxDistance);
            _ctx.CurrentDistance = currentDistance;

            // Store target rotation relative to player (not camera)
            var targetRotationRelativeToPlayer = Quaternion.Inverse(_ctx.Transform.rotation) * objTransform.rotation;
            _ctx.TargetRotationRelativeToPlayer = targetRotationRelativeToPlayer;
            
            // Increase angular damping for picked up object
            var rb = holdableObj.Rb;
            _ctx.SetDefaultObjectDampings(rb.linearDamping, rb.angularDamping);
            rb.angularDamping = _parameters.AngularDampingPickup;
            rb.linearDamping = _parameters.LinerDampingPickup;
        }

        private void PickupObject(IDraggable draggable, Vector3 pickupPoint)
        {
            _ctx.HandledObj = draggable;
            
            // Calculate initial distance and offset
            var camera = _ctx.CameraController.GetCamera();
            var currentDistance = Vector3.Distance(camera.transform.position, pickupPoint);
            currentDistance = Mathf.Clamp(currentDistance, _parameters.MinDistance, _parameters.MaxDistance);
            _ctx.CurrentDistance = currentDistance;

            // Store target rotation relative to player (not camera)
            _ctx.TargetRotationRelativeToPlayer = Quaternion.identity;
            
            // Increase angular damping for picked up object
            var rb = draggable.Rb;
            _ctx.SetDefaultObjectDampings(rb.linearDamping, rb.angularDamping);
            rb.angularDamping = _parameters.AngularDampingDrag;
            rb.linearDamping = _parameters.LinerDampingDrag;
        }
    }
}