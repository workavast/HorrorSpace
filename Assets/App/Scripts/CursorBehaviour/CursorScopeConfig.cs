using App.LifetimeScopes;
using VContainer;

namespace App.CursorBehaviour
{
	public class CursorScopeConfig : ScopeConfig
	{
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<CursorVisibilityChanger>(Lifetime.Singleton).NonLazy();
		}
	}
}