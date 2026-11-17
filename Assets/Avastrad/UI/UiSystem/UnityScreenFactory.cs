using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    public sealed class UnityScreenFactory : IUIScreenFactory
    {
        public ScreenBase Create(ScreenBase prefab, Transform parent)
            => Object.Instantiate(prefab, parent, false);
    }
}
