using Avastrad.VContainerExt.Installers;
using VContainer;

namespace App.GamePausing
{
	public class GamePauserInstaller : Installer
	{
		public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<GamePauser>(Lifetime.Singleton);
		}
	}
}