using Avastrad.Settings;
using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer;

namespace App.UI
{
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] private SettingsWindow settingsWindow;
        [SerializeField] private SettingsAutoApplyer settingsAutoApplyer;

        [Inject] private readonly SettingsRepository _settingsRepository;

        public override void Initialize()
        {
            settingsWindow.Initialize(_settingsRepository);
            settingsAutoApplyer.Initialize();
            settingsWindow.gameObject.SetActive(true);
        }
    }
}