using Avastrad.VContainerExt.Installers;
using UnityEngine;
using VContainer;

namespace App.CameraStuff
{
    public class MainCameraInstaller : Installer
    {
        [SerializeField] private Camera _mainCamera;
        
        public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
        {
            builder.Register<MainCameraProvider>(Lifetime.Singleton).WithParameter(_mainCamera);
        }
    }
}