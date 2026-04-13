using System;
using App.Health;
using App.UI;
using Avastrad.ScenesLoading;
using Avastrad.UI.UiSystem;
using Avastrad.Utils.ScenesReferencing;
using UnityEngine;
using VContainer;

namespace App
{
    public class GameOverDetector : MonoBehaviour
    {
        [SerializeField] private HealthModel playerHealth;
        [SerializeField] private SceneReference sceneRef;
        [SerializeField] private LoadingConfig loadingConfig;
        
        [Inject] private ScreensController _screensController;

        private bool _triggered;

        private void FixedUpdate()
        {
            if (!playerHealth.IsAlive && !_triggered)
            {
                _triggered = true;
                _screensController.SetScreen<GameEndedScreen>();
            }
        }
    }
}