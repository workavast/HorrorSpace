using App.DraggedObjects;
using App.InteractiveObjects;
using UnityEngine;

namespace App.Lockers
{
	public class LockerDoorBlocker : MonoBehaviour
	{
		[SerializeField] private DraggedObj draggedObj;
		[SerializeField] private InteractiveObj interactiveObj;
		
		public void SetState(bool isBlocked)
		{
			draggedObj.SetStateWithRb(!isBlocked);
			interactiveObj.SetActivityState(!isBlocked);
		}
	}
}