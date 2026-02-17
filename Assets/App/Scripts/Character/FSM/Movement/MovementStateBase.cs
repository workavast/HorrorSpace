using Avastrad.FSM;
using UnityEngine;

namespace App.Character.FSM.Movement
{
    public abstract class MovementStateBase : StateBase
    {
        protected readonly PlayerParams _params;
        protected readonly IReadOnlyInputProvider _input;
        protected readonly PlayerContext _ctx;

        protected MovementStateBase(PlayerParams p, IReadOnlyInputProvider input, PlayerContext ctx)
        {
            _params = p;
            _input = input;
            _ctx = ctx;
        }

        protected void HandleMovementInput(Vector2 moveInput, float targetSpeed)
        {
            // If we are in an in-air jump phase, keep horizontal velocity locked
            if (!_ctx.IsGrounded && _ctx.IsInAirJump)
            {
                _ctx.Velocity.x = _ctx.JumpHorizontalVelocity.x;
                _ctx.Velocity.z = _ctx.JumpHorizontalVelocity.z;
                return;
            }

            targetSpeed *= moveInput.magnitude;

            var currentHorizontalSpeed = new Vector3(_ctx.Velocity.x, 0, _ctx.Velocity.z).magnitude;
            if (moveInput.magnitude > 0)
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * _params.Acceleration);
            else
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, 0, Time.deltaTime * _params.Deceleration);

            var moveDirection = _ctx.Transform.right * moveInput.x + _ctx.Transform.forward * moveInput.y;
            _ctx.Velocity.x = moveDirection.x * currentHorizontalSpeed;
            _ctx.Velocity.z = moveDirection.z * currentHorizontalSpeed;
        }
        
        // Small helper to smoothly change controller height and center for crouch
        protected void UpdateHeight(float targetHeight)
        {
            var currentHeight = _ctx.CharacterController.height;
            _ctx.CharacterController.height = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * _params.CrouchHeightSpeed);
            
            var currentCenter = _ctx.CharacterController.center;
            var targetCenterY = targetHeight * 0.5f;
            currentCenter.y = Mathf.Lerp(currentCenter.y, targetCenterY, Time.deltaTime * _params.CrouchHeightSpeed);
            _ctx.CharacterController.center = currentCenter;
        }
    }
}
