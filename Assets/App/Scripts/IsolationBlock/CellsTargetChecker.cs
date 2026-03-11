using System.Collections.Generic;
using App.IsolationBlock.Cells;
using UnityEngine;

namespace App.IsolationBlock
{
	public class CellsTargetChecker : MonoBehaviour
	{
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

		private void TargetEnter(Target obj)
		{
			Debug.Log("CellsTargetChecker.TargetEnter");
			IsTargetInside = true;
		}

		private void TargetExit(Target obj)
		{
			Debug.Log("CellsTargetChecker.TargetExit");
			IsTargetInside = false;
		}
	}
}