using App.KeySystem;
using UnityEngine;

namespace App.Lockers
{
	public class LockerLock : MonoBehaviour
	{
		[SerializeField] private LockerController locker;
		[SerializeField] private LockProvider lockProvider;
		[SerializeField] private AudioSource unlockSound;
		
		private void Awake() 
			=> Initialize();

		private void Initialize()
		{
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
			unlockSound.Play();
			locker.SetUnLockState(true);
			lockProvider.SetActivityState(false);
		}
        
		private void InvalidKey() 
			=> Debug.Log("Invalid key");
	}
}