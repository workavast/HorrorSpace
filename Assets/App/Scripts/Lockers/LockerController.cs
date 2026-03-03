using UnityEngine;

namespace App.Lockers
{
	public class LockerController : MonoBehaviour
	{
		[SerializeField] private LockerState initialState;
		[SerializeField] private LockerStateView view;
		[SerializeField] private GameObject door;
		
		private void Awake()
		{
			view.SetState(initialState);
		}

		public void SetUnLockState(bool unlocked)
		{
			view.SetState(unlocked ? LockerState.Unlocked : LockerState.Locked);
			door.SetActive(!unlocked);
		}
	}
}