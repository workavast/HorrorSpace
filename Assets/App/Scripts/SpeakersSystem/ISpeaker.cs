using System;
using App.Audio.Sources;

namespace App.SpeakersSystem
{
    public interface ISpeaker
    {
        public string Id { get; }
        
        public event Action OnAllAudiosPlayed;

        public void Play(AudioSourceHolder audioSourceHolder);
    }
}