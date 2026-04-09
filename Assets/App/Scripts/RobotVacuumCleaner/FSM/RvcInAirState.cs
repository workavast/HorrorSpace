using System.Collections.Generic;

namespace App.RobotVacuumCleaner.FSM
{
    public class RvcInAirState : RvcBaseState
    {
        public RvcInAirState(RvcContext ctx, RvcParams parameters) : base(ctx, parameters)
        {
        }

        public override void Enter(Dictionary<string, object> args)
        {
        }

        public override void Exit()
        {
        }

        public override void ManualUpdate()
        {
            if (IsGrounded())
            {
                SetState<RvcIdleState>();
                return;
            }
        }

        public override void ManualFixedUpdate()
        {
        }
    }
}