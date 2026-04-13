using System.Collections.Generic;
using UnityEngine;

namespace App.SpeakersSystem.SpeakersTriggers
{
    public class SpeakerEventsDatabase
    {
        private readonly List<string> _events = new();

        public void AddEvent(string eventName)
        {
            if (Contains(eventName))
            {
                Debug.LogWarning("Duplicate event: " + eventName);
                return;
            }
            
            _events.Add(eventName);
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public bool Contains(string eventName) 
            => _events.Contains(eventName);

        public bool ContainsAll(IReadOnlyList<EventsToPlayTuple> events)
        {
            foreach (var eventTuple in events)
                if (eventTuple.State != Contains(eventTuple.EventName))
                    return false;
            
            return true;
        }
    }
}