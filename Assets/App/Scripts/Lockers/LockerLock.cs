using App.Audio;
using App.Audio.Sources;
using App.KeySystem;
using UnityEngine;

namespace App.Lockers
{
	public class LockerLock : MonoBehaviour
	{
		[SerializeField] private Locker locker;
		[SerializeField] private KeyReader keyReader;
		[SerializeField] private AudioPlayer audioPlayer;
		
		public void Initialize(AudioFactory audioFactory)
		{
			audioPlayer.Initialize(audioFactory);
			
			keyReader.OnValidKey += Unlock;
			keyReader.OnInvalidKey += InvalidKey;
		}
        
		private void OnDestroy()
		{
			keyReader.OnValidKey -= Unlock;
			keyReader.OnInvalidKey -= InvalidKey;
		}

		private void Unlock()
		{
			audioPlayer.Play();
			locker.SetUnLockState(true);
			keyReader.SetActivityState(false);
		}
        
		private void InvalidKey() 
			=> Debug.Log("Invalid key");
	}
}