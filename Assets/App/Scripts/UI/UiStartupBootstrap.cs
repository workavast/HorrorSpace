using System;
using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using Avastrad.UI.UiSystem;
using Avastrad.UI.UiSystem.Ids;
using UnityEngine;

namespace App.UI
{
    public sealed class UiStartupBootstrap : Bootstrap
    {
        [SerializeField] private ScreenIdReference _initialScreenId;

        protected override async Task SelfInitialization(CancellationToken cancellationToken)
        {
            var uiScope = FindAnyObjectByType<UiRootScope>();
            if (uiScope == null)
                throw new InvalidOperationException("No UiScope found in scene");

            if (uiScope.Container == null)
                uiScope.Build();

            var uiRoot = uiScope.GetComponent<UiRoot>();
            await uiRoot.PreloadCatalogAsync(createHidden: true, ct: cancellationToken);
            if (!string.IsNullOrEmpty(_initialScreenId))
                await uiRoot.SetAsync(_initialScreenId, ct: cancellationToken);
        }
    }
}
