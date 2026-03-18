using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Avastrad.Bootstraps
{
    public class AwaitBootstrap : Bootstrap
    {
        [SerializeField] private int awaitDelay = 1000;

        protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope) 
            => Await(cancellationToken);

        private async Task Await(CancellationToken cancellationToken) 
            => await Task.Delay(awaitDelay, cancellationToken);
    }
}
