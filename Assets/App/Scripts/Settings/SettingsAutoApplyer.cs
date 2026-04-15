using System;
using Avastrad.Settings;
using Avastrad.Settings.Localization;
using UnityEngine;
using VContainer;

namespace App
{
    public class SettingsAutoApplyer : MonoBehaviour
    {
        [SerializeField] private LocalizationSettingViewModel viewModel;
        [SerializeField] private SettingsWindow settingsWindow;
        
        public void Initialize()
        {
            viewModel.OnChanged += Apply;
        }

        private void OnDestroy()
        {
            viewModel.OnChanged -= Apply;
        }

        private void Apply()
        {
            settingsWindow.ApplySettings();
        }
    }
}