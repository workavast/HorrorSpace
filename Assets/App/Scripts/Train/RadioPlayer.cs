using App.Audio.Sources;
using UnityEngine;
using VContainer;

namespace App
{
	public class RadioPlayer : MonoBehaviour
	{
		[SerializeField] private AudioSourceHolder audioSourceHolderPrefab;

		private PortableRadio _portableRadio;

		public void Initialize(IObjectResolver container)
		{
			_portableRadio = container.Resolve<PortableRadio>();
		}

		public void Play()
		{
			_portableRadio.Play(audioSourceHolderPrefab);
		}
	}
}