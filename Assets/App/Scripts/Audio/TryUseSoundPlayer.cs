using App.Audio.Sources;
using App.Usable;
using UnityEngine;
using VContainer;

namespace App.Audio
{
    public class TryUseSoundPlayer : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IUsable> usable;
        [SerializeField] private AudioPlayer[] successAudios;
        [SerializeField] private AudioPlayer[] failAudios;
        
        [Inject]
        public void Construct(AudioFactory audioFactory)
        {
            foreach (var audioPlayer in successAudios)
                audioPlayer.Construct(audioFactory);
            
            foreach (var audioPlayer in failAudios)
                audioPlayer.Construct(audioFactory);
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