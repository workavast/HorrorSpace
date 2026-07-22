using System;

namespace App.Usable
{
    public interface IUsable
    {
        public bool IsUsable { get; }
        
        /// <summary>
        /// call only on success use
        /// </summary>
        public event Action OnUse;
        
        /// <summary>
        /// return use result (success or not)
        /// </summary>
        public event Action<bool> OnTryUse;
        
        /// <summary>
        /// calls when <see cref="IsUsable"/> changed
        /// </summary>
        public event Action OnUsableStateChanged; 
        
        public void Interact();
    }
}
