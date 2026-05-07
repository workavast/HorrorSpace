using Avastrad.UI.UiSystem;
using VContainer;

namespace App.UI
{
    public class GameEndedScreen : ScreenBase
    {
        [Inject] private readonly ScreensController _screensController;
        
        private void OnEnable() 
            => _screensController.BlockRequest(true);

        private void OnDisable() 
            => _screensController.BlockRequest(false);
    }
}