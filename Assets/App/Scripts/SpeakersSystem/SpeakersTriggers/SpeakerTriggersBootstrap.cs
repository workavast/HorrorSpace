using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer;

namespace App.SpeakersSystem.SpeakersTriggers
{
	public class SpeakerTriggersBootstrap : Bootstrap
	{
		[Inject] private IObjectResolver _objectResolver;

		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var radioPlayers = FindObjectsByType<SpeakerTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var radioPlayer in radioPlayers) 
				radioPlayer.Initialize(_objectResolver);
			
			return Task.CompletedTask;
		}
	}
}