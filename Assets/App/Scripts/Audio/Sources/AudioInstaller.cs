using Avastrad.VContainerExt.Installers;
using VContainer;

namespace App.Audio.Sources
{
    public class AudioInstaller : Installer
    {
        public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<AudioFactory>(Lifetime.Singleton).NonLazySelf();
        }
    }
}