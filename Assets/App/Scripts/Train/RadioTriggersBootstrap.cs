using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer.Unity;

namespace App
{
	public class RadioTriggersBootstrap : Bootstrap
	{
		protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
		{
			var radioTriggers = FindObjectsByType<RadioPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var radioTrigger in radioTriggers) 
				radioTrigger.Initialize(lifeTimeScope.Container);
			
			return Task.CompletedTask;
		}
	}
}