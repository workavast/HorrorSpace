using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    public interface IUIScreenFactory
    {
        ScreenBase Create(ScreenBase prefab, Transform parent);
    }
}
