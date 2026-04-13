using System;
using System.Collections.Generic;
using App.Audio.Sources;
using App.SpeakersSystem;
using UnityEngine;

namespace App.Audio
{
    public class PublicAddressSystem : MonoBehaviour, ISpeaker
    {
        [SerializeField] private SpeakerConfig speakerConfig;
        [SerializeField] private List<AudioQueuePlayer> audioPlayers;

        public string Id => speakerConfig.Id;
        public event Action OnAllAudiosPlayed;

        private void Awake()
        {
            foreach (var audioPlayer in audioPlayers)
                audioPlayer.OnAllAudiosPlayed += CheckAudioPlayers;
        }

        private void OnDestroy()
        {
            foreach (var audioPlayer in audioPlayers)
                audioPlayer.OnAllAudiosPlayed -= CheckAudioPlayers;
        }

        public void Play(AudioSourceHolder audioPrefab)
        {
            foreach (var audioPlayer in audioPlayers)
            {
                if (!audioPlayer.Contains(audioPrefab)) 
                    audioPlayer.Play(audioPrefab);
            }
        }
        
        private void CheckAudioPlayers()
        {
            foreach (var audioPlayer in audioPlayers)
                if (audioPlayer.IsPlay())
                    return;
            
            OnAllAudiosPlayed?.Invoke();
        }
    }
}