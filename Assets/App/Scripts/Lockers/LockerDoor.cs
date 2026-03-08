using App.DraggedObjects;
using App.InteractiveObjects;
using UnityEngine;

namespace App.Lockers
{
	public class LockerDoor : MonoBehaviour
	{
		[SerializeField] private DraggedObj draggedObj;
		[SerializeField] private InteractiveObj interactiveObj;
		
		public void SetState(bool isLocked)
		{
			draggedObj.SetStateWithRb(!isLocked);
			interactiveObj.SetActivityState(!isLocked);
		}
	}
}