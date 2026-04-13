using UnityEngine;
using VContainer;

namespace App.CursorBehaviour
{
    public class CursorTrigger : MonoBehaviour
    {
        [Inject] private readonly CursorVisibilityChanger _cursorVisibilityChanger;

        private void OnEnable() 
            => _cursorVisibilityChanger.AddRequest();

        private void OnDisable() 
            => _cursorVisibilityChanger.RemoveRequest();
    }
}