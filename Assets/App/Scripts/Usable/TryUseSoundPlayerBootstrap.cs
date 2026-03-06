using System.Threading;
using System.Threading.Tasks;
using App.Audio.Sources;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer.Unity;
using VContainer;

namespace App.Usable
{
	public class TryUseSoundPlayerBootstrap : Bootstrap
	{
		[SerializeField] private LifetimeScope lifetimeScope;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var all = FindObjectsByType<TryUseSoundPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var tryUseSoundPlayer in all)
				tryUseSoundPlayer.Initialize(lifetimeScope.Container.Resolve<AudioFactory>());
			
			return Task.CompletedTask;
		}
	}
}