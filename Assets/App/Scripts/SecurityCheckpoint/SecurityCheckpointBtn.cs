using System;
using App.Character;
using App.Doors;
using App.Usable;
using UnityEngine;

namespace App.SecurityCheckpoint
{
	public class SecurityCheckpointBtn : MonoBehaviour, IUsable
	{
		[SerializeField] private DoorController entranceDoor;
		[SerializeField] private DoorController exitDoor;
		[SerializeField] private bool isAvailable;

		public bool IsUsable => isAvailable;

		public event Action OnUse;
		public event Action<bool> OnTryUse;
		public event Action OnUsableStateChanged;

		private void Start()
		{
			entranceDoor.SetUnLockState(true);
			exitDoor.SetUnLockState(false);
		}

		public void Interact(PlayerContext playerContext)
		{
			if (!IsUsable)
			{
				OnTryUse?.Invoke(false);
				return;
			}

			if (entranceDoor.IsOpen)
			{
				var result = entranceDoor.Interact(playerContext);
				if (!result)
				{
					OnTryUse?.Invoke(false);
					return;
				}
			}
			
			entranceDoor.SetUnLockState(false);
			exitDoor.SetUnLockState(true);

			SetAvailableState(false);
			OnUse?.Invoke();
			OnTryUse?.Invoke(true);
		}
		
		public void SetAvailableState(bool isAvailableState)
		{
			isAvailable = isAvailableState;
			OnUsableStateChanged?.Invoke();
		}
	}
}