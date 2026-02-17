namespace App.Character.FSM.Movement
{
    public class MovementStandState : MovementStateBase
    {
        public MovementStandState(PlayerParams p, IReadOnlyInputProvider input, PlayerContext ctx) : base(p, input, ctx)
        {
        }

        public override void Enter(string[] args)
        {
            _ctx.CameraController.SetTargetHeight(_params.NormalCameraHeight);
        }

        public override void Exit()
        {
        }

        public override void ManualFixedUpdate()
        {
        }

        public override void ManualUpdate()
        {
            var moveInput = _input.MoveInput;
            var targetSpeed = _input.IsSprintPressed ? _params.SprintSpeed : _params.MoveSpeed;
            
            HandleMovementInput(moveInput, targetSpeed);
            UpdateHeight(_ctx.DefaultHeight);

            if (_input.IsCrouchPressed)
            {
                SetState<MovementCrouchState>();
                return;
            }
        }
    }
}
