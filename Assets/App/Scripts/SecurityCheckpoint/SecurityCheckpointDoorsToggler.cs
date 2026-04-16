using System;
using App.Doors;
using App.Usable;
using UnityEngine;

namespace App.SecurityCheckpoint
{
	public class SecurityCheckpointDoorsToggler : MonoBehaviour
	{
		[SerializeField] private bool initialStateIsEntranceOpen = true;
		[SerializeField] private UsableObj usableObj;
		[SerializeField] private DoorController entranceDoor;
		[SerializeField] private DoorController exitDoor;
		
		private bool _isEntranceOpen = true;
		
		/// <summary>
		/// return isEntranceOpen (false, mean that exit is open)
		/// </summary>
		public event Action<bool> OnStateChanged;
		
		private void Awake()
		{
			usableObj.AddCheck(CheckEntranceDoor);
			usableObj.OnUse += Interact;
		}

		private void Start() 
			=> ToggleEntrance(initialStateIsEntranceOpen, false, false);

		private void Interact() 
			=> ToggleEntrance(false, false, true);

		private void ToggleEntrance(bool isEntranceOpenState, bool isUsable, bool notify)
		{
			_isEntranceOpen = isEntranceOpenState;
			
			entranceDoor.SetLockState(!_isEntranceOpen);
			exitDoor.SetLockState(_isEntranceOpen);

			usableObj.SetState(isUsable);

			if (notify)
				OnStateChanged?.Invoke(_isEntranceOpen);
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