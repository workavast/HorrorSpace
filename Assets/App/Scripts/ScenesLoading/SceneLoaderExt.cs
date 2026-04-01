using Avastrad.ScenesLoading;
using Avastrad.Utils.ScenesReferencing;

namespace App.ScenesLoading
{
	public static class SceneLoaderExt
	{
		public static void LoadSceneExt(this ISceneLoader loader, SceneReference sceneRef, LoadingConfig loadingConfig)
		{
			loader.LoadScene(sceneRef.SceneIndex, loadingConfig.ShowDuration);
		}
	}
}