using UnityEngine;

namespace App.RobotVacuumCleaner
{
    [System.Serializable]
    public class RvcParams
    {
        [Header("Movement")]
        public float MovementForce = 5f;
        public float MaxMoveSpeed = 1.4f;
        public float RotationSpeed = 120f;
        public float RotationCompleteAngle = 2f;

        [Header("Obstacle detection")]
        public float ObstacleCheckDistance = 0.5f;
        public LayerMask ObstacleLayer = ~0;
        public LayerMask OccupiedLayer = ~0;

        [Header("Ground check")]
        public float GroundCheckDistance = 0.2f;
        public LayerMask GroundLayer = ~0;
    }
}
