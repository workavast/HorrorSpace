using System;
using System.Collections.Generic;

namespace App.Initialization
{
    public class LateInitializer : ILateInitializer, IDisposable
    {
        public bool Initialized {get; private set; }
        
        private readonly List<Action> _subscribers;

        public LateInitializer(int initialCapacity = 8)
        {
            _subscribers = new List<Action>(initialCapacity);
        }
        
        public void Dispose()
        {
            _subscribers.Clear();
        }
        
        public void Initialize()
        {
            Initialized = true;
            foreach (var subscriber in _subscribers)
                subscriber();
            
            _subscribers.Clear();
        }

        public void Register(Action initializeMethod)
        {
            if (Initialized)
            {
                initializeMethod();
                return;
            }
            
            _subscribers.Add(initializeMethod);
        }
    }
}