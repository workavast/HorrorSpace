using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using Avastrad.ScenesLoading;
using Avastrad.Utils.ScenesReferencing;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.ScenesLoading
{
	public class LoadSceneBootstrap : Bootstrap
	{
		[SerializeField] private LoadingConfig loadingConfig;
		[SerializeField] private SceneReference sceneRef;
		[SerializeField] private LifetimeScope lifetimeScope;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var sceneLoader = lifetimeScope.Container.Resolve<ISceneLoader>();
			sceneLoader.LoadScene(sceneRef.SceneIndex, loadingConfig.ShowDuration);
			
			return Task.CompletedTask;
		}
	}
}