using Avastrad.UI.UiSystem;
using Avastrad.UI.UiSystem.Ids;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.UI
{
    [RequireComponent(typeof(Button))]
    public class SetScreenBtn : MonoBehaviour
    {
        [SerializeField] private ScreenIdReference screenId;
        
        [Inject] private UiRoot _uiRoot;
        
        private void Awake() 
            => GetComponent<Button>().onClick.AddListener(SetScreen);

        private void OnDestroy() 
            => GetComponent<Button>().onClick.RemoveListener(SetScreen);

        private void SetScreen() 
            => _uiRoot.Set(screenId);
    }
}