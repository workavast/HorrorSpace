using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer.Unity;

namespace App.SpeakersSystem.SpeakersTriggers
{
	public class SpeakerTriggersBootstrap : Bootstrap
	{
		protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
		{
			var radioPlayers = FindObjectsByType<SpeakerTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var radioPlayer in radioPlayers) 
				radioPlayer.Initialize(lifeTimeScope.Container);
			
			return Task.CompletedTask;
		}
	}
}