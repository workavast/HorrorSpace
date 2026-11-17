using App.Health;
using Avastrad.UI.UiSystem;
using Avastrad.UI.UiSystem.Ids;
using UnityEngine;
using VContainer;

namespace App
{
    public class GameOverDetector : MonoBehaviour
    {
		[SerializeField] private ScreenIdReference screenId;
        [SerializeField] private HealthModel playerHealth;
        
        [Inject] private UiRoot _uiRoot;

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
                _uiRoot.Set(screenId);
            }
        }
    }
}