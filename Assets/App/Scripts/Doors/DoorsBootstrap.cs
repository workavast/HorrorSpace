using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;
using VContainer.Unity;

namespace App.Doors
{
	public class DoorsBootstrap : Bootstrap
	{
		[SerializeField] private LifetimeScope lifetimeScope;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var doors = FindObjectsByType<DoorController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (var door in doors)
				door.Initialize();
			
			return Task.CompletedTask;
		}
	}
}