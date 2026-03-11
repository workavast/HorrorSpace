using App.DraggedDoors;
using App.Utils;
using UnityEngine;

namespace App.IsolationBlock.Cells
{
	public class CellDoor : DraggedDoor
	{
		[SerializeField] private BoxCollider obstaclesCheckZone;
		[SerializeField] private LayerMask obstaclesLayerMask;
		
		public bool HasObstacles() 
			=> obstaclesCheckZone.CheckBox(obstaclesLayerMask);
	}
}