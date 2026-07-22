using App.Audio;
using App.Audio.Sources;
using App.KeySystem;
using UnityEngine;
using VContainer;

namespace App.Lockers
{
	public class LockerLock : MonoBehaviour
	{
		[SerializeField] private Locker locker;
		[SerializeField] private KeyReader keyReader;
		[SerializeField] private AudioPlayer audioPlayer;
		
		[Inject]
		public void Construct(AudioFactory audioFactory)
		{
			audioPlayer.Construct(audioFactory);
		}

		private void Start()
		{
			keyReader.OnValidKey += Unlock;
			keyReader.OnInvalidKey += InvalidKey;
		}

		private void OnDestroy()
		{
			keyReader.OnValidKey -= Unlock;
			keyReader.OnInvalidKey -= InvalidKey;
		}

		private void Unlock(IKeyObject _)
		{
			audioPlayer.Play();
			locker.SetUnLockState(true);
			keyReader.SetActivityState(false);
		}
        
		private void InvalidKey(IKeyObject _) 
			=> Debug.Log("Invalid key");
	}
}