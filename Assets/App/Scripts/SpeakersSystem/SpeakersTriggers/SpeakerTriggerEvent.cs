using UnityEngine;
using VContainer;

namespace App.SpeakersSystem.SpeakersTriggers
{
    public class SpeakerTriggerEvent : MonoBehaviour
    {
        [SerializeField] private string eventName;
        
        private SpeakerEventsDatabase _database;

        public void Initialize(IObjectResolver container)
        {
            _database = container.Resolve<SpeakerEventsDatabase>();
        }

        public void PerformEvent()
        {
            if (string.IsNullOrEmpty(eventName))
                return;
            
            _database.AddEvent(eventName);
        }
    }
}