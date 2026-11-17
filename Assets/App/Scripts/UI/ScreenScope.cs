using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScreenBase))]
    public sealed class ScreenScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
            => builder.RegisterInstance(GetComponent<ScreenBase>());
    }
}
