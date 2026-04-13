using Avastrad.Settings;
using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer;

namespace App.UI
{
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] private SettingsWindow settingsWindow;

        [Inject] private readonly SettingsRepository _settingsRepository;

        public override void Initialize()
        {
            settingsWindow.Initialize(_settingsRepository);
            settingsWindow.gameObject.SetActive(true);
        }
    }
}