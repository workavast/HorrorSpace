using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using Avastrad.ScenesLoading;
using VContainer;

namespace App.ScenesLoading
{
	public class LoadTargetSceneBootstrap : Bootstrap
	{
		[Inject] private ISceneLoader _sceneLoader;

		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			_sceneLoader.LoadTargetScene();
			
			return Task.CompletedTask;
		}
	}
}