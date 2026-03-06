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
		[SerializeField] private LifetimeScope lifetimeScope;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var lockerLocks = FindObjectsByType<LockerLock>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var lockerLock in lockerLocks)
				lockerLock.Initialize(lifetimeScope.Container.Resolve<AudioFactory>());
			
			return Task.CompletedTask;
		}
	}
}