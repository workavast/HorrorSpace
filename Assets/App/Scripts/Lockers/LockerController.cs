using UnityEngine;

namespace App.Lockers
{
	public class LockerController : MonoBehaviour
	{
		[SerializeField] private LockerState initialState;
		[SerializeField] private LockerStateView view;
		[SerializeField] private LockerDoorBlocker doorBlocker;
		
		private void Awake()
		{
			view.SetState(initialState);
			doorBlocker.SetState(initialState != LockerState.Unlocked);
		}

		public void SetUnLockState(bool unlocked)
		{
			view.SetState(unlocked ? LockerState.Unlocked : LockerState.Locked);
			doorBlocker.SetState(!unlocked);
		}
	}
}