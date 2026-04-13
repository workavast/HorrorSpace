using System;
using App.Audio;
using App.Audio.Sources;
using App.SpeakersSystem;
using UnityEngine;

namespace App.PortableRadioSystem
{
	public class PortableRadio : MonoBehaviour, ISpeaker
	{
		[SerializeField] private SpeakerConfig speakerConfig;
		[SerializeField] private AudioQueuePlayer audioPlayer;
		
		public string Id => speakerConfig.Id;
		
		public event Action OnAllAudiosPlayed;

		private void Awake()
		{
			audioPlayer.OnAllAudiosPlayed += CallOnAllAudiosPlayed;
		}

		private void OnDestroy()
		{
			audioPlayer.OnAllAudiosPlayed -= CallOnAllAudiosPlayed;
		}
		
		public void Play(AudioSourceHolder audioSourcePrefab) 
			=> audioPlayer.Play(audioSourcePrefab);

		private void CallOnAllAudiosPlayed() 
			=> OnAllAudiosPlayed?.Invoke();
	}
}