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

        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private bool _isSprintPressed;
        private bool _jumpPressed;
        private bool _jumpPressedThisFrame;
        private bool _crouchPressed;
        private bool _crouchPressedThisFrame;

        public Vector2 MoveInput => _moveInput;
        public Vector2 LookInput => _lookInput;
        public bool IsSprintPressed => _isSprintPressed;
        public bool JumpPressedThisFrame => _jumpPressedThisFrame;
        public bool IsCrouchPressed => _crouchPressed;
        public bool CrouchPressedThisFrame => _crouchPressedThisFrame;

        private void OnEnable()
        {
            EnableInputActions();
        }

        private void OnDisable()
        {
            DisableInputActions();
        }

        private void Update()
        {
            ReadInputs();
        }

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
        }

        private void ReadInputs()
        {
            _moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
            _lookInput = lookAction != null ? lookAction.action.ReadValue<Vector2>() : Vector2.zero;
            _isSprintPressed = sprintAction != null ? sprintAction.action.IsPressed() : false;
            
            // Отслеживаем нажатие прыжка в этом кадре
            bool jumpCurrentlyPressed = jumpAction != null ? jumpAction.action.IsPressed() : false;
            _jumpPressedThisFrame = jumpCurrentlyPressed && !_jumpPressed;
            _jumpPressed = jumpCurrentlyPressed;
            
            // Отслеживаем нажатие приседания в этом кадре
            bool crouchCurrentlyPressed = crouchAction != null ? crouchAction.action.IsPressed() : false;
            _crouchPressedThisFrame = crouchCurrentlyPressed && !_crouchPressed;
            _crouchPressed = crouchCurrentlyPressed;
        }
    }
}
