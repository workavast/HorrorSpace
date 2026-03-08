using System;
using App.Character;
using App.InteractiveObjects;
using App.Usable;
using UnityEngine;

namespace App.SecurityCheckpoint
{
	public class SecurityCheckpointUnlocker : MonoBehaviour, IUsable
	{
		[SerializeField] private SecurityCheckpointBtn securityCheckpointBtn;

		public bool IsUsable { get; private set; } = true;
		
		public event Action OnUse;
		public event Action<bool> OnTryUse;
		public event Action OnUsableStateChanged;
		
		public void Interact(PlayerContext playerContext)
		{
			if (!IsUsable)
			{
				OnTryUse?.Invoke(false);
				return;
			}

			IsUsable = false;
			securityCheckpointBtn.SetAvailableState(true);
			OnUse?.Invoke();
			OnTryUse?.Invoke(true);
			OnUsableStateChanged?.Invoke();
		}
	}
}