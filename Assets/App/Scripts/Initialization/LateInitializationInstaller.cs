using App.LifetimeScopes;
using VContainer;

namespace App.Initialization
{
    public class LateInitializationInstaller : Installer
    {
        public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<LateInitializer>(Lifetime.Singleton).WithParameter(8).AsSelf().AsImplementedInterfaces();
        }
    }
}