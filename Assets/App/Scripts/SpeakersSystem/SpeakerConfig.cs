using UnityEngine;

namespace App.SpeakersSystem
{
    [CreateAssetMenu(fileName = nameof(SpeakerConfig), menuName = "Configs/" + nameof(SpeakerConfig))]
    public class SpeakerConfig : ScriptableObject
    {
        [SerializeField] private string id;
        
        public string Id => id;
    }
}