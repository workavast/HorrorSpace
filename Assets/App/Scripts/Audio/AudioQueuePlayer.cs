using System;
using System.Collections.Generic;
using App.Audio.Sources;
using UnityEngine;

namespace App.Audio
{
	public class AudioQueuePlayer : MonoBehaviour
	{
		[SerializeField] private Transform playPoint;
		
		private readonly Queue<AudioSourceHolder> _audioSources = new();
		private AudioSourceHolder _activeAudioSource;

		public event Action OnAllAudiosPlayed;

		public void Play(AudioSourceHolder audioSourcePrefab)
		{
			if (_activeAudioSource == null)
			{
				_activeAudioSource = Instantiate(audioSourcePrefab, playPoint);
				_activeAudioSource.OnPlayOver += TryPlayNext;
			}
			else
				_audioSources.Enqueue(audioSourcePrefab);
		}

		public bool Contains(AudioSourceHolder audioSourcePrefab) 
			=> _audioSources.Contains(audioSourcePrefab);

		private void TryPlayNext()
		{
			if (_activeAudioSource != null)
			{
				_activeAudioSource.OnPlayOver -= TryPlayNext;
				Destroy(_activeAudioSource.gameObject);
				_activeAudioSource = null;
			}

			if (_audioSources.Count == 0)
			{
				OnAllAudiosPlayed?.Invoke();
				return;
			}
			
			var nextAudioSourcePrefab = _audioSources.Dequeue();
			if (nextAudioSourcePrefab != null)
				Play(nextAudioSourcePrefab);
		}
	}
}