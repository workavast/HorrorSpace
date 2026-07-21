using System;
using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using UnityEngine;

namespace App.Localization
{
    [Serializable]
    public class StringTablesPreloadBootstrap : Bootstrap
    {
        [SerializeField] private StringTablesPreloader preloader;
        
        protected override Task SelfInitialization(CancellationToken cancellationToken)
        {
            preloader.Initialize();
            return preloader.Preload();
        }

        protected override void OnDestroyCallback()
        {
            preloader.Release();
            preloader.Dispose();
        }
    }
}