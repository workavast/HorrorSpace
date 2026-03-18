using System.Threading;
using System.Threading.Tasks;
using App.Audio.Sources;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Lockers
{
	public class LockersBootstrap : Bootstrap
	{
		protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
		{
			var lockerLocks = FindObjectsByType<LockerLock>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var lockerLock in lockerLocks)
				lockerLock.Initialize(lifeTimeScope.Container.Resolve<AudioFactory>());
			
			return Task.CompletedTask;
		}
	}
}