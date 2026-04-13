using System.Collections.Generic;
using App.Audio.Sources;
using UnityEngine;

namespace App.SpeakersSystem
{
    public class MonologSpeakers : MonoBehaviour
    {
        [SerializeField] private List<InterfaceReference<ISpeaker>> speakers;
        
        private readonly Dictionary<string, InterfaceReference<ISpeaker>> _speakersById = new();

        public void Awake()
        {
            foreach (var speaker in speakers) 
                _speakersById.Add(speaker.Value.Id, speaker);
        }

        public void Play(SpeakerConfig speakerConfig, AudioSourceHolder audioSourceHolder) 
            => Play(speakerConfig.Id, audioSourceHolder);

        public void Play(string id, AudioSourceHolder audioSourceHolder)
        {
            if (_speakersById.TryGetValue(id, out var speaker)) 
                speaker.Value.Play(audioSourceHolder);
        }

        public ISpeaker GetSpeaker(string id)
        {
            if (_speakersById.TryGetValue(id, out var speaker))
                return speaker.Value;
            
            return null;
        }
    }
}