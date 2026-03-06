using App.LifetimeScopes;
using VContainer;

namespace App.Audio.Sources
{
    public class AudioScopeConfig : ScopeConfig
    {
        public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<AudioFactory>(Lifetime.Singleton).NonLazySelf();
        }
    }
}