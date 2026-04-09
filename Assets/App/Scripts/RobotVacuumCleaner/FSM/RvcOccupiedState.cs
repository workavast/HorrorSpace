using System.Collections.Generic;

namespace App.RobotVacuumCleaner.FSM
{
    public class RvcOccupiedState : RvcBaseState
    {
        public RvcOccupiedState(RvcContext ctx, RvcParams parameters) : base(ctx, parameters)
        {
        }

        public override void Enter(Dictionary<string, object> args) { }

        public override void Exit() { }

        public override void ManualUpdate()
        {
            if (!IsGrounded())
            {
                SetState<RvcInAirState>();
                return;
            }
            
            if (!IsOccupied())
            {
                SetState<RvcMoveState>();
                return;
            }
        }

        public override void ManualFixedUpdate() { }
    }
}
