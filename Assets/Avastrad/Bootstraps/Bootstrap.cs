using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Avastrad.Bootstraps
{
    public abstract class Bootstrap : MonoBehaviour
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        protected async Task Initialize(CancellationToken externalToken, LifetimeScope lifetimeScope)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, externalToken);
            var linkedToken = linkedCts.Token;
            
            await SelfInitialization(linkedToken, lifetimeScope);

            linkedToken.ThrowIfCancellationRequested();
            
            await InitializeChildren(linkedToken, lifetimeScope);
        }

        protected virtual void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        protected abstract Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope);
        
        private async Task InitializeChildren(CancellationToken cancellationToken, LifetimeScope lifetimeScope)
        {
            var children = GetChildrenBootstraps();
            foreach (var child in children)
            {
                await child.Initialize(cancellationToken, lifetimeScope);
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
        
        private List<Bootstrap> GetChildrenBootstraps()
        {
            var children = new List<Bootstrap>(gameObject.transform.childCount);
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (child.gameObject.activeSelf && child.TryGetComponent<Bootstrap>(out var childBootstrap)) 
                    children.Add(childBootstrap);
            }

            return children;
        }
    }
}