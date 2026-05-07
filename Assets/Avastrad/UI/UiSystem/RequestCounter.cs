using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    internal class RequestCounter
    {
        public bool RequestMoreZero => RequestsCount > 0;
        public int RequestsCount { get; private set; }
        
        public void ChangeRequests(bool addRequest)
        {
            if (addRequest)
                AddRequest();
            else
                RemoveRequest();
        }

        public void AddRequest()
        {
            RequestsCount++;
        }

        public void RemoveRequest()
        {
            RequestsCount--;
            if (RequestsCount < 0)
            {
                RequestsCount = 0;
                Debug.LogWarning("You try reduce requests count when it equal 0");
                return;
            }
        }
    }
}