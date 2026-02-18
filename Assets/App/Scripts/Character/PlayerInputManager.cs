using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Character
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference lookAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference crouchAction;
        [SerializeField] private InputActionReference interactAction;

        [SerializeField] private InputActionReference pickupAction;
        [SerializeField] private InputActionReference rotateAction;
        [SerializeField] private InputActionReference zoomAction;

        private bool _jumpPressed;
        
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool IsSprintPressed { get; private set; }
        public bool JumpPressedThisFrame { get; private set; }
        public bool IsCrouchPressed { get; private set; }
        public bool CrouchPressedThisFrame { get; private set; }
        public bool InteractPressedThisFrame { get; private set; }
        public bool IsPickupPressed { get; private set; }
        public bool IsRotatePressed { get; private set; }
        public float ZoomDelta { get; private set; }

        private void OnEnable() 
            => EnableInputActions();

        private void OnDisable() 
            => DisableInputActions();

        private void Update() 
            => ReadInputs();

        private void EnableInputActions()
        {
            if (moveAction != null)
                moveAction.action.Enable();
            if (lookAction != null)
                lookAction.action.Enable();
            if (sprintAction != null)
                sprintAction.action.Enable();
            if (jumpAction != null)
                jumpAction.action.Enable();
            if (crouchAction != null)
                crouchAction.action.Enable();
            if (interactAction != null)
                interactAction.action.Enable();
            if (pickupAction != null)
                pickupAction.action.Enable();
            if (rotateAction != null)
                rotateAction.action.Enable();
            if (zoomAction != null)
                zoomAction.action.Enable();
        }

        private void DisableInputActions()
        {
            if (moveAction != null)
                moveAction.action.Disable();
            if (lookAction != null)
                lookAction.action.Disable();
            if (sprintAction != null)
                sprintAction.action.Disable();
            if (jumpAction != null)
                jumpAction.action.Disable();
            if (crouchAction != null)
                crouchAction.action.Disable();
            if (interactAction != null)
                interactAction.action.Disable();
            if (pickupAction != null)
                pickupAction.action.Disable();
            if (rotateAction != null)
                rotateAction.action.Disable();
            if (zoomAction != null)
                zoomAction.action.Disable();
        }

        private void ReadInputs()
        {
            MoveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
            LookInput = lookAction != null ? lookAction.action.ReadValue<Vector2>() : Vector2.zero;
            IsSprintPressed = sprintAction != null ? sprintAction.action.IsPressed() : false;
            
            // Отслеживаем нажатие прыжка в этом кадре
            var jumpCurrentlyPressed = jumpAction != null ? jumpAction.action.IsPressed() : false;
            JumpPressedThisFrame = jumpCurrentlyPressed && !_jumpPressed;
            _jumpPressed = jumpCurrentlyPressed;
            
            // Отслеживаем нажатие приседания в этом кадре
            var crouchCurrentlyPressed = crouchAction != null ? crouchAction.action.IsPressed() : false;
            CrouchPressedThisFrame = crouchCurrentlyPressed && !IsCrouchPressed;
            IsCrouchPressed = crouchCurrentlyPressed;

            // Interact - rising edge using action.triggered
            InteractPressedThisFrame = interactAction != null && interactAction.action != null && interactAction.action.triggered;

            IsPickupPressed = pickupAction != null ? pickupAction.action.IsPressed() : false;
            IsRotatePressed = rotateAction != null ? rotateAction.action.IsPressed() : false;
            ZoomDelta = zoomAction != null ? zoomAction.action.ReadValue<Vector2>().y : 0;
        }
    }
}
