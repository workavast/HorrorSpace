using App.Installers;
using App.LifetimeScopes;
using UnityEngine;
using VContainer;

namespace App.SpeakersSystem
{
	public class MonologSpeakersInstaller : Installer
	{
		[SerializeField] private MonologSpeakers monologSpeakers;
		
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.RegisterInstance<MonologSpeakers>(monologSpeakers);
		}
	}
}