using System.Collections.Generic;
using App.IsolationBlock.Cells;
using App.SecurityCheckpoint;
using UnityEngine;

namespace App.IsolationBlock
{
	public class CellsTargetChecker : MonoBehaviour
	{
		[SerializeField] private SecurityCheckpointController securityCheckpointController;
		[SerializeField] private List<IsolationCell> cells;

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
			Debug.Log("CellsTargetChecker.TargetEnter");
			IsTargetInside = true;
			securityCheckpointController.SetAvailableState(true);
		}

		private void TargetExit(TargetToIsolate obj)
		{
			Debug.Log("CellsTargetChecker.TargetExit");
			IsTargetInside = false;
			securityCheckpointController.SetAvailableState(false);
		}
	}
}