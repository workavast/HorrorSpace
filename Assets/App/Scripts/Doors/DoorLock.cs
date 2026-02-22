using App.KeySystem;
using UnityEngine;

namespace App.Doors
{
    public class DoorLock : MonoBehaviour
    {
        [SerializeField] private DoorController door;
        [SerializeField] private LockProvider lockProvider;

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
            door.SetUnLockState(true);
            lockProvider.SetActivityState(false);
        }
        
        private void InvalidKey() 
            => Debug.Log("Invalid key");
    }
}