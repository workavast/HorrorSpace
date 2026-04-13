using App.LifetimeScopes;
using UnityEngine;
using VContainer;

namespace App.SpeakersSystem
{
	public class MonologSpeakersScopeConfig : ScopeConfig
	{
		[SerializeField] private MonologSpeakers monologSpeakers;
		
		public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
		{
			builder.RegisterInstance<MonologSpeakers>(monologSpeakers);
		}
	}
}