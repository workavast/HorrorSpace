using UnityEngine;

namespace App.Character.FSM.Movement
{
    public class MovementCrouchState : MovementStateBase
    {
        public MovementCrouchState(PlayerParams p, PlayerInputManager input, PlayerContext ctx) : base(p, input, ctx)
        {
        }

        public override void Enter(string[] args)
        {
            _ctx.IsCrouching = true;
            _ctx.CameraController.SetTargetHeight(_params.CrouchCameraHeight);
        }

        public override void Exit()
        {
            _ctx.IsCrouching = false;
        }

        public override void ManualFixedUpdate()
        {
        }

        public override void ManualUpdate()
        {
            var moveInput = _input.MoveInput;
            
            HandleMovementInput(moveInput, _params.CrouchSpeed);
            UpdateHeight(_params.CrouchHeight);

            if (!_input.IsCrouchPressed && CanStand())
            {
                SetState<MovementStandState>();
                return;
            }
        }

        private bool CanStand()
        {
            var headPosition = _ctx.Transform.position + Vector3.up * _params.CrouchHeight + Vector3.up * _params.StandCheckOffset;
            var origin = headPosition + Vector3.down * _params.CollisionCheckRadius;
            
            return !Physics.SphereCast(
                origin,
                _params.CollisionCheckRadius,
                Vector3.up,
                out var hit,
                _params.StandCheckDistance,
                _params.ObstacleLayer
            );
        }
    }
}
