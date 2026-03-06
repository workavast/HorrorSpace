using App.Audio;
using App.Audio.Sources;
using UnityEngine;

namespace App.Usable
{
    public class TryUseSoundPlayer : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IUsable> usable;
        [SerializeField] private AudioPlayer[] successAudios;
        [SerializeField] private AudioPlayer[] failAudios;
        
        public void Initialize(AudioFactory audioFactory)
        {
            foreach (var audioPlayer in successAudios)
                audioPlayer.Initialize(audioFactory);
            
            foreach (var audioPlayer in failAudios)
                audioPlayer.Initialize(audioFactory);
        }
        
        private void Awake()
        {
            usable.Value.OnTryUse += PlaySound;
        }

        private void OnDestroy()
        {
            usable.Value.OnTryUse -= PlaySound;
        }

        private void PlaySound(bool useSuccessful)
        {
            if (useSuccessful)
                PlaySounds(successAudios);
            else
                PlaySounds(failAudios);
        }

        private static void PlaySounds(AudioPlayer[] sounds)
        {
            foreach (var sound in sounds)
                sound.Play();
        }
    }
}