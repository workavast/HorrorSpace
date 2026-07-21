using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using VContainer;

namespace App.Initialization
{
    public class LateInitializationBootstrap : Bootstrap
    {
        [Inject] private readonly LateInitializer _lateInitializer;
        
        protected override Task SelfInitialization(CancellationToken cancellationToken)
        {
            _lateInitializer.Initialize();
            
            return Task.CompletedTask;
        }

        protected override void OnDestroyCallback()
        {
            _lateInitializer.Dispose();
        }
    }
}