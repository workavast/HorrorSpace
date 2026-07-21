using System;
using UnityEngine;

namespace App.Usable
{
	public class UsableObj : MonoBehaviour, IUsable
	{
		public bool IsUsable { get; private set; }
		
		private Func<bool> _check;
		
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
			
			if (_check != null && !_check())
			{
				OnTryUse?.Invoke(false);
				return;
			}
			
			OnUse?.Invoke();
			OnTryUse?.Invoke(true);
		}

		public void AddCheck(Func<bool> check)
		{
			_check = check;
		}
		
		public void SetState(bool isUsable)
		{
			IsUsable = isUsable;
			OnUsableStateChanged?.Invoke();
		}
	}
}