using System.Collections.Generic;
using App.HandlingObjects.HoldableObjects;
using Avastrad.Libs.CheckOnNullLib;
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
            
            if (!_ctx.InputManager.IsPickupPressed || !ObjInHandleRange(_ctx.HandledObj.Transform))
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
        }

        public override void ManualFixedUpdate()
        {
            HoldObject(_ctx.HandledObj as IHoldableObj);
            RotateObject();
        }

        private void RotateObject()
        {
            if (_ctx.HandledObj.IsAnyNull())
                return;
            
            var objTransform = _ctx.HandledObj.Transform;
            var rb = _ctx.HandledObj.Rb;

            var mouseDelta = _ctx.InputManager.LookInput;
            
            // Apply manual rotation relative to player orientation
            var localTorque = new Vector3(-mouseDelta.y, mouseDelta.x, 0) * _params.ManualRotationSpeed;
            var worldTorque = _ctx.Transform.TransformVector(localTorque);
            rb.AddTorque(worldTorque);
                
            // Update target rotation based on new orientation
            var targetRotationRelativeToPlayer = Quaternion.Inverse(_ctx.Transform.rotation) * objTransform.rotation;
            _ctx.TargetRotationRelativeToPlayer = targetRotationRelativeToPlayer;
        }

    }
}