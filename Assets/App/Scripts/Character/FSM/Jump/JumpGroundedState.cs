using UnityEngine;

namespace App.Character.FSM.Jump
{
    public class JumpGroundedState : JumpStateBase
    {
        public JumpGroundedState(PlayerParams p, IReadOnlyInputProvider input, PlayerContext ctx) : base(p, input, ctx)
        {
        }

        public override void Enter(string[] args)
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void ManualFixedUpdate()
        {
        }

        public override void ManualUpdate()
        {
            _ctx.Velocity.y = -9.8f;

            // If jump pressed, perform jump and switch to air state
            if (_input.JumpPressedThisFrame && _ctx.IsGrounded)
            {
                _ctx.JumpHorizontalVelocity = new Vector3(_ctx.Velocity.x, 0f, _ctx.Velocity.z);
                _ctx.Velocity.y = _params.JumpForce;
                SetState<JumpAirState>();
            }
        }
    }
}
