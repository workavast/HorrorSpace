using UnityEngine;

namespace App.Character
{
    [System.Serializable]
    public class PlayerParams
    {
        public float MoveSpeed = 5f;
        public float SprintSpeed = 8f;
        public float Acceleration = 10f;
        public float Deceleration = 10f;
        public float GroundDrag = 5f;
        public float JumpForce = 5f;
        public float Gravity = 9.81f;
        public LayerMask GroundLayer;
        public float GroundCheckDistance = 0.1f;
        public float CrouchHeight = 1.3f;
        public float CrouchSpeed = 2f;
        public float CrouchHeightSpeed = 5f;
        public float HeadCollisionDistance = 0.1f;
        public LayerMask ObstacleLayer;
        public float StandCheckDistance = 1.35f;
        public float StandCheckOffset = -0.1f;
        [Space]
        public float CollisionCheckRadius = 0.2f;
        [Space]
        public float NormalCameraHeight = 2.1f;
        public float CrouchCameraHeight = 0.9f;
    }
}
