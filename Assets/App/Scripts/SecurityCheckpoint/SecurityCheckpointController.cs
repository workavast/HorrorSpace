using UnityEngine;

namespace App.SecurityCheckpoint
{
	public class SecurityCheckpointController : MonoBehaviour
	{
		[SerializeField] private SecurityCheckpointBtn securityCheckpointBtn;
		
		public void SetAvailableState(bool isAvailable)
		{
			securityCheckpointBtn.SetAvailableState(isAvailable);
		}
	}
}