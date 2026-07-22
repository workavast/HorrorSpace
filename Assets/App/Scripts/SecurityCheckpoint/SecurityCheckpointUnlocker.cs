using App.Usable;
using UnityEngine;

namespace App.SecurityCheckpoint
{
	public class SecurityCheckpointUnlocker : MonoBehaviour
	{
		[SerializeField] private UsableObj usableObj;
		[SerializeField] private SecurityCheckpointController securityCheckpointBtn;

		private void Start()
		{
			usableObj.OnUse += Unlock;
		}

		private void OnDestroy()
		{
			usableObj.OnUse -= Unlock;
		}

		private void Unlock()
		{
			securityCheckpointBtn.SetAvailableState(true);
			usableObj.SetUsableState(false);
		}
	}
}