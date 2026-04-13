using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using VContainer.Unity;

namespace App.LifetimeScopes
{
	public class LifetimeScopeBootstrap : Bootstrap
	{
		protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
		{
			lifeTimeScope.Build();
			
			return Task.CompletedTask;
		}
	}
}