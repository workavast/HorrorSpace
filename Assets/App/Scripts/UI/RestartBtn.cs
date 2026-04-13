using System;
using Avastrad.ScenesLoading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace App.UI
{
    [RequireComponent(typeof(Button))]
    public class RestartBtn : MonoBehaviour
    {
        [SerializeField] private LoadingConfig loadingConfig;
        
        [Inject] private ISceneLoader _sceneLoader;
        
        private void Awake() 
            => GetComponent<Button>().onClick.AddListener(() => _sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex, loadingConfig.ShowDuration));
    }
}