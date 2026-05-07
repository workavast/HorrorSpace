using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    public abstract class ScreenBase : MonoBehaviour
    {
        [SerializeField] private ScreenIdReference screenIdReference;

        public string ScreenId => screenIdReference?.Id;
        public bool HasScreenId => !string.IsNullOrEmpty(ScreenId);

        public virtual void Initialize() {}
        
        public void SetActive(bool isActive)
        {
            if (isActive)
                Show();
            else
                Hide();
        }
        
        protected virtual void Show()
            => gameObject.SetActive(true);

        protected virtual void Hide() 
            => HideInstantly();
        
        protected virtual void HideInstantly()
            => gameObject.SetActive(false);
    }
}