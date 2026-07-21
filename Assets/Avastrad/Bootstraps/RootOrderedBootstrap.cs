using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Avastrad.Bootstraps
{
    public class RootOrderedBootstrap : Bootstrap
    {
        [SerializeField] internal Order _order = Order.Start;
        
        public async void StartInitialization()
        {
            try
            {
                await Initialize(CancellationToken.None);
            }
            catch (TaskCanceledException e)
            {
                Debug.Log(e);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        protected override Task SelfInitialization(CancellationToken cancellationToken) 
            => Task.CompletedTask;
    }
}