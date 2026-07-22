using System;
using UnityEngine;

namespace App.Usable
{
	public class UsableObj : MonoBehaviour, IUsable
	{
		[field: SerializeField] public bool IsUsable { get; private set; }
		
		private Func<bool> _externalUsableCheck;
		
		public event Action OnUse;
		public event Action<bool> OnTryUse;
		public event Action OnUsableStateChanged;
		
		public void Interact()
		{
			if (!IsUsable)
			{
				OnTryUse?.Invoke(false);
				return;
			}
			
			if (_externalUsableCheck != null && !_externalUsableCheck())
			{
				OnTryUse?.Invoke(false);
				return;
			}
			
			OnUse?.Invoke();
			OnTryUse?.Invoke(true);
		}

		public void AddExternalUsableCheck(Func<bool> usableCheck) 
			=> _externalUsableCheck = usableCheck;

		public void SetUsableState(bool isUsable)
		{
			IsUsable = isUsable;
			OnUsableStateChanged?.Invoke();
		}
	}
}