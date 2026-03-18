using App.Doors;
using App.Usable;
using UnityEngine;

namespace App.SecurityCheckpoint
{
	public class SecurityCheckpointDoorsToggler : MonoBehaviour
	{
		[SerializeField] private UsableObj usableObj;
		[SerializeField] private DoorController entranceDoor;
		[SerializeField] private DoorController exitDoor;

		private void Awake()
		{
			usableObj.AddCheck(CheckEntranceDoor);
			usableObj.OnUse += Interact;
		}

		private void Start()
		{
			entranceDoor.SetUnLockState(true);
			exitDoor.SetUnLockState(false);
		}

		private void Interact()
		{
			entranceDoor.SetUnLockState(false);
			exitDoor.SetUnLockState(true);
			usableObj.SetState(false);
		}
		
		private bool CheckEntranceDoor()
		{
			if (entranceDoor.IsOpen)
			{
				var result = entranceDoor.TryToggle();
				return result;
			}
			
			return true;
		}
	}
}