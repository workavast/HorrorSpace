using UnityEngine;

namespace App.Character
{
    public class InputProvider : IReadOnlyInputProvider
    {
        public Vector2 MoveInput { get; private set; }
        public bool IsSprintPressed { get; private set; }
        public bool IsCrouchPressed { get; private set; }
        public bool JumpPressedThisFrame { get; private set; }

        public void SetMove(Vector2 value) => MoveInput = value;
        public void SetSprint(bool value) => IsSprintPressed = value;
        public void SetCrouch(bool value) => IsCrouchPressed = value;
        public void SetJumpThisFrame(bool value) => JumpPressedThisFrame = value;
    }
}
