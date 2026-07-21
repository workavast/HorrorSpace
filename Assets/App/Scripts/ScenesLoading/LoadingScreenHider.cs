using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using Avastrad.ScenesLoading;
using UnityEngine;
using VContainer;

namespace App.ScenesLoading
{
	public class LoadingScreenHider : Bootstrap
	{
		[SerializeField] private LoadingConfig loadingConfig;
		
		[Inject] private ISceneLoader _sceneLoader;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			_sceneLoader.HideLoadScreen(loadingConfig.HideDuration);

			return Task.CompletedTask;
		}
	}
}