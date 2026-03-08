using App.DraggedObjects;
using App.InteractiveObjects;
using UnityEngine;

namespace App.DraggedDoors
{
	public class DraggedDoor : MonoBehaviour
	{
		[SerializeField] private DraggedObj draggedObj;
		[SerializeField] private InteractiveObj interactiveObj;
		
		public void SetState(bool isDragged)
		{
			draggedObj.SetStateWithRb(isDragged);
			interactiveObj.SetActivityState(isDragged);
		}
	}
}