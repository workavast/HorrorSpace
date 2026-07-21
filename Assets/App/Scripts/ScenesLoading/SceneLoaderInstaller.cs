using App.LifetimeScopes;
using Avastrad.ScenesLoading;
using UnityEngine;
using VContainer;

namespace App.ScenesLoading
{
	public class SceneLoaderInstaller : Installer
	{
		[SerializeField] private int loadingSceneIndex;
		[SerializeField] private LoadingScreen loadingScreen;
		
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			var screen = Instantiate(loadingScreen);
			DontDestroyOnLoad(screen);
			screen.Initialize();
			
			builder.RegisterInstance<ILoadingScreen>(screen);
			builder.Register<DefaultSceneLoader>(Lifetime.Singleton).WithParameter(loadingSceneIndex).NonLazyInterfaces();
		}
	}
}