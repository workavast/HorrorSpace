using Avastrad.UI.UiSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.UI
{
    [RequireComponent(typeof(Button))]
    public class RevertScreenBtn : MonoBehaviour
    {
        [Inject] private UiRoot _uiRoot;

        private void OnEnable()
            => GetComponent<Button>().onClick.AddListener(Revert);

        private void OnDisable()
            => GetComponent<Button>().onClick.RemoveListener(Revert);

        private void Revert()
            => _uiRoot.Undo();
    }
}