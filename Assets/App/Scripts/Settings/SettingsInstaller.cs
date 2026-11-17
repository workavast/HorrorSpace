using Avastrad.Settings;
using Avastrad.VContainerExt.Installers;
using UnityEngine;
using VContainer;

namespace App
{
    public class SettingsInstaller : Installer
    {
        [SerializeField] private SettingsConfigsRepository settingsConfigsRepository;
        
        public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<SettingsRepository>(Lifetime.Singleton).WithParameter(settingsConfigsRepository);
        }
    }
}