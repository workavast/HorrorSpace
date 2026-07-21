using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;

namespace App.Doors
{
	public class DoorsBootstrap : Bootstrap
	{
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			var doors = FindObjectsByType<DoorController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (var door in doors)
				door.Initialize();
			
			return Task.CompletedTask;
		}
	}
}