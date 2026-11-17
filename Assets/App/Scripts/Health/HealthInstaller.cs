using Avastrad.VContainerExt.Installers;
using VContainer;

namespace App.Health
{
    public class HealthInstaller : Installer
    {
        public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<HealthContext>(Lifetime.Singleton);
        }
    }
}