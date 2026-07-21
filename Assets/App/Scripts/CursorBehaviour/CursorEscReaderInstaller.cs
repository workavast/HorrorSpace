using App.LifetimeScopes;
using VContainer;

namespace App.CursorBehaviour
{
	public class CursorEscReaderInstaller : Installer
	{
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<CursorVisibilityEscReader>(Lifetime.Singleton).NonLazySelf();
		}
	}
}