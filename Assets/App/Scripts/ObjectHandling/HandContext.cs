using System;
using App.Character;
using UnityEngine;

namespace App.ObjectHandling
{
    [Serializable]
    public class HandContext
    {
        public readonly PlayerInputManager InputManager;
        public readonly CameraController CameraController;
        public readonly Transform PlayerTransform;

        public bool HasHandledObject => HandledObject != null;
        
        public IHandleObject HandledObject;
        public float CurrentDistance;
        public Quaternion TargetRotationRelativeToPlayer;
        public float DefaultObjectLinearDamping;
        public float DefaultObjectAngularDamping;

        public HandContext(PlayerInputManager inputManager, CameraController cameraController, Transform playerTransform)
        {
            InputManager = inputManager;
            CameraController = cameraController;
            PlayerTransform = playerTransform;
        }

        public void SetDefaultObjectDampings(float defaultObjectLinearDamping, float defaultObjectAngularDamping)
        {
            DefaultObjectLinearDamping = defaultObjectLinearDamping;
            DefaultObjectAngularDamping = defaultObjectAngularDamping;
        }
    }
}