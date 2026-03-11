using System;
using App.DraggedDoors;
using UnityEngine;

namespace App.IsolationBlock.Cells
{
	[Serializable]
	public class CellContext
	{
		public readonly DraggedDoor Door;
		public readonly IsolationCellTriggerZone TriggerZone;

		public bool IsClosed;
		
		public CellContext(DraggedDoor door, IsolationCellTriggerZone triggerZone)
		{
			Door = door;
			TriggerZone = triggerZone;
		}
	}
}