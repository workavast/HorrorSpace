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
            _ctx.Velocity.y = -0.1f;

            // If jump pressed, perform jump and switch to air state
            if (_input.JumpPressedThisFrame && _ctx.IsGrounded)
            {
                _ctx.Motor.ForceUnground();
                _ctx.Velocity.y = _params.JumpForce;
                SetState<JumpAirState>();
                return;
            }

            if (!_ctx.IsGroundContact)
            {
                SetState<JumpAirState>();
                return;
            }
        }
    }
}
