using Avastrad.ScenesLoading;
using Avastrad.VContainerExt.Installers;

namespace App.ScenesLoading
{
	public class DefaultSceneLoader : Avastrad.ScenesLoading.DefaultSceneLoader, INonLazy
	{
		public DefaultSceneLoader(ILoadingScreen loadingScreen, int loadingSceneIndex) : base(loadingScreen, loadingSceneIndex)
		{
		}
	}
}