using System.Collections.Generic;
using App.HandlingObjects;
using App.HandlingObjects.DraggedObjects;
using App.HandlingObjects.HoldableObjects;
using Avastrad.Libs.CastLib;
using Avastrad.Libs.CheckOnNullLib;
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

                if (TryDragObj(out var grabPoint))
                {
                    SetState<HandDragState>(new() { { HandDragState.DragPointKey, grabPoint } });
                    return;
                }
            }
        }

        public override void ManualFixedUpdate()
        {
        }

        private bool TryHoldObj()
        {
            if (!TryHandleObj(out var handleObj, out var hitPoint))
                return false;

            if (!handleObj.TryCast(out IHoldableObj holdableObj))
                return false;
            
            GrabObject(holdableObj, hitPoint);
            return true;
        }

        private bool TryDragObj(out Vector3 grabPoint)
        {
            if (!TryHandleObj(out var handleObj, out grabPoint))
                return false;

            if (!handleObj.TryCast(out IDraggable draggable))
                return false;
            
            GrabObject(draggable, grabPoint);
            return true;
        }
        
        private bool TryHandleObj(out IHandleObject handleObj, out Vector3 grabPoint)
        {
            var camera = _ctx.CameraController.GetCamera();
            var ray = new Ray(camera.transform.position, camera.transform.forward);

            if (Physics.Raycast(ray, out var hit, _params.PickupRange, _params.HandleLayerMask))
            {
                handleObj = hit.collider.GetComponent<IHandleObject>() ??
                            hit.collider.GetComponentInParent<IHandleObject>();

                if (handleObj != null && handleObj.CanBeHandled)
                {
                    grabPoint = hit.point;
                    return true;
                }
            }

            handleObj = null;
            grabPoint = default;
            return false;
        }
        
        private void GrabObject(IHoldableObj holdableObj, Vector3 grabPoint)
        {
            _ctx.HandledObj = holdableObj;
            holdableObj.OnPickup();
            
            // Calculate initial distance
            var camera = _ctx.CameraController.GetCamera();
            var currentDistance = Vector3.Distance(camera.transform.position, grabPoint);
            currentDistance = Mathf.Clamp(currentDistance, _params.MinDistance, _params.MaxDistance);
            _ctx.CurrentDistance = currentDistance;

            // Store target rotation relative to player (not camera)
            var objTransform = holdableObj.Transform;
            var targetRotationRelativeToPlayer = Quaternion.Inverse(_ctx.Transform.rotation) * objTransform.rotation;
            _ctx.TargetRotationRelativeToPlayer = targetRotationRelativeToPlayer;
            
            // Increase angular damping for picked up object
            var rb = holdableObj.Rb;
            _ctx.SetDefaultObjectDampings(rb.linearDamping, rb.angularDamping);
            rb.angularDamping = _params.AngularDampingPickup;
            rb.linearDamping = _params.LinerDampingPickup;
        }

        private void GrabObject(IDraggable draggable, Vector3 grabPoint)
        {
            _ctx.HandledObj = draggable;
            
            // Calculate initial distance
            var camera = _ctx.CameraController.GetCamera();
            var currentDistance = Vector3.Distance(camera.transform.position, grabPoint);
            currentDistance = Mathf.Clamp(currentDistance, _params.MinDistance, _params.MaxDistance);
            _ctx.CurrentDistance = currentDistance;

            // Store target rotation relative to player (not camera)
            _ctx.TargetRotationRelativeToPlayer = Quaternion.identity;
            
            // Increase angular damping for picked up object
            var rb = draggable.Rb;
            _ctx.SetDefaultObjectDampings(rb.linearDamping, rb.angularDamping);
            rb.angularDamping = _params.AngularDampingDrag;
            rb.linearDamping = _params.LinerDampingDrag;
        }
    }
}