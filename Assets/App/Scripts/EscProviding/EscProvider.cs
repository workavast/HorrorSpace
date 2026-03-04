using System.Collections.Generic;
using Avastrad.Libs.CheckOnNullLib;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace App.EscProviding
{
    public class EscProvider
    {
        private readonly List<IEscListener> _listeners = new();
        
        public void Sub(IEscListener listener)
        {
            if (_listeners.Contains(listener))
            {
                Debug.LogError("Duplicate subscription");
                return;
            }
            
            _listeners.Add(listener);
        }
        
        public void UnSub(IEscListener listener) 
            => _listeners.Remove(listener);

        public void OnEscPressed()
        {
            ClearNulls();
            if (_listeners.Count > 0)
                _listeners[^1].OnEscPressed();
        }

        private void ClearNulls()
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i].IsAnyNull())
                {
                    _listeners.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}