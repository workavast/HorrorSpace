using UnityEngine;
using VContainer;

namespace App.CursorBehaviour
{
    public class CursorTrigger : MonoBehaviour
    {
        [Inject] private readonly CursorVisibilityChanger _cursorVisibilityChanger;

        private void OnEnable()
        {
            Debug.Log("Cursor trigger enabled");
            _cursorVisibilityChanger.AddRequest();
        }

        private void OnDisable()
        {
            Debug.Log("Cursor trigger disabled");
            _cursorVisibilityChanger.RemoveRequest();
        }
    }
}