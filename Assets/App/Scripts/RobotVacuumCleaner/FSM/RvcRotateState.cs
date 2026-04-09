using System.Collections.Generic;
using UnityEngine;

namespace App.RobotVacuumCleaner.FSM
{
    public class RvcRotateState : RvcBaseState
    {
        public RvcRotateState(RvcContext ctx, RvcParams parameters) : base(ctx, parameters)
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
            
            if (IsOccupied())
            {
                SetState<RvcOccupiedState>();
                return;
            }

            var targetRotation = Quaternion.LookRotation(_ctx.TargetDirection, Vector3.up);
            var nextRotation = Quaternion.RotateTowards(Transform.rotation, targetRotation, _params.RotationSpeed * Time.deltaTime);
            _ctx.Rb.MoveRotation(nextRotation);

            if (Quaternion.Angle(Transform.rotation, targetRotation) <= _params.RotationCompleteAngle)
            {
                SetState<RvcMoveState>();
                return;
            }
        }

        public override void ManualFixedUpdate() { }
    }
}
