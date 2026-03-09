using App.HandlingObjects;
using App.HandlingObjects.HoldableObjects;
using UnityEngine;

namespace App.Character.Hand
{
	public class HandContext
	{
		public readonly PlayerInputManager InputManager;
		public readonly CameraController CameraController;
		public readonly Transform Transform;
		public readonly CursorViewToggler CursorViewToggler;
		
		public bool HasHandledObject => HandledObj != null;
		public IHandleObject HandledObj;
		public float DraggedObjMass;
		public float CurrentDistance;
		public Quaternion TargetRotationRelativeToPlayer;
		public float DefaultObjectLinearDamping;
		public float DefaultObjectAngularDamping;

		public HandContext(PlayerInputManager inputManager, CameraController cameraController, Transform transform, CursorViewToggler cursorViewToggler)
		{
			InputManager = inputManager;
			CameraController = cameraController;
			Transform = transform;
			CursorViewToggler = cursorViewToggler;
		}

		public void SetDefaultObjectDampings(float defaultObjectLinearDamping, float defaultObjectAngularDamping)
		{
			DefaultObjectLinearDamping = defaultObjectLinearDamping;
			DefaultObjectAngularDamping = defaultObjectAngularDamping;
		}
	}
}