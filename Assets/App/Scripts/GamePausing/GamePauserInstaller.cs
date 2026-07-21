using App.Installers;
using App.LifetimeScopes;
using VContainer;

namespace App.GamePausing
{
	public class GamePauserInstaller : Installer
	{
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<GamePauser>(Lifetime.Singleton);
		}
	}
}