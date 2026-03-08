using App.Audio;
using App.Audio.Sources;
using App.KeySystem;
using UnityEngine;

namespace App.Lockers
{
	public class LockerLock : MonoBehaviour
	{
		[SerializeField] private Locker locker;
		[SerializeField] private LockProvider lockProvider;
		[SerializeField] private AudioPlayer audioPlayer;
		
		public void Initialize(AudioFactory audioFactory)
		{
			audioPlayer.Initialize(audioFactory);
			
			lockProvider.OnUnlocked += Unlock;
			lockProvider.OnInvalidKey += InvalidKey;
		}
        
		private void OnDestroy()
		{
			lockProvider.OnUnlocked -= Unlock;
			lockProvider.OnInvalidKey -= InvalidKey;
		}

		private void Unlock()
		{
			audioPlayer.Play();
			locker.SetUnLockState(true);
			lockProvider.SetActivityState(false);
		}
        
		private void InvalidKey() 
			=> Debug.Log("Invalid key");
	}
}