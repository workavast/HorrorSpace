using App.LifetimeScopes;
using Avastrad.VContainerExt.Installers;
using VContainer;

namespace App.SpeakersSystem.SpeakersTriggers
{
    public class SpeakerEventsDatabaseInstaller : Installer
    {
        public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<SpeakerEventsDatabase>(Lifetime.Singleton).AsSelf();
        }
    }
}