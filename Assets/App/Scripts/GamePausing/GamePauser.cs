using Avastrad.Utils;
using UnityEngine;

namespace App.GamePausing
{
    public class GamePauser
    {
        private readonly RequestCounter _requestCounter;

        public GamePauser()
        {
            _requestCounter = new RequestCounter(UpdateState);
            UpdateState(_requestCounter.RequestMoreZero);
        }

        public void AddRequest() 
            => _requestCounter.AddRequest();

        public void RemoveRequest() 
            => _requestCounter.RemoveRequest();

        private static void UpdateState(bool isActive)
        {
            if(isActive)
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
            }
            else
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
            }
        }
    }
}