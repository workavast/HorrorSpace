using System;
using System.Threading;
using Avastrad.UI.UiSystem.Ids;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    public sealed class AutoScreensLoader : MonoBehaviour
    {
        [SerializeField] private bool _autoRun = true;
        [SerializeField] private UiRoot _uiRoot;
        [SerializeField] private ScreenIdReference[] _screens = Array.Empty<ScreenIdReference>();

        private CancellationTokenSource _lifetimeCts;

        private void Awake()
            => _lifetimeCts = new CancellationTokenSource();

        private void Start()
        {
            if (_autoRun) 
                RunAsync().Forget(Debug.LogException);
        }

        public UniTask RunAsync(CancellationToken ct = default)
        {
            if (_uiRoot == null)
                throw new InvalidOperationException($"{nameof(AutoScreensLoader)} has no UiRoot");
            
            return _uiRoot.AddAsync(_screens, ct: ct == CancellationToken.None ? _lifetimeCts.Token : ct);
        }

        private void OnDestroy()
        {
            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
        }
    }
}
