using System;
using App.LifetimeScopes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.EscProviding
{
	public class EscTrigger : INonLazy, IDisposable
	{
		private readonly EscProvider _escProvider;
		private readonly InputActionReference _escRef;

		public EscTrigger(EscProvider escProvider, InputActionReference escRef)
		{
			_escProvider = escProvider;
			_escRef = escRef;
			
			_escRef.action.Enable();
			_escRef.action.performed += EscPressed;
		}
		
		public void Dispose()
		{
			_escRef.action.started -= EscPressed;
		}

		private void EscPressed(InputAction.CallbackContext obj)
		{
			_escProvider.OnEscPressed();
		}
	}
}