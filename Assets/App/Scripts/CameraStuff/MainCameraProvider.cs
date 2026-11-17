using System;
using Avastrad.VContainerExt.Installers;
using UnityEngine;

namespace App.CameraStuff
{
    public class MainCameraProvider : INonLazy
    {
        public Camera MainCamera { get; private set; }
        
        public event Action<Camera> OnMainCameraChanged;

        public MainCameraProvider(Camera mainCamera)
        {
            MainCamera = mainCamera;
        }
        
        public void SetCamera(Camera camera)
        {
            MainCamera = camera;
            OnMainCameraChanged?.Invoke(MainCamera);
        }
    }
}