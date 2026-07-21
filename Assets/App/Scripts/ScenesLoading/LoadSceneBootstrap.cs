using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using Avastrad.ScenesLoading;
using Avastrad.Utils.ScenesReferencing;
using UnityEngine;
using VContainer;

namespace App.ScenesLoading
{
	public class LoadSceneBootstrap : Bootstrap
	{
		[SerializeField] private LoadingConfig loadingConfig;
		[SerializeField] private SceneReference sceneRef;
		
		[Inject] private ISceneLoader _sceneLoader;

		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			_sceneLoader.LoadScene(sceneRef.SceneIndex, loadingConfig.ShowDuration);
			
			return Task.CompletedTask;
		}
	}
}