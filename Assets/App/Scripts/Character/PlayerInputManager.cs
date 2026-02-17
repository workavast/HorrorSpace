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

        private bool _jumpPressed;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool IsSprintPressed { get; private set; }
        public bool JumpPressedThisFrame { get; private set; }
        public bool IsCrouchPressed { get; private set; }
        public bool CrouchPressedThisFrame { get; private set; }

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
        }
    }
}
