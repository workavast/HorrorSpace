using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer.Unity;

namespace App.PortableRadioSystem
{
	public class RadioPlayersBootstrap : Bootstrap
	{
		protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
		{
			var radioPlayers = FindObjectsByType<RadioPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var radioPlayer in radioPlayers) 
				radioPlayer.Initialize(lifeTimeScope.Container);
			
			return Task.CompletedTask;
		}
	}
}