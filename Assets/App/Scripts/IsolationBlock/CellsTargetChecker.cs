using System.Collections.Generic;
using App.Audio.Sources;
using App.IsolationBlock.Cells;
using App.SecurityCheckpoint;
using App.SpeakersSystem;
using UnityEngine;
using VContainer;

namespace App.IsolationBlock
{
	public class CellsTargetChecker : MonoBehaviour
	{
		[SerializeField] private SecurityCheckpointController securityCheckpointController;
		[SerializeField] private List<IsolationCell> cells;
		[Header("Public Address System")]
		[SerializeField] private SpeakerConfig speakerConfig;
		[SerializeField] private AudioSourceHolder systemVoiceOnTargetIsolated;
		[SerializeField] private AudioSourceHolder systemVoiceOnTargetEscaped;

		public bool IsTargetInside { get; private set; }

		private MonologSpeakers _monologSpeakers;

		public void Initialize(IObjectResolver objResolver)
		{
			_monologSpeakers = objResolver.Resolve<MonologSpeakers>();	
			
			foreach (var cell in cells)
			{
				cell.OnTargetEnter += TargetEnter;
				cell.OnTargetExit += TargetExit;
			}
		}

		private void OnDestroy()
		{
			foreach (var cell in cells)
			{
				cell.OnTargetEnter -= TargetEnter;
				cell.OnTargetExit -= TargetExit;
			}
		}

		private void TargetEnter(TargetToIsolate obj)
		{
			IsTargetInside = true;
			securityCheckpointController.SetAvailableState(true);
			_monologSpeakers.Play(speakerConfig, systemVoiceOnTargetIsolated);
		}

		private void TargetExit(TargetToIsolate obj)
		{
			IsTargetInside = false;
			securityCheckpointController.SetAvailableState(false);
			_monologSpeakers.Play(speakerConfig, systemVoiceOnTargetEscaped);
		}
	}
}