using UnityEngine;

namespace App.Character.FSM.Jump
{
    public class JumpAirState : JumpStateBase
    {
        public JumpAirState(PlayerParams p, IReadOnlyInputProvider input, PlayerContext ctx) : base(p, input, ctx)
        {
        }

        public override void Enter(string[] args)
        {
            _ctx.IsInAirJump = true;
        }

        public override void Exit()
        {
            _ctx.IsInAirJump = false;
        }

        public override void ManualFixedUpdate()
        {
        }

        public override void ManualUpdate()
        {
            // Apply gravity
            _ctx.Velocity.y -= _params.Gravity * Time.deltaTime;
            // Drag
            _ctx.Velocity *= (1f / (1f + (_params.AirDrag * Time.deltaTime)));
            
            // If landed, switch to grounded
            if (_ctx.IsGrounded)
            {
                SetState<JumpGroundedState>();
                return;
            }

            if (CheckHeadCollision())
                _ctx.Velocity.y = -0.1f;
        }
        
        private bool CheckHeadCollision()
        {
            if (_ctx.Velocity.y <= 0)
                return false;

            var headPosition = _ctx.Transform.position + Vector3.up * _ctx.Motor.Capsule.height;
            var checkOffset = Vector3.up * _params.HeadCollisionDistance;
            var origin = headPosition + checkOffset - Vector3.up * _params.CollisionCheckRadius;
            
            return Physics.CheckSphere(origin, _params.CollisionCheckRadius, _params.ObstacleLayer);
        }
    }
}
