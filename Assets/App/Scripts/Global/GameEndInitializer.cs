using System.Threading;
using System.Threading.Tasks;
using App.SecurityCheckpoint;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer.Unity;

namespace App.Global
{
	public class GameEndInitializer : Bootstrap
	{
		[SerializeField] private LifetimeScope lifetimeScope;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var all = FindObjectsByType<GameEndTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var gameEndTrigger in all)
				gameEndTrigger.Initialize(lifetimeScope.Container);
			
			return Task.CompletedTask;
		}
	}
}