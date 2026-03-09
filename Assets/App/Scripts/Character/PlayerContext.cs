using System;
using App.ObjectHandling;
using KinematicCharacterController;
using UnityEngine;

namespace App.Character
{
    [Serializable]
    public class PlayerContext
    {
        //Core
        public readonly KinematicCharacterMotor Motor;
        public readonly PlayerInputManager InputManager;
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

        //Hands
        public readonly CursorViewToggler CursorViewToggler;
        public bool HasHandledObject => HandledObject != null;
        public float DraggedObjMass;
        public IHandleObject HandledObject;
        public float CurrentDistance;
        public Quaternion TargetRotationRelativeToPlayer;
        public float DefaultObjectLinearDamping;
        public float DefaultObjectAngularDamping;
        public Vector3 DragPoint;
        public Vector3 PickupOffsetLocal;             // local offset from object origin to pickup point

        public PlayerContext(KinematicCharacterMotor motor, CameraController cameraController, Transform t, CursorViewToggler cursorViewToggler, PlayerInputManager inputManager)
        {
            Motor = motor;
            CameraController = cameraController;
            Transform = t;
            CursorViewToggler = cursorViewToggler;
            InputManager = inputManager;

            DefaultHeight = Motor.Capsule.height;
        }
        
        public void SetDefaultObjectDampings(float defaultObjectLinearDamping, float defaultObjectAngularDamping)
        {
            DefaultObjectLinearDamping = defaultObjectLinearDamping;
            DefaultObjectAngularDamping = defaultObjectAngularDamping;
        }
    }
}
