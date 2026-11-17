using System;

namespace App.InteractiveObjects
{
    public class InteractionContext
    {
        public bool IsGrab {get; private set; }
        public IInteractiveObj InteractiveObj {get; private set; }

        public event Action<bool> OnGrabStateChanged; 
        public event Action<IInteractiveObj> OnInteractiveObjChanged;

        public void SetGrabState(bool isGrab)
        {
            IsGrab = isGrab;
            OnGrabStateChanged?.Invoke(IsGrab);
        }
        
        public void SetInteractiveObj(IInteractiveObj interactiveObj)
        {
            InteractiveObj = interactiveObj;
            OnInteractiveObjChanged?.Invoke(InteractiveObj);
        }
    }
}