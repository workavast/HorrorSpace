using App.Audio;
using App.Audio.Sources;
using UnityEngine;

namespace App.SecurityCheckpoint
{
    public class SecurityCheckPointPasPlayer : MonoBehaviour
    {
        [SerializeField] private SecurityCheckpointDoorsToggler securityCheckpointDoorsToggler;
        [SerializeField] private PublicAddressSystem publicAddressSystem;
        [SerializeField, Tooltip("Can be null")] private AudioSourceHolder audioSourceEntranceOpen;
        [SerializeField, Tooltip("Can be null")] private AudioSourceHolder audioSourceExitOpen;

        private void Awake()
        {
            securityCheckpointDoorsToggler.OnStateChanged += PlayAudio;
        }

        private void PlayAudio(bool isEntranceOpen)
        {
            TryPlay(isEntranceOpen ? audioSourceEntranceOpen : audioSourceExitOpen);
        }

        private void TryPlay(AudioSourceHolder audioSourceHolder)
        {
            if (audioSourceHolder == null)
                return;
            
            publicAddressSystem.Play(audioSourceHolder);
        }
    }
}