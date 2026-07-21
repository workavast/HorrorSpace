using App.LifetimeScopes;
using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer;

namespace App.UI
{
    public class UiInstaller : Installer
    {
        [SerializeField] private ScreensController screensController;
        
        public override void Configure(CoreLifetimeScope scope, IContainerBuilder builder)
        {
            builder.RegisterInstance(screensController);
        }
    }
}