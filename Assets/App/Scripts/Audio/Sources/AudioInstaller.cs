using App.LifetimeScopes;
using VContainer;

namespace App.Audio.Sources
{
    public class AudioInstaller : Installer
    {
        public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<AudioFactory>(Lifetime.Singleton).NonLazySelf();
        }
    }
}