using App.LifetimeScopes;
using VContainer;

namespace App.CursorBehaviour
{
	public class CursorEscReaderScopeConfig : ScopeConfig
	{
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<CursorVisibilityEscReader>(Lifetime.Singleton).NonLazySelf();
		}
	}
}