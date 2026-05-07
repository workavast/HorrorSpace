using System;
using System.Collections.Generic;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    [DisallowMultipleComponent]
    public class ScreensRepository : MonoBehaviour
    {
        private readonly Dictionary<string, ScreenBase> _screens = new(StringComparer.Ordinal);
    
        public IEnumerable<ScreenBase> Screens => _screens.Values;

        public void Initialize()
        {
            _screens.Clear();

            var screens = GetComponentsInChildren<ScreenBase>(true);
            foreach (var screen in screens)
            {
                if (screen == null)
                    continue;

                if (!screen.HasScreenId)
                {
                    Debug.LogWarning($"Screen '{screen.name}' has missing or invalid ScreenId.");
                    continue;
                }

                var screenId = screen.ScreenId;
                if (_screens.ContainsKey(screenId))
                {
                    Debug.LogWarning($"Duplicate screen id '{screenId}' found for screen '{screen.name}'.");
                    continue;
                }

                _screens.Add(screenId, screen);
            }
        }

        public ScreenBase GetScreen(string screenId)
        {
            if (string.IsNullOrEmpty(screenId))
            {
                Debug.LogWarning("Error: screenId is null or empty.");
                return null;
            }

            if (!_screens.TryGetValue(screenId, out var screen))
            {
                Debug.LogWarning($"Error: invalid screen id: '{screenId}'.");
                return null;
            }

            return screen;
        }
    }
}