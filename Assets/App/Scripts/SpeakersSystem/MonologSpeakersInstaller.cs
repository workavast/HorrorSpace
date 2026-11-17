using Avastrad.VContainerExt.Installers;
using UnityEngine;
using VContainer;

namespace App.SpeakersSystem
{
	public class MonologSpeakersInstaller : Installer
	{
		[SerializeField] private MonologSpeakers monologSpeakers;
		
		public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
		{
			builder.RegisterInstance<MonologSpeakers>(monologSpeakers);
		}
	}
}