using Avastrad.UI.UiSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.UI
{
    [RequireComponent(typeof(Button))]
    public class SetScreenBtn : MonoBehaviour
    {
        [SerializeField] private ScreenIdReference screenId;
        
        [Inject] private readonly ScreensController _screensController;
        
        private void Awake() 
            => GetComponent<Button>().onClick.AddListener(SetScreen);

        private void OnDestroy() 
            => GetComponent<Button>().onClick.RemoveListener(SetScreen);

        private void SetScreen() 
            => _screensController.SetScreen(screenId);
    }
}