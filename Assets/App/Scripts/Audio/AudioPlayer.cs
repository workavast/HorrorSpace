using System;
using App.Audio.Sources;
using UnityEngine;

namespace App.Audio
{
	[Serializable]
	public class AudioPlayer
	{
		[SerializeField] private AudioSourceHolderPoolable audioPrefab;
		[SerializeField] private Transform point;
		
		private AudioSourceHolderPoolable _lastAudio;
		private AudioFactory _audioFactory;
		
		public void Initialize(AudioFactory audioFactory)
		{
			_audioFactory = audioFactory;
		}
		
		public void Play()
		{
			if (_lastAudio != null)
				return;
			
			_lastAudio = _audioFactory.Create(audioPrefab, point.position);
			_lastAudio.OnPlayOver += Clear;
		}

		public void Clear()
		{
			_lastAudio.OnPlayOver -= Clear;
			_lastAudio = null;
		}
	}
}