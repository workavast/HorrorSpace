using System;

namespace App.InteractiveObjects
{
    public class InteractionContext
    {
        public IInteractiveObj InteractiveObj {get; private set; }

        public event Action<IInteractiveObj> OnInteractiveObjChanged;
        
        public void SetInteractiveObj(IInteractiveObj interactiveObj)
        {
            InteractiveObj = interactiveObj;
            OnInteractiveObjChanged?.Invoke(InteractiveObj);
        }
    }
}