using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UiRoot))]
    public sealed class UiRootScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
            => builder.RegisterInstance(GetComponent<UiRoot>());
    }
}
