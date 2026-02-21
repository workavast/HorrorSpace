using UnityEngine;

namespace App.KeySystem
{
    public class KeyObject : MonoBehaviour, IKeyObject
    {
        [SerializeField] private KeyConfig config;

        public string GetKey() => config.Key;
        public KeyType GetKeyType() => config.KeyType;
    }
}