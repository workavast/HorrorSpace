using System.Threading;
using System.Threading.Tasks;
using App.SecurityCheckpoint;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer;

namespace App.Global
{
	public class GameEndInitializer : Bootstrap
	{
		[Inject] private IObjectResolver _objectResolver;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var all = FindObjectsByType<GameEndTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var gameEndTrigger in all)
				gameEndTrigger.Initialize(_objectResolver);
			
			return Task.CompletedTask;
		}
	}
}