using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer.Unity;

namespace App.Levels
{
    public class LevelBootstrap : Bootstrap
    {
        [SerializeField] private InterfaceReference<ILevel>[] levels;
        
        protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
        {
            if (levels != null && levels.Length > 0)
                foreach (var level in levels) 
                    level.Value.Initialize(lifeTimeScope.Container);                
            
            return Task.CompletedTask;
        }
    }
}