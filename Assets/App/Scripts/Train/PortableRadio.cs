using System;
using System.Collections.Generic;
using App.Audio.Sources;
using App.LifetimeScopes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App
{
	public class PortableRadio : INonLazy
	{
		private readonly Queue<AudioSourceHolder> _audioSources = new();
		private AudioSourceHolder _activeAudioSource;

		public event Action OnAllAudiosPlayed;

		public void Play(AudioSourceHolder audioSourcePrefab)
		{
			if (_activeAudioSource == null)
			{
				_activeAudioSource = Object.Instantiate(audioSourcePrefab);
				_activeAudioSource.OnPlayOver += TryPlayNext;
			}
			else
				_audioSources.Enqueue(audioSourcePrefab);
		}

		private void TryPlayNext()
		{
			if (_activeAudioSource != null)
			{
				_activeAudioSource.OnPlayOver -= TryPlayNext;
				Object.Destroy(_activeAudioSource.gameObject);
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