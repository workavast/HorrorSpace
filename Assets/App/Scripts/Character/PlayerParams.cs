using UnityEngine;

namespace App.Character
{
    [System.Serializable]
    public class PlayerParams
    {
        [Header("Movement")]
        public AnimationCurve VelocityModifierPerGrabObjMass;
        public float MoveSpeed = 5f;
        public float SprintSpeed = 8f;
        public float Acceleration = 10f;
        public float Deceleration = 10f;
        
        [Header("Jump and Air")]
        public float JumpForce = 5f;
        public float Gravity = 9.81f;
        public float AirDrag = 0.1f;
        public float AirAcceleration = 10f;
        
        [Header("Crouch")]
        public float CrouchHeight = 1.3f;
        public float CrouchSpeed = 2f;
        public float CrouchHeightSpeed = 5f;
        public float HeadCollisionDistance = 0.1f;
        public LayerMask ObstacleLayer;
        public float StandCheckDistance = 1.35f;
        public float StandCheckOffset = -0.1f;
        
        [Header("Camera")]
        public float NormalCameraHeight = 2.1f;
        public float CrouchCameraHeight = 0.9f;
        
        [Header("Other")]
        public float CollisionCheckRadius = 0.2f;
    }
}
