using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using Avastrad.UI.UiSystem;
using VContainer;

namespace App.UI
{
    public class UiBootstrap : Bootstrap
    {
        [Inject] private ScreensController _screensController;
        
        protected override Task SelfInitialization(CancellationToken cancellationToken)
        {
            _screensController.Initialize();
            return Task.CompletedTask;
        }
    }
}