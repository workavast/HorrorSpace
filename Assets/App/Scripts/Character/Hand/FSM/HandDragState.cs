using System.Collections.Generic;
using App.HandlingObjects.DraggedObjects;
using DCFApixels;
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

            var offsetWorld = objTransform.TransformVector(_pickupOffsetLocal);
            var currentGrabPoint = objTransform.position + offsetWorld;
            var targetPointOnRay = camera.transform.position + camera.transform.forward * _ctx.CurrentDistance;

            var rawForce = _params.DragForcePerKg * _draggableObject.Mass;
            var forceLimit = _params.DragForcePerKg * _params.DragMassLimit;
            var clampedRawForce = Mathf.Clamp(rawForce, 0, forceLimit);
            
            // Critical damping: c = 2 * sqrt(k * m) p.s. 2 removed to reduce damping
            var criticalDamping = Mathf.Sqrt(clampedRawForce * rb.mass);
            
            var pointVelocity = rb.GetPointVelocity(currentGrabPoint);

            // PD controller
            var error = targetPointOnRay - currentGrabPoint;
            var dampingForce = pointVelocity * criticalDamping;

            var force = error.normalized * clampedRawForce - dampingForce;
            if (error.magnitude < 1)
                force = error * clampedRawForce - dampingForce;

            rb.AddForceAtPosition(force, currentGrabPoint);
            
            if (_params.DragDebug)
            {
                DebugX.Draw(Color.red).Sphere(currentGrabPoint, 0.1f);
                DebugX.Draw(Color.blue).Sphere(targetPointOnRay, 0.1f);
                DebugX.Draw(Color.red).Line(currentGrabPoint, targetPointOnRay);
                DebugX.Draw().Text(targetPointOnRay, 
                    $"{rawForce}: raw force" +
                    $"\n{forceLimit}: limit force" +
                    $"\n{clampedRawForce}: clamped raw force" +
                    $"\n{dampingForce}: damping force" +
                    $"\n{force}: result force");
            }
        }
    }
}
