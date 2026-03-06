using App.Audio.Settings;
using App.LifetimeScopes;
using Avastrad.Settings.Volume;
using UnityEngine;
using VContainer;

namespace App.Audio
{
    public class AudioProjectScopeConfig : ScopeConfig
    {
        [SerializeField] private VolumeSettingConfig volumeSettingConfig;
        
        public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<SettingsAudioApplier>(Lifetime.Singleton).WithParameter(volumeSettingConfig).NonLazySelf();
        }
    }
}