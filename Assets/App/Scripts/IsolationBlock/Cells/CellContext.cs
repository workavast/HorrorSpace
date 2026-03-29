using System;
using App.DraggedDoors;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace App.IsolationBlock.Cells
{
	[Serializable]
	public class CellContext
	{
		public readonly CellDoor Door;
		public readonly IsolationCellTriggerZone TriggerZone;

		public bool IsClosed;
		
		public CellContext(CellDoor door, IsolationCellTriggerZone triggerZone)
		{
			Door = door;
			TriggerZone = triggerZone;
		}
	}
}