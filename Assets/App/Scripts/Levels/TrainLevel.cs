using App.Audio.Sources;
using App.ElectricPanels;
using App.ScenesLoading;
using App.SpeakersSystem;
using Avastrad.ScenesLoading;
using Avastrad.Utils.ScenesReferencing;
using UnityEngine;
using VContainer;

namespace App.Levels
{
	public class TrainLevel : MonoBehaviour, ILevel
	{
		[SerializeField] private SpeakerConfig speakerConfig;
		[SerializeField] private ElectricPanel electricPanel;
		[SerializeField] private AudioSourceHolder audioSourceHolderPrefab;
		[SerializeField] private SceneReference sceneRef;
		[SerializeField] private LoadingConfig loadingConfig;
		
		private ISceneLoader _sceneLoader;
		private MonologSpeakers _monologSpeakers;
		private ISpeaker _speaker;
		
		public void Initialize(IObjectResolver container)
		{
			_sceneLoader =  container.Resolve<ISceneLoader>();
			_monologSpeakers =  container.Resolve<MonologSpeakers>();
			
			electricPanel.OnFix += Play;
			_speaker = _monologSpeakers.GetSpeaker(speakerConfig.Id);
		}
		
		private void Play()
		{
			_speaker.Play(audioSourceHolderPrefab);
			_speaker.OnAllAudiosPlayed += LoadNextMission;
		}

		private void LoadNextMission()
		{
			_speaker.OnAllAudiosPlayed -= LoadNextMission;
			_sceneLoader.LoadSceneExt(sceneRef, loadingConfig);
		}
	}
}