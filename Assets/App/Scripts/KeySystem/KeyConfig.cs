using UnityEngine;

namespace App.KeySystem
{
    [CreateAssetMenu(fileName = nameof(KeyConfig), menuName = KeySystemConsts.KeySystemConfigsRoot + nameof(KeyConfig))]
    public class KeyConfig : ScriptableObject
    {
        [SerializeField] private KeyType keyType;
        [SerializeField] private string key;
        
        public KeyType KeyType => keyType;
        public string Key => key;
    }
}