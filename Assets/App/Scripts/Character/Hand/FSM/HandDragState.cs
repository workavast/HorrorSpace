using System.Collections.Generic;
using App.HandlingObjects.DraggedObjects;
using UnityEngine;

namespace App.Character.Hand.FSM
{
    public class HandDragState : HandState
    {
        public const string DragPointKey = "dragPoint";
        
        private IDraggable _draggableObject;
        private Vector3 _pickupOffsetLocal; // offset from object origin to the point that was grabbed, in local space
        
        public HandDragState(HandContext ctx, IHandParams parameters)
            : base(ctx, parameters)
        {
        }

        public override void Enter(Dictionary<string, object> args)
        {
            if (!args.ContainsKey(DragPointKey))
            {
                Debug.LogError("No drag point found in args");
                return;
            }
            
            _draggableObject = _ctx.HandledObj?.Transform.GetComponent<IDraggable>() ??
                               _ctx.HandledObj?.Transform.GetComponentInParent<IDraggable>();

            if (_draggableObject == null)
            {
                Debug.LogError("No draggable component found on object");
                SetState<HandHoldState>();
                return;
            }

            _ctx.CursorViewToggler.SetState(true);
            _ctx.DraggedObjMass = _draggableObject.Mass;

            var dragPoint = (Vector3)args[DragPointKey];
            
            // compute local offset to the point that was grabbed
            var objTransform = _draggableObject.Transform;
            _pickupOffsetLocal = objTransform.InverseTransformPoint(dragPoint);
        }

        public override void Exit()
        {
            _draggableObject = null;
        }

        public override void ManualUpdate()
        {
            if (!_ctx.HasHandledObject)
            {
                SetState<HandEmptyState>();
                return;
            }

            if (!_ctx.InputManager.IsPickupPressed || !ObjInHandleRange(_draggableObject.Transform))
            {
                DropObject();
                SetState<HandEmptyState>();
                return;
            }
        }

        public override void ManualFixedUpdate()
        {
            DragObject();
        }

        /// <summary>
        /// Drags the object in world space based on current cursor ray.
        /// The object follows the previously grabbed point while preserving
        /// the initial camera distance.
        /// </summary>
        private void DragObject()
        {
            if (_draggableObject == null)
                return;
            
            var objTransform = _draggableObject.Transform;
            var rb = _draggableObject.Rb;
            var camera = _ctx.CameraController.GetCamera();

            // compute target point along camera forward at fixed pickup distance
            var targetPointOnRay = camera.transform.position + camera.transform.forward * _ctx.CurrentDistance;

            // offset of grab point in world space
            var offsetWorld = objTransform.TransformVector(_pickupOffsetLocal);
            var currentGrabPoint = objTransform.position + offsetWorld;

            // apply attraction force to move object to target position
            var direction = targetPointOnRay - currentGrabPoint;
            var distance = direction.magnitude;

            //TODO import DebugX tool
            if (_params.DragDebug)
            {
                Physics.CheckSphere(currentGrabPoint, 0.1f);
                Physics.CheckSphere(targetPointOnRay, 0.1f);
                Physics.Linecast(currentGrabPoint, targetPointOnRay);
            }

            Vector3 force;
            if (distance > _params.DragDampingDistance)
                 force = direction.normalized * (_params.DragAttractionForce);
            else
                 force = direction.normalized * ((_params.DragAttractionForce) * (distance / _params.DragDampingDistance));

            // calculate world point that was grabbed
            rb.AddForceAtPosition(force * Time.fixedDeltaTime, currentGrabPoint);
        }
    }
}
