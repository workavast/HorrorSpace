using UnityEngine;
using VContainer;

namespace App.SpeakersSystem.SpeakersTriggers
{
    public class SpeakerTrigger : MonoBehaviour
    {
        [SerializeField] private SpeakerTriggerZone triggerZone;
        [SerializeField] private SpeakerTriggerEvent eventer;
        [SerializeField] private SpeakerTriggerPlayer player;

        [Inject]
        public void Construct(SpeakerEventsDatabase database, MonologSpeakers monologSpeakers)
        {
            eventer.Construct(database);
            player.Construct(database, monologSpeakers);
        }

        private void Start()
        {
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