using App.LifetimeScopes;
using VContainer;

namespace App.SpeakersSystem.SpeakersTriggers
{
    public class SpeakerEventsDatabaseScopeConfig : ScopeConfig
    {
        public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<SpeakerEventsDatabase>(Lifetime.Singleton).AsSelf();
        }
    }
}