using Avastrad.UI.UiSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.UI
{
    [RequireComponent(typeof(Button))]
    public class RevertScreenBtn : MonoBehaviour
    {
        [Inject] private readonly ScreensController _screensController;

        private void Awake() 
            => GetComponent<Button>().onClick.AddListener(_screensController.Revert);

        private void OnDestroy() 
            => GetComponent<Button>().onClick.RemoveListener(_screensController.Revert);
    }
}