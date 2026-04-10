using App.Health;
using Avastrad.ScenesLoading;
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
        
        [Inject] private ISceneLoader _sceneLoader;

        private bool _triggered;
        
        private void FixedUpdate()
        {
            if (!playerHealth.IsAlive && !_triggered)
            {
                _triggered = true;
                _sceneLoader.LoadScene(sceneRef.SceneIndex, loadingConfig.ShowDuration);
            }
        }
    }
}