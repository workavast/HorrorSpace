using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using Avastrad.ScenesLoading;
using VContainer;
using VContainer.Unity;

namespace App.ScenesLoading
{
	public class LoadTargetSceneBootstrap : Bootstrap
	{
		protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
		{
			var sceneLoader = lifeTimeScope.Container.Resolve<ISceneLoader>();
			sceneLoader.LoadTargetScene();
			
			return Task.CompletedTask;
		}
	}
}