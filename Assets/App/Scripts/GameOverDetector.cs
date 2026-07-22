using App.Health;
using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer;

namespace App
{
    public class GameOverDetector : MonoBehaviour
    {
		[SerializeField] private ScreenIdReference screenId;
        [SerializeField] private HealthModel playerHealth;
        
        [Inject] private ScreensController _screensController;

        private bool _triggered;

        private void Awake()
        {
            playerHealth.OnHealthChanged += CheckHealthStatus;
        }

        private void OnDestroy()
        {
            playerHealth.OnHealthChanged -= CheckHealthStatus;
        }

        private void CheckHealthStatus()
        {
            if (!playerHealth.IsAlive && !_triggered)
            {
                _triggered = true;
                _screensController.SetScreen(screenId);
            }
        }
    }
}