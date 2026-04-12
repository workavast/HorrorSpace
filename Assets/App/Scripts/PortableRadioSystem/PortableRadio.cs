using System;
using App.Audio;
using App.Audio.Sources;
using UnityEngine;

namespace App.PortableRadioSystem
{
	public class PortableRadio : MonoBehaviour
	{
		[SerializeField] private AudioQueuePlayer audioPlayer;
		
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