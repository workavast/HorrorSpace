using System;

namespace App.Usable
{
    public interface IUsable
    {
        public bool IsUsable { get; }
        
        public event Action OnUse; 
        public event Action<bool> OnTryUse; 
        public event Action OnUsableStateChanged; 
        
        public void Interact();
    }
}
