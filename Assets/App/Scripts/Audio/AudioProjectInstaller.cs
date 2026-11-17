using App.Audio.Settings;
using Avastrad.Settings.Volume;
using Avastrad.VContainerExt.Installers;
using UnityEngine;
using VContainer;

namespace App.Audio
{
    public class AudioProjectInstaller : Installer
    {
        [SerializeField] private VolumeSettingConfig volumeSettingConfig;
        
        public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<SettingsAudioApplier>(Lifetime.Singleton).WithParameter(volumeSettingConfig).NonLazySelf();
        }
    }
}