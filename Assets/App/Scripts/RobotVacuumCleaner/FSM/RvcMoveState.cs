using System.Collections.Generic;
using UnityEngine;

namespace App.RobotVacuumCleaner.FSM
{
    public class RvcMoveState : RvcBaseState
    {
        public RvcMoveState(RvcContext ctx, RvcParams parameters) : base(ctx, parameters)
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

            if (IsObstacleAhead())
            {
                SetState<RvcIdleState>();
                return;
            }
        }

        public override void ManualFixedUpdate()
        {
            ApplyMovementForce(Transform.forward, _params.MovementForce, _params.MaxMoveSpeed);
        }

        private void ApplyMovementForce(Vector3 direction, float force, float maxSpeed)
        {
            var horizontalVelocity = new Vector3(_ctx.Rb.linearVelocity.x, 0f, _ctx.Rb.linearVelocity.z);
            
            if (horizontalVelocity.magnitude < maxSpeed) 
                _ctx.Rb.AddForce(direction * force, ForceMode.Force);
        }
    }
}
