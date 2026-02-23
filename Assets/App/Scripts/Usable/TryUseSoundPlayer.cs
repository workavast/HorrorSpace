using System;
using UnityEngine;

namespace App.Usable
{
    public class TryUseSoundPlayer : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IUsable> usable;
        [SerializeField] private AudioSource[] successSound;
        [SerializeField] private AudioSource[] failSound;
        
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
                PlaySounds(successSound);
            else
                PlaySounds(failSound);
        }

        private static void PlaySounds(AudioSource[] sounds)
        {
            foreach (var sound in sounds)
            {
                if (sound == null || sound.clip == null || sound.isPlaying) 
                    return;
                
                sound.Play();
            }
        }
    }
}