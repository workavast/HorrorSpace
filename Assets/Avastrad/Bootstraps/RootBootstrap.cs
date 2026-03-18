using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Avastrad.Bootstraps
{
    public class RootBootstrap : Bootstrap
    {
        [SerializeField] private LifetimeScope lifetimeScope;
        
        public async void Start()
        {
            try
            {
                await Initialize(CancellationToken.None, lifetimeScope);
            }
            catch (TaskCanceledException e)
            {
                Debug.Log(e);
            }
        }

        protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope) 
            => Task.CompletedTask;
    }
}