using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace App.LifetimeScopes
{
	public abstract class CoreLifetimeScope : LifetimeScope
	{
		protected sealed override void Configure(IContainerBuilder builder)
		{
			var scopesConfigs = GetComponentsInChildren<ScopeConfig>(false);

			foreach (var scopeConfig in scopesConfigs) 
				scopeConfig.Configure(this, builder);

			ResolveNonLazyBindings(builder);
		}
		
		private static void ResolveNonLazyBindings(IContainerBuilder builder)
		{
			builder.RegisterBuildCallback(container =>
			{
				// Force resolve non-lazy bindings
				container.Resolve<IEnumerable<INonLazy>>();
			});
		}
	}
}