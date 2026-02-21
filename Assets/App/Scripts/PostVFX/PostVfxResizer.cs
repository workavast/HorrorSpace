using App.ResolutionProviding;
using UnityEngine;

namespace App.PostVfx
{
    public class PostVfxResizer : MonoBehaviour
    {
        [SerializeField] private Camera postVfxCamera;
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private InterfaceReference<IResolutionProviderRO> resolutionProvider;

        private void Start()
        {
            resolutionProvider.Value.OnResolutionChanged += UpdatePostVfxResolution;
            UpdatePostVfxResolution();
        }

        private void OnDestroy()
        {
            resolutionProvider.Value.OnResolutionChanged -= UpdatePostVfxResolution;
        }

        private void UpdatePostVfxResolution()
        {
            renderTexture.Release();
            renderTexture.width = resolutionProvider.Value.Resolution.x;
            renderTexture.height = resolutionProvider.Value.Resolution.y;
            renderTexture.Create();
            
            postVfxCamera.Render();
        }
    }
}