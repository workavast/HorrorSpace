using Avastrad.ScenesLoading;
using Avastrad.Utils.ScenesReferencing;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.UI
{
    [RequireComponent(typeof(Button))]
    public class BtnLoadScene : MonoBehaviour
    {
        [SerializeField] private SceneReference sceneRef;
        [SerializeField] private LoadingConfig loadingConfig;
        
        [Inject] private readonly ISceneLoader _sceneLoader;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(()=>_sceneLoader.LoadScene(sceneRef.SceneIndex, loadingConfig.ShowDuration));
        }
    }
}