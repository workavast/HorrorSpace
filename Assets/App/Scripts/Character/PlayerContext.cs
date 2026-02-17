using System;
using UnityEngine;

namespace App.Character
{
    [Serializable]
    public class PlayerContext
    {
        public readonly CharacterController CharacterController;
        public readonly CameraController CameraController;
        public readonly Transform Transform;
        public readonly Animator Animator;
        public readonly float DefaultHeight;

        public Vector3 Velocity;
        public Vector3 JumpHorizontalVelocity;
        public bool IsInAirJump;
        public bool IsCrouching;
        public bool IsGroundContact;
        public bool IsGrounded => IsGroundContact && Velocity.y <= 0.01f;

        public PlayerContext(CharacterController cc, CameraController cameraController, Transform t, Animator a)
        {
            CharacterController = cc;
            CameraController = cameraController;
            Transform = t;
            Animator = a;

            DefaultHeight = cc.height;
        }
    }
}
