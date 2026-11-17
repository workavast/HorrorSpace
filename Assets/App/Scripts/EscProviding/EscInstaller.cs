using Avastrad.VContainerExt.Installers;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace App.EscProviding
{
	public class EscInstaller : Installer
	{
		[SerializeField] private InputActionReference escRef;
		
		public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
		{
			builder.Register<EscProvider>(Lifetime.Singleton);
			builder.Register<EscTrigger>(Lifetime.Singleton).WithParameter(escRef).NonLazySelf();
		}
	}
}