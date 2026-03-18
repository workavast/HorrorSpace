using System.Threading;
using System.Threading.Tasks;
using App.SecurityCheckpoint;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Global
{
	public class GameEndInitializer : Bootstrap
	{
		protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
		{
			var all = FindObjectsByType<GameEndTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var gameEndTrigger in all)
				gameEndTrigger.Initialize(lifeTimeScope.Container);
			
			return Task.CompletedTask;
		}
	}
}