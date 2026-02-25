using System;
using KinematicCharacterController;
using UnityEngine;

namespace App.Character
{
    [Serializable]
    public class PlayerContext
    {
        public readonly CharMover CharMover;
        public readonly KinematicCharacterMotor Motor;
        public readonly CameraController CameraController;
        public readonly Transform Transform;
        public readonly float DefaultHeight;

        public Vector2 VelocityInput;
        public float GravityVelocity;
        public float TargetSpeed;

        public Vector3 Velocity => Motor.Velocity;
        public bool IsInAirJump;
        public bool IsCrouching;
        public bool IsGroundContact => Motor.GroundingStatus.IsStableOnGround;
        public bool IsGrounded => IsGroundContact && GravityVelocity <= 0.01f;

        public PlayerContext(CharMover charMover, KinematicCharacterMotor motor, CameraController cameraController, Transform t)
        {
            CharMover = charMover;
            Motor = motor;
            CameraController = cameraController;
            Transform = t;

            DefaultHeight = Motor.Capsule.height;
        }
    }
}
