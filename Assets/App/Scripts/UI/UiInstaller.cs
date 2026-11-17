using Avastrad.UI.UiSystem;
using Avastrad.VContainerExt.Installers;
using UnityEngine;
using VContainer;

namespace App.UI
{
    public class UiInstaller : Installer
    {
        [SerializeField] private UiRoot uiRoot;
        
        public override void Configure(BaseLifetimeScope scope, IContainerBuilder builder)
        {
            builder.RegisterInstance(uiRoot);
        }
    }
}