using Avastrad.Settings;
using UnityEngine;

namespace App
{
    public class SettingsAutoApplyer : MonoBehaviour
    {
        [SerializeField] private SettingsWindow settingsWindow;
        
        private void Update()
        {
            if (settingsWindow.HasChangedAny())
            {
                settingsWindow.ApplySettings();
                settingsWindow.HasChangedAny();
            }
        }
    }
}