using UnityEngine;

namespace App.Lockers
{
	public class LockerDoorBlocker : MonoBehaviour
	{
		[SerializeField] private Rigidbody doorRb;
		
		public void SetState(bool isBlocked)
		{
			doorRb.isKinematic = isBlocked;
		}
	}
}