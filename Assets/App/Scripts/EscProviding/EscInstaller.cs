using App.LifetimeScopes;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace App.EscProviding
{
	public class EscInstaller : Installer
	{
		[SerializeField] private InputActionReference escRef;
		
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<EscProvider>(Lifetime.Singleton);
			builder.Register<EscTrigger>(Lifetime.Singleton).WithParameter(escRef).NonLazySelf();
		}
	}
}