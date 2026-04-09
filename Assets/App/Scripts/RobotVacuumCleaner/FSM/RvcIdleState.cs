using System.Collections.Generic;
using UnityEngine;

namespace App.RobotVacuumCleaner.FSM
{
    public class RvcIdleState : RvcBaseState
    {
        public RvcIdleState(RvcContext ctx, RvcParams parameters) : base(ctx, parameters)
        {
        }

        public override void Enter(Dictionary<string, object> args)
        {
            if (!IsGrounded())
            {
                SetState<RvcInAirState>();
                return;
            }
            
            if (IsOccupied())
            {
                SetState<RvcOccupiedState>();
                return;
            }
            
            var angle = Random.Range(0f, 360f);
            _ctx.TargetDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            SetState<RvcRotateState>();
        }

        public override void Exit() { }
        public override void ManualUpdate() { }
        public override void ManualFixedUpdate() { }
    }
}
