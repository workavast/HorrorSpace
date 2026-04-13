using UnityEngine;
using VContainer;

namespace App.GamePausing
{
    public class GamePauserTrigger : MonoBehaviour
    {
        [Inject] private readonly GamePauser _cursorVisibilityChanger;

        private void OnEnable() 
            => _cursorVisibilityChanger.AddRequest();

        private void OnDisable() 
            => _cursorVisibilityChanger.RemoveRequest();
    }
}