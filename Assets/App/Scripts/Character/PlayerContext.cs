using System;
using App.Character.Hand;
using App.ObjectHandling;
using KinematicCharacterController;
using UnityEngine;

namespace App.Character
{
    [Serializable]
    public class PlayerContext
    {
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

        private readonly HandContext _handContext;
        
        public bool HasHandledObject => _handContext.HasHandledObject;
        public float DraggedObjMass => _handContext.DraggedObjMass;

        public PlayerContext(KinematicCharacterMotor motor, CameraController cameraController, Transform t, HandContext handContext)
        {
            Motor = motor;
            CameraController = cameraController;
            Transform = t;
            _handContext = handContext;

            DefaultHeight = Motor.Capsule.height;
        }
    }
}
