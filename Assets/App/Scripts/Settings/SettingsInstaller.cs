using App.Installers;
using Avastrad.Settings;
using UnityEngine;
using VContainer;

namespace App
{
    public class SettingsInstaller : Installer
    {
        [SerializeField] private SettingsConfigsRepository settingsConfigsRepository;
        
        public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<SettingsRepository>(Lifetime.Singleton).WithParameter(settingsConfigsRepository);
        }
    }
}