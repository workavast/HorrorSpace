using App.KeySystem;
using UnityEngine;

namespace App.Doors
{
    public class DoorLock : MonoBehaviour
    {
        [SerializeField] private DoorButton doorBtn;
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
            doorBtn.SetState(true);
            lockProvider.SetActivityState(false);
        }
        
        private void InvalidKey() 
            => Debug.Log("Invalid key");
    }
}