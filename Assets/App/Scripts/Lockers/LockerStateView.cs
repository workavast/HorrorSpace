using UnityEngine;

namespace App.Lockers
{
	public class LockerStateView : MonoBehaviour
	{
		[SerializeField] private LockerHandView lockerHandView;
		[SerializeField] private LockerMaterialView lockerMaterialView;

		public void SetState(LockerState state)
		{
			lockerHandView.SetState(state);
			lockerMaterialView.SetState(state);
		}
	}
}