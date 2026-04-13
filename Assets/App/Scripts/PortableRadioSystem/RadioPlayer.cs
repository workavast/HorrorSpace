using App.Audio.Sources;
using App.SpeakersSystem;
using UnityEngine;
using VContainer;

namespace App.PortableRadioSystem
{
	public class RadioPlayer : MonoBehaviour
	{
		[SerializeField] private SpeakerConfig speakerConfig;
		[SerializeField] private AudioSourceHolder audioSourceHolderPrefab;

		private MonologSpeakers _portableRadio;

		public void Initialize(IObjectResolver container)
		{
			_portableRadio = container.Resolve<MonologSpeakers>();
		}

		public void Play()
		{
			_portableRadio.Play(speakerConfig, audioSourceHolderPrefab);
		}
	}
}