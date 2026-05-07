using System;
using System.Collections.Generic;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    [DisallowMultipleComponent]
    internal class ScreensControllerInternal : MonoBehaviour
    {
        private ScreensRepository _screenRepository;

        public void Initialize()
        {
            _screenRepository = GetComponentInChildren<ScreensRepository>();
            _screenRepository.Initialize();

            foreach (var screen in _screenRepository.Screens)
                screen.Initialize();
        }
        
        public IReadOnlyList<string> GetActiveScreens()
        {
            var screens = new List<string>();

            foreach (var screen in _screenRepository.Screens)
                if (screen.gameObject.activeSelf)
                    screens.Add(screen.ScreenId);
            
            return screens;
        }

        public ScreenBase GetScreen(string screenId) 
            => _screenRepository.GetScreen(screenId);

        public void SetScreen(string screenId)
        {
            var newScreen = GetScreen(screenId);
            if (newScreen == null)
                return;

            foreach (var screen in _screenRepository.Screens)
                if (screen.gameObject.activeSelf)
                    screen.SetActive(false);

            newScreen.SetActive(true);
        }
        
        public void SetScreens(IReadOnlyList<string> screenIds)
        {
            foreach (var screen in _screenRepository.Screens)
                if (screen.isActiveAndEnabled && !Contains(screenIds, screen.ScreenId))
                    TryToggleScreen(screen, false);

            foreach (var screenId in screenIds)
                ToggleScreen(screenId, true);
        }
        
        public void ToggleScreen(string screenId, ToggleType toggleType)
        {
            switch (toggleType)
            {
                case ToggleType.Auto:
                    ToggleScreen(screenId);
                    break;
                case ToggleType.Show:
                    ToggleScreen(screenId, true);
                    break;
                case ToggleType.Hide:
                    ToggleScreen(screenId, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toggleType), toggleType, null);
            }
        }

        private void ToggleScreen(string screenId)
        {
            var screen = _screenRepository.GetScreen(screenId);
            if (screen == null)
                return;

            TryToggleScreen(screen, !screen.isActiveAndEnabled);
        }

        private void ToggleScreen(string screenId, bool show)
        {
            var screen = _screenRepository.GetScreen(screenId);
            if (screen == null)
                return;

            TryToggleScreen(screen, show);
        }

        private static void TryToggleScreen(ScreenBase screen, bool show)
        {
            if (screen.isActiveAndEnabled == show)
                return;

            screen.SetActive(show);
        }
        
        private static bool Contains(IReadOnlyList<string> list, string value) 
        {
            for (var i = 0; i < list.Count; i++)
                if (list[i] == value)
                    return true;
            
            return false;
        }
    }
}