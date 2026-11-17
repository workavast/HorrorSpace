using Avastrad.VContainerExt.Installers;
using VContainer;

namespace App.InteractiveObjects
{
    public class InteractionInstaller : Installer
    {
        public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<InteractionContext>(Lifetime.Singleton);
        }
    }
}