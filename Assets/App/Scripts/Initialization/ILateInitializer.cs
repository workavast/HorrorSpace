using System;

namespace App.Initialization
{
    public interface ILateInitializer
    {
        public bool Initialized {get; }
        public void Register(Action initializeMethod);
    }
}