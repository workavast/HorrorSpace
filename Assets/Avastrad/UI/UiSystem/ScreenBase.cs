using System;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    public abstract class ScreenBase : MonoBehaviour
    {
        private string _screenId;

        public string ScreenId => _screenId;
        public bool HasScreenId => !string.IsNullOrEmpty(ScreenId);
        protected bool IsRequestedVisible { get; private set; }

        public event Action OnEnabled;
        public event Action OnDisabled;
        
        public virtual void Initialize() {}

        internal void AssignScreenId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Screen ID is empty", nameof(id));

            if (HasScreenId && !string.Equals(_screenId, id, StringComparison.Ordinal))
                return;
            
            _screenId = id;
        }
        
        protected virtual void OnEnable()
        {
            OnEnabled?.Invoke();
        }

        protected virtual void OnDisable()
        {
            OnDisabled?.Invoke();
        }
        
        public void SetActive(bool isActive)
        {
            IsRequestedVisible = isActive;
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

        internal void PrepareHidden()
        {
            IsRequestedVisible = false;
            HideInstantly();
        }
    }
}
