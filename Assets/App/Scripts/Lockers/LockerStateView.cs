using UnityEngine;

namespace App.Lockers
{
	public class LockerStateView : MonoBehaviour
	{
		[SerializeField] private LockerLockView lockerLockView;
		[SerializeField] private LockerMaterialView lockerMaterialView;

		public void SetState(LockerState state)
		{
			lockerLockView.SetState(state);
			lockerMaterialView.SetState(state);
		}
	}
}