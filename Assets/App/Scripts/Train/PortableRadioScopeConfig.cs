using App.LifetimeScopes;
using VContainer;

namespace App
{
	public class PortableRadioScopeConfig : ScopeConfig
	{
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<PortableRadio>(Lifetime.Singleton).NonLazySelf();
		}
	}
}