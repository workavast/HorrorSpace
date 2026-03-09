using App.DraggedObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Character.Hand.FSM
{
    /// <summary>
    /// State for dragging objects that implement IDraggable interface.
    /// Dragging moves the previously picked-up point toward the current cursor ray
    /// while maintaining the initial pickup distance from the camera.
    /// </summary>
    public class HandDragState : HandState
    {
        private IDraggable _draggableObject;
        // offset from object origin to the point that was grabbed, in local space
        private Vector3 _pickupOffsetLocal;
        private float _distance;
        
        public HandDragState(HandContext ctx, IHandParams parameters)
            : base(ctx, parameters)
        {
        }

        public override void Enter(string[] args)
        {
            // Get the draggable component from the handled object
            var handleObject = _ctx.HandledObject;
            _draggableObject = handleObject?.Transform.GetComponent<IDraggable>() ??
                              handleObject?.Transform.GetComponentInParent<IDraggable>();

            if (_draggableObject == null)
            {
                Debug.LogError("No draggable component found on object");
                // Should not happen, but fallback to hold state
                SetState<HandHoldState>();
                return;
            }

            _draggableObject.OnDragStart();
            _ctx.CursorViewToggler.SetState(true);
            _ctx.DraggedObjMass = _draggableObject.Mass;

            // compute local offset to the point that was grabbed
            var objTransform = handleObject.Transform;
            _pickupOffsetLocal = objTransform.InverseTransformPoint(_ctx.DragPoint);
            
            var camera = _ctx.CameraController.GetCamera();
            _distance = Vector3.Distance(camera.transform.position, _ctx.DragPoint);
        }

        public override void Exit()
        {
            if (_draggableObject != null)
            {
                _draggableObject.OnDragEnd();
                _draggableObject = null;
            }
        }

        public override void ManualUpdate()
        {
            if (!_ctx.HasHandledObject)
            {
                SetState<HandEmptyState>();
                return;
            }

            // Drop if pickup button is released or object goes out of range
            if (!_ctx.InputManager.IsPickupPressed || !ObjectInHoldRange())
            {
                DropObject();
                SetState<HandEmptyState>();
                return;
            }

            DragObject();
        }

        public override void ManualFixedUpdate()
        {
        }

        /// <summary>
        /// Drags the object in world space based on current cursor ray.
        /// The object follows the previously grabbed point while preserving
        /// the initial camera distance.
        /// </summary>
        private void DragObject()
        {
            var handleObject = _ctx.HandledObject;
            var objTransform = handleObject.Transform;
            var rb = handleObject.Rb;
            var camera = _ctx.CameraController.GetCamera();

            // compute target point along cursor ray at fixed pickup distance
            var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            var targetPointOnRay = ray.origin + ray.direction * _distance;

            // offset of grab point in world space
            var offsetWorld = objTransform.TransformVector(_pickupOffsetLocal);
            var targetPosition = targetPointOnRay - offsetWorld;

            // apply attraction force to move object to target position
            var direction = targetPosition - objTransform.position;
            var distance = direction.magnitude;
            if (distance > 0.01f)
            {
                var force = direction.normalized * (_parameters.DragAttractionForce);

                // calculate world point that was grabbed
                var pickupWorld = objTransform.position + offsetWorld;
                rb.AddForceAtPosition(force * Time.deltaTime, pickupWorld);
            }
        }
    }
}
