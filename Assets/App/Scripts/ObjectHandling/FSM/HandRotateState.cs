using UnityEngine;

namespace App.ObjectHandling
{
    public class HandRotateState : HandState
    {
        public HandRotateState(HandContext context, IHandParams parameters)
            : base(context, parameters)
        {
        }

        public override void Enter(string[] args)
        {
            _context.CameraController.SetCameraRotationEnabled(false);
            _context.CursorViewToggler.SetState(true);
        }

        public override void Exit()
        {
            _context.CameraController.SetCameraRotationEnabled(true);
        }

        public override void ManualUpdate()
        {
            if (!_context.HasHandledObject)
            {
                SetState<HandEmptyState>();
                return;
            }
            
            if (!_context.InputManager.IsPickupPressed || !ObjectInHoldRange())
            {
                DropObject();
                SetState<HandEmptyState>();
                return;
            }

            if (!_context.InputManager.IsRotatePressed)
            {
                SetState<HandHoldState>();
                return;
            }

            HoldObject();
            RotateObject();
        }

        public override void ManualFixedUpdate()
        {
        }

        private void RotateObject()
        {
            var objTransform = _context.HandledObject.Transform;
            var rb = _context.HandledObject.Rb;

            var mouseDelta = _context.InputManager.LookInput;
            
            // Apply manual rotation relative to player orientation
            var localTorque = new Vector3(-mouseDelta.y, mouseDelta.x, 0) * _parameters.ManualRotationSpeed;
            var worldTorque = _context.PlayerTransform.TransformVector(localTorque);
            rb.AddTorque(worldTorque);
                
            // Update target rotation based on new orientation
            var targetRotationRelativeToPlayer = Quaternion.Inverse(_context.PlayerTransform.rotation) * objTransform.rotation;
            _context.TargetRotationRelativeToPlayer = targetRotationRelativeToPlayer;
        }

    }
}