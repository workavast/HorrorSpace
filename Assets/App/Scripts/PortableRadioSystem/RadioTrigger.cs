using App.Character;
using UnityEngine;

namespace App.PortableRadioSystem
{
	public class RadioTrigger : MonoBehaviour
	{
		[SerializeField] private PlayerTriggerZone playerTriggerZone;
		[SerializeField] private RadioPlayer radioPlayer;

		private void Awake()
		{
			playerTriggerZone.OnEnter += PlayAudio;
		}

		private void PlayAudio(PlayerCharacterController _)
		{
			radioPlayer.Play();
			playerTriggerZone.OnEnter -= PlayAudio;
		}
	}
}