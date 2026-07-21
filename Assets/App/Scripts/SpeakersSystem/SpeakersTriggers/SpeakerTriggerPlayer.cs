using System.Collections.Generic;
using App.Audio.Sources;
using UnityEngine;

namespace App.SpeakersSystem.SpeakersTriggers
{
    public class SpeakerTriggerPlayer : MonoBehaviour
    {
        [SerializeField] private SpeakerConfig speakerConfig;
        [SerializeField] private AudioSourceHolder audioSource;
        [SerializeField] private string eventNameToAdd;
        [SerializeField] private List<EventsToPlayTuple> eventsToPlay;
        
        private MonologSpeakers _monologSpeakers;
        private SpeakerEventsDatabase _database;

        public void Construct(SpeakerEventsDatabase database, MonologSpeakers monologSpeakers)
        {
            _monologSpeakers = monologSpeakers;
            _database = database;
        }

        public bool TryPlayAudio()
        {
            if (eventsToPlay == null || eventsToPlay.Count == 0)
            {
                _monologSpeakers.Play(speakerConfig, audioSource);
                _database.AddEvent(eventNameToAdd);
                return true;
            }
            
            if (_database.ContainsAll(eventsToPlay))
            {
                _monologSpeakers.Play(speakerConfig, audioSource);
                _database.AddEvent(eventNameToAdd);
                return true;
            }
            
            return false;
        }
    }
}