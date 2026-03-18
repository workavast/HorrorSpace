using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using Avastrad.ScenesLoading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.ScenesLoading
{
	public class LoadingScreenHider : Bootstrap
	{
		[SerializeField] private LoadingConfig loadingConfig;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
		{
			var sceneLoader = lifeTimeScope.Container.Resolve<ISceneLoader>();
			sceneLoader.HideLoadScreen(loadingConfig.HideDuration);

			return Task.CompletedTask;
		}
	}
}