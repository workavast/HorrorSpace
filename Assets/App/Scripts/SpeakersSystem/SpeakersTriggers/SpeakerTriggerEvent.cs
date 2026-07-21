using UnityEngine;

namespace App.SpeakersSystem.SpeakersTriggers
{
    public class SpeakerTriggerEvent : MonoBehaviour
    {
        [SerializeField] private string eventName;
        
        private SpeakerEventsDatabase _database;

        public void Construct(SpeakerEventsDatabase database)
        {
            _database = database;
        }

        public void PerformEvent()
        {
            if (string.IsNullOrEmpty(eventName))
                return;
            
            _database.AddEvent(eventName);
        }
    }
}