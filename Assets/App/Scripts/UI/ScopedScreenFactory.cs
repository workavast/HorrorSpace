using System;
using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer.Unity;

namespace App.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UiRootScope))]
    public sealed class ScopedScreenFactory : MonoBehaviour, IUIScreenFactory
    {
        [SerializeField] private UiRootScope _rootScope;

        public ScreenBase Create(ScreenBase prefab, Transform parent)
        {
            if (_rootScope == null)
                _rootScope = GetComponent<UiRootScope>();
            if (_rootScope == null || _rootScope.Container == null)
                throw new InvalidOperationException("UI root VContainer scope is not ready");

            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                prefab.SetActive(false);
                var r = _rootScope.Container.Instantiate(prefab, parent);
                prefab.SetActive(true);
                return r;
            }
        }
    }
}
