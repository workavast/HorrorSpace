using App.EscProviding;
using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer;

namespace App.UI.GameplayMenu
{
    public class GameplayMenuByEscActivator : MonoBehaviour, IEscListener
    {
        [SerializeField] private ScreenIdReference screenId;
        
        [Inject] private readonly EscProvider _escProvider;
        [Inject] private readonly ScreensController _screensController;
        
        private void Start() 
            => _escProvider.Sub(this);

        private void OnDestroy() 
            => _escProvider.UnSub(this);

        public void OnEscPressed() 
            => _screensController.SetScreen(screenId);
    }
}