using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Avastrad.Bootstraps
{
    public class AwaitBootstrap : Bootstrap
    {
        [SerializeField, Tooltip("milliseconds")] private int awaitDelay = 1000;

        protected override Task SelfInitialization(CancellationToken cancellationToken) 
            => Await(cancellationToken);

        private async Task Await(CancellationToken cancellationToken) 
            => await Task.Delay(awaitDelay, cancellationToken);
    }
}
