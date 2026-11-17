using Avastrad.ScenesLoading;
using Avastrad.VContainerExt.Installers;
using UnityEngine;
using VContainer;

namespace App.ScenesLoading
{
	public class SceneLoaderInstaller : Installer
	{
		[SerializeField] private int loadingSceneIndex;
		[SerializeField] private LoadingScreen loadingScreen;
		
		public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
		{
			var screen = Instantiate(loadingScreen);
			DontDestroyOnLoad(screen);
			screen.Initialize();
			
			builder.RegisterInstance<ILoadingScreen>(screen);
			builder.Register<DefaultSceneLoader>(Lifetime.Singleton).WithParameter(loadingSceneIndex).NonLazyInterfaces();
		}
	}
}