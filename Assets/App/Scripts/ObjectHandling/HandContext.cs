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
        public readonly CursorViewToggler CursorViewToggler;

        public bool HasHandledObject => HandledObject != null;
        
        public IHandleObject HandledObject;
        public float CurrentDistance;
        public Quaternion TargetRotationRelativeToPlayer;
        public float DefaultObjectLinearDamping;
        public float DefaultObjectAngularDamping;

        // Data for dragging behavior
        public Vector3 PickupPoint;                   // world point where object was grabbed
        public Vector3 PickupOffsetLocal;             // local offset from object origin to pickup point

        public HandContext(PlayerInputManager inputManager, CameraController cameraController, Transform playerTransform, CursorViewToggler cursorViewToggler)
        {
            InputManager = inputManager;
            CameraController = cameraController;
            PlayerTransform = playerTransform;
            CursorViewToggler = cursorViewToggler;
        }

        public void SetDefaultObjectDampings(float defaultObjectLinearDamping, float defaultObjectAngularDamping)
        {
            DefaultObjectLinearDamping = defaultObjectLinearDamping;
            DefaultObjectAngularDamping = defaultObjectAngularDamping;
        }
    }
}