using Avastrad.VContainerExt.Installers;
using VContainer;

namespace App.CursorBehaviour
{
	public class CursorEscReaderInstaller : Installer
	{
		public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<CursorVisibilityEscReader>(Lifetime.Singleton).NonLazySelf();
		}
	}
}