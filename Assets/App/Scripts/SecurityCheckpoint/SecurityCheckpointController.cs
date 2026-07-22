using App.Usable;
using UnityEngine;

namespace App.SecurityCheckpoint
{
	public class SecurityCheckpointController : MonoBehaviour
	{
		[SerializeField] private UsableObj securityCheckpointBtn;
		
		public void SetAvailableState(bool isAvailable)
		{
			securityCheckpointBtn.SetUsableState(isAvailable);
		}
	}
}