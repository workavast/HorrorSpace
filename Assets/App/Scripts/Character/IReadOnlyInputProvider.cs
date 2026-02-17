using UnityEngine;

namespace App.Character
{
    public interface IReadOnlyInputProvider
    {
        Vector2 MoveInput { get; }
        bool IsSprintPressed { get; }
        bool IsCrouchPressed { get; }
        bool JumpPressedThisFrame { get; }
    }
}
