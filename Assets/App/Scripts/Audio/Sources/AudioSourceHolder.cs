using System;
using UnityEngine;

namespace App.Audio.Sources
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceHolder : MonoBehaviour
    {
        protected AudioSource _audioSource;
        private bool _isPlayingAtALstFrame;
        
        public event Action OnPlayOver;
        
        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_isPlayingAtALstFrame && !_audioSource.isPlaying)
            {
                _isPlayingAtALstFrame = false;
                OnPlayOver?.Invoke();
            }
        }

        protected void Play()
        {
            _audioSource.Stop();
            _audioSource.Play();
            _isPlayingAtALstFrame = true;
        }

        public void SetPitch(float newPitch) 
            => _audioSource.pitch = newPitch;

        public void SetClip(AudioClip audioClip, bool play = true)
        {
            _audioSource.clip = audioClip;

            if (play)
                Play();
        }
    }
}