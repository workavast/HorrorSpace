using App.Installers;
using App.LifetimeScopes;
using VContainer;

namespace App.SpeakersSystem.SpeakersTriggers
{
    public class SpeakerEventsDatabaseInstaller : Installer
    {
        public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<SpeakerEventsDatabase>(Lifetime.Singleton).AsSelf();
        }
    }
}