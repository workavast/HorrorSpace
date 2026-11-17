using Avastrad.VContainerExt.Installers;
using VContainer;
using VContainer.Unity;

namespace App.CursorBehaviour
{
	public class CursorInstaller : Installer
	{
		public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<CursorVisibilityChanger>(Lifetime.Singleton).NonLazySelf();
		}
	}
}