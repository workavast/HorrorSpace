using App.LifetimeScopes;
using Avastrad.Settings;
using UnityEngine;
using VContainer;

namespace App
{
    public class SettingsScopeConfig : ScopeConfig
    {
        [SerializeField] private SettingsConfigsRepository settingsConfigsRepository;
        
        public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<SettingsRepository>(Lifetime.Singleton).WithParameter(settingsConfigsRepository);
        }
    }
}