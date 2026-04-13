using UnityEngine;
using VContainer;

namespace App.SpeakersSystem.SpeakersTriggers
{
    public class SpeakerTrigger : MonoBehaviour
    {
        [SerializeField] private SpeakerTriggerZone triggerZone;
        [SerializeField] private SpeakerTriggerEvent eventer;
        [SerializeField] private SpeakerTriggerPlayer player;

        public void Initialize(IObjectResolver container)
        {
            eventer.Initialize(container);
            player.Initialize(container);
            
            triggerZone.OnEnter += OnSpeakerEnter;
        }

        private void OnDestroy()
        {
            triggerZone.OnEnter -= OnSpeakerEnter;
        }

        private void OnSpeakerEnter(SpeakerTriggerObj _)
        {
            eventer.PerformEvent();
            
            if (player.TryPlayAudio()) 
                triggerZone.OnEnter -= OnSpeakerEnter;
        }
    }
}