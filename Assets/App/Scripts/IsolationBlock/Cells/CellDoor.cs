using App.DraggedDoors;
using App.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace App.IsolationBlock.Cells
{
	public class CellDoor : DraggedDoor
	{
		[SerializeField] private BoxCollider obstaclesCheckZone;
		[SerializeField] private LayerMask obstaclesLayerMask;
		[SerializeField] private NavMeshObstacle doorObstacle;
		
		public bool HasObstacles() 
			=> obstaclesCheckZone.CheckBox(obstaclesLayerMask);
		
		public void SetDoorObstacle(bool isEnabled)
			=> doorObstacle.enabled = isEnabled;
	}
}