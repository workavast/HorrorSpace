using App.Installers;
using App.LifetimeScopes;
using VContainer;
using VContainer.Unity;

namespace App.CursorBehaviour
{
	public class CursorInstaller : Installer
	{
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<CursorVisibilityChanger>(Lifetime.Singleton).NonLazySelf();
		}
	}
}