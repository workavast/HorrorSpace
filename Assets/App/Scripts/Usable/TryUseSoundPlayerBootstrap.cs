using System.Threading;
using System.Threading.Tasks;
using App.Audio.Sources;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Usable
{
	public class TryUseSoundPlayerBootstrap : Bootstrap
	{
		protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
		{
			var all = FindObjectsByType<TryUseSoundPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var tryUseSoundPlayer in all)
				tryUseSoundPlayer.Initialize(lifeTimeScope.Container.Resolve<AudioFactory>());
			
			return Task.CompletedTask;
		}
	}
}