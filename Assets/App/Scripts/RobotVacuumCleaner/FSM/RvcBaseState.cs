using Avastrad.Extensions;
using Avastrad.FSM;
using UnityEngine;

namespace App.RobotVacuumCleaner.FSM
{
    public abstract class RvcBaseState : StateBase
    {
        protected readonly RvcContext _ctx;
        protected readonly RvcParams _params;
        
        protected Transform Transform => _ctx.Transform;

        protected RvcBaseState(RvcContext ctx, RvcParams parameters)
        {
            _ctx = ctx;
            _params = parameters;
        }

        protected bool IsOccupied() 
            => _ctx.OccupationCheckCollider.CheckBox(_params.OccupiedLayer);

        protected bool IsObstacleAhead() 
        {
            var distance = _params.ObstacleCheckDistance;
            var direction = Transform.forward;
            
            foreach (var forwardObstacleCheck in _ctx.ForwardObstacleChecks)
            {
                var origin = forwardObstacleCheck.transform.position;
                if (Physics.Raycast(origin, direction, distance, _params.GroundLayer, QueryTriggerInteraction.Ignore))
                    return true;
            }

            return false;
        }
        
        protected bool IsGrounded() 
        {
            var origin = _ctx.GroundCheckPoint.transform.position;
            return Physics.Raycast(origin, -Transform.up, _params.GroundCheckDistance, _params.GroundLayer, QueryTriggerInteraction.Ignore);
        }
    }
}
