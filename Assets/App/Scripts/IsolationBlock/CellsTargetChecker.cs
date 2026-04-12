using System.Collections.Generic;
using App.Audio;
using App.Audio.Sources;
using App.IsolationBlock.Cells;
using App.SecurityCheckpoint;
using UnityEngine;

namespace App.IsolationBlock
{
	public class CellsTargetChecker : MonoBehaviour
	{
		[SerializeField] private SecurityCheckpointController securityCheckpointController;
		[SerializeField] private List<IsolationCell> cells;
		[Header("System notification")]
		[SerializeField] private PublicAddressSystem publicAddressSystem;
		[SerializeField] private AudioSourceHolder systemVoiceOnTargetIsolated;
		[SerializeField] private AudioSourceHolder systemVoiceOnTargetEscaped;

		public bool IsTargetInside { get; private set; }

		private void Start()
		{
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
			publicAddressSystem.Play(systemVoiceOnTargetIsolated);
		}

		private void TargetExit(TargetToIsolate obj)
		{
			IsTargetInside = false;
			securityCheckpointController.SetAvailableState(false);
			publicAddressSystem.Play(systemVoiceOnTargetEscaped);
		}
	}
}