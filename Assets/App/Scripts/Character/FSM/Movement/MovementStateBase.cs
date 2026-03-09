using Avastrad.FSM;
using UnityEngine;

namespace App.Character.FSM.Movement
{
    public abstract class MovementStateBase : StateBase
    {
        protected readonly PlayerParams _params;
        protected readonly PlayerInputManager _input;
        protected readonly PlayerContext _ctx;

        protected MovementStateBase(PlayerParams p, PlayerInputManager input, PlayerContext ctx)
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
                _ctx.VelocityInput *= (1f / (1f + (_params.AirDrag * Time.deltaTime)));
                return;
            }

            _ctx.VelocityInput = moveInput;
            _ctx.TargetSpeed = targetSpeed;
        }
        
        // Small helper to smoothly change controller height and center for crouch
        protected void UpdateHeight(float targetHeight)
        {
            var currentHeight = _ctx.Motor.Capsule.height;
            var height = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * _params.CrouchHeightSpeed);
            
            var currentCenter = _ctx.Motor.Capsule.center;
            var targetCenterY = targetHeight * 0.5f;
            currentCenter.y = Mathf.Lerp(currentCenter.y, targetCenterY, Time.deltaTime * _params.CrouchHeightSpeed);

            var capsule = _ctx.Motor.Capsule;
            _ctx.Motor.SetCapsuleDimensions(capsule.radius, height, currentCenter.y);
        }
    }
}
