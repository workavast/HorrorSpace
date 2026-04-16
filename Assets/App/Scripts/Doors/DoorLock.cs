using App.KeySystem;
using UnityEngine;

namespace App.Doors
{
    public class DoorLock : MonoBehaviour
    {
        [SerializeField] private DoorController door;
        [SerializeField] private KeyReader keyReader;

        private void Awake() 
            => Initialize();

        private void Initialize()
        {
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
            door.SetLockState(false);
            keyReader.SetActivityState(false);
        }
        
        private void InvalidKey() 
            => Debug.Log("Invalid key");
    }
}