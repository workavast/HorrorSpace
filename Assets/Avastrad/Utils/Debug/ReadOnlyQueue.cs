using System;
using App.Utils.ReadOnlyAttributeDrawing;
using UnityEngine;

namespace App.Utils.Debug
{
    [Serializable]
    public class ReadOnlyQueue<T>
    {
        [SerializeField, ReadOnly] private T[] queue;

        private int _lastIndex = -1;
        
        public ReadOnlyQueue(int size)
        {
            queue = new T[size];
        }
        
        public void Enqueue(T item, bool enqueueIfNewValue = true)
        {
            if (enqueueIfNewValue && _lastIndex >= 0 && item.Equals(queue[_lastIndex]))
                return;
            
            if (_lastIndex >= queue.Length - 1)
            {
                Array.Copy(queue, 1, queue, 0, queue.Length - 1);
                queue[^1] = item;
            }
            else
            {
                _lastIndex++;
                queue[_lastIndex] = item;
            }
        }
    }
}