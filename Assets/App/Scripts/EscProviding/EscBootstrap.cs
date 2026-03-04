using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.EscProviding
{
	public class EscBootstrap : Bootstrap
	{
		[SerializeField] private InputActionReference escRef;
		
		private EscTrigger _escTrigger;
		
		protected override Task SelfInitialization(CancellationToken cancellationToken)
		{
			// _escTrigger = new EscTrigger(escRef);
			return Task.CompletedTask;
		}
	}
}