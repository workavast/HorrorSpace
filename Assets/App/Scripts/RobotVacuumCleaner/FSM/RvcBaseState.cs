using Avastrad.FSM;
using UnityEngine;

namespace App.RobotVacuumCleaner
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
        {
            var center = _ctx.OccupationCheckCollider.transform.TransformPoint(_ctx.OccupationCheckCollider.center);
            var extents = Vector3.Scale(_ctx.OccupationCheckCollider.size * 0.5f, _ctx.OccupationCheckCollider.transform.lossyScale);
            var rotation = _ctx.OccupationCheckCollider.transform.rotation;
            return Physics.CheckBox(center, extents, rotation, _params.OccupiedLayer, QueryTriggerInteraction.Ignore);
        }
        
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
