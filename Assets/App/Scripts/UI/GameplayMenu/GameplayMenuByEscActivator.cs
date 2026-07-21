using App.EscProviding;
using App.Initialization;
using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer;

namespace App.UI.GameplayMenu
{
    public class GameplayMenuByEscActivator : MonoBehaviour, IEscListener
    {
        [SerializeField] private ScreenIdReference screenId;
        
        private EscProvider _escProvider;
        private ScreensController _screensController;

        [Inject]
        public void Construct(ILateInitializer lateInitializer, EscProvider  escProvider, ScreensController  screensController)
        {
            _escProvider = escProvider;
            _screensController = screensController;
            
            lateInitializer.Register(Initialize);
        }

        private void Initialize() 
            => _escProvider.Sub(this);

        private void OnDestroy() 
            => _escProvider?.UnSub(this);

        public void OnEscPressed() 
            => _screensController.SetScreen(screenId);
    }
}