using System.Collections.Generic;
using UnityEngine;

namespace App.Character.Hand.FSM
{
    public class HandRotateState : HandState
    {
        public HandRotateState(HandContext ctx, IHandParams parameters)
            : base(ctx, parameters)
        {
        }

        public override void Enter(Dictionary<string, object> args)
        {
            _ctx.CameraController.SetCameraRotationEnabled(false);
            _ctx.CursorViewToggler.SetState(true);
        }

        public override void Exit()
        {
            _ctx.CameraController.SetCameraRotationEnabled(true);
        }

        public override void ManualUpdate()
        {
            if (!_ctx.HasHandledObject)
            {
                SetState<HandEmptyState>();
                return;
            }
            
            if (!_ctx.InputManager.IsPickupPressed || !ObjectInHoldRange())
            {
                DropObject();
                SetState<HandEmptyState>();
                return;
            }

            if (!_ctx.InputManager.IsRotatePressed)
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
            var objTransform = _ctx.HandledObject.Transform;
            var rb = _ctx.HandledObject.Rb;

            var mouseDelta = _ctx.InputManager.LookInput;
            
            // Apply manual rotation relative to player orientation
            var localTorque = new Vector3(-mouseDelta.y, mouseDelta.x, 0) * _parameters.ManualRotationSpeed;
            var worldTorque = _ctx.Transform.TransformVector(localTorque);
            rb.AddTorque(worldTorque);
                
            // Update target rotation based on new orientation
            var targetRotationRelativeToPlayer = Quaternion.Inverse(_ctx.Transform.rotation) * objTransform.rotation;
            _ctx.TargetRotationRelativeToPlayer = targetRotationRelativeToPlayer;
        }

    }
}