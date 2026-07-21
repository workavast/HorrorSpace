using System.Threading;
using System.Threading.Tasks;
using App.Audio.Sources;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer;

namespace App.Usable
{
	public class TryUseSoundPlayerBootstrap : Bootstrap
	{
		[Inject] private AudioFactory _audioFactory;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var all = FindObjectsByType<TryUseSoundPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var tryUseSoundPlayer in all)
				tryUseSoundPlayer.Initialize(_audioFactory);
			
			return Task.CompletedTask;
		}
	}
}