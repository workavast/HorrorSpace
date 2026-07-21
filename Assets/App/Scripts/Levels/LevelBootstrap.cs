using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer;

namespace App.Levels
{
    public class LevelBootstrap : Bootstrap
    {
        [SerializeField] private InterfaceReference<ILevel>[] levels;
        
        [Inject] private IObjectResolver _objectResolver;

        protected override Task SelfInitialization(CancellationToken cancellationToken)
        {
            if (levels != null && levels.Length > 0)
                foreach (var level in levels) 
                    level.Value.Initialize(_objectResolver);                
            
            return Task.CompletedTask;
        }
    }
}