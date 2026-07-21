using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer.Unity;

namespace App.Installers
{
	public class LifetimeScopeBootstrap : Bootstrap
	{
		[SerializeField] private LifetimeScope _lifeTimeScope;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			_lifeTimeScope.Build();
			
			return Task.CompletedTask;
		}
	}
}