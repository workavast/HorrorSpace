using System.Threading;
using System.Threading.Tasks;
using App.Audio.Sources;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer;

namespace App.Lockers
{
	public class LockersBootstrap : Bootstrap
	{
		[Inject] private AudioFactory _audioFactory;

		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var lockerLocks = FindObjectsByType<LockerLock>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var lockerLock in lockerLocks)
				lockerLock.Initialize(_audioFactory);
			
			return Task.CompletedTask;
		}
	}
}