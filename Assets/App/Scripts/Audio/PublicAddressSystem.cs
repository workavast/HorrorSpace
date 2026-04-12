using System.Collections.Generic;
using App.Audio.Sources;
using UnityEngine;

namespace App.Audio
{
    public class PublicAddressSystem : MonoBehaviour
    {
        [SerializeField] private List<AudioQueuePlayer> audioPlayers;

        public void Play(AudioSourceHolder audioPrefab)
        {
            foreach (var audioPlayer in audioPlayers)
            {
                if (!audioPlayer.Contains(audioPrefab)) 
                    audioPlayer.Play(audioPrefab);
            }
        }
    }
}