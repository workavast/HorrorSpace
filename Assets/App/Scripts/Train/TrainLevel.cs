using App.Audio.Sources;
using App.ElectricPanels;
using App.PortableRadioSystem;
using App.ScenesLoading;
using Avastrad.ScenesLoading;
using Avastrad.Utils.ScenesReferencing;
using UnityEngine;
using VContainer;

namespace App.Train
{
	public class TrainLevel : MonoBehaviour
	{
		[SerializeField] private ElectricPanel electricPanel;
		[SerializeField] private AudioSourceHolder audioSourceHolderPrefab;
		[SerializeField] private SceneReference sceneRef;
		[SerializeField] private LoadingConfig loadingConfig;
		
		[Inject] private ISceneLoader _sceneLoader;
		[Inject] private PortableRadio _radioPlayer;
		
		private void Awake()
		{
			electricPanel.OnFix += Play;
		}

		private void Play()
		{
			_radioPlayer.Play(audioSourceHolderPrefab);
			_radioPlayer.OnAllAudiosPlayed += LoadNextMission;
		}

		private void LoadNextMission()
		{
			_radioPlayer.OnAllAudiosPlayed -= LoadNextMission;
			_sceneLoader.LoadSceneExt(sceneRef, loadingConfig);
		}
	}
}