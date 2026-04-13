using App.LifetimeScopes;
using VContainer;

namespace App.GamePausing
{
	public class GamePauserScopeConfig : ScopeConfig
	{
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<GamePauser>(Lifetime.Singleton);
		}
	}
}