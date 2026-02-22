using App.Usable;
using UnityEngine;

namespace App.Doors
{
    public class UsabilityView : MonoBehaviour
    {
        [SerializeField] private GameObject isUsableGo;
        [SerializeField] private GameObject isUnUsableGo;
        [SerializeField] private InterfaceReference<IUsable> doorBtn;

        private void Awake()
        {
            doorBtn.Value.OnUsableStateChanged += UpdateView;
            UpdateView();
        }

        private void OnDestroy()
        {
            doorBtn.Value.OnUsableStateChanged -= UpdateView;
        }

        private void UpdateView()
        {
            isUsableGo.SetActive(doorBtn.Value.IsUsable);
            isUnUsableGo.SetActive(!doorBtn.Value.IsUsable);
        }
    }
}