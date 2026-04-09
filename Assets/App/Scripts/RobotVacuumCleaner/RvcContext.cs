using System.Collections.Generic;
using UnityEngine;

namespace App.RobotVacuumCleaner
{
    public class RvcContext
    {
        public readonly Transform Transform;
        public readonly Transform GroundCheckPoint;
        public readonly Rigidbody Rb;
        public readonly BoxCollider OccupationCheckCollider;
        public readonly IReadOnlyList<Transform> ForwardObstacleChecks;
        
        public Vector3 TargetDirection { get; set; }

        public RvcContext(Transform transform, Rigidbody rb, BoxCollider occupationCheckCollider, 
            Transform groundCheckPoint, IReadOnlyList<Transform> forwardObstacleCheck)
        {
            Transform = transform;
            Rb = rb;
            OccupationCheckCollider = occupationCheckCollider;
            ForwardObstacleChecks = forwardObstacleCheck;
            GroundCheckPoint = groundCheckPoint;
            TargetDirection = transform.forward;
        }
    }
}
