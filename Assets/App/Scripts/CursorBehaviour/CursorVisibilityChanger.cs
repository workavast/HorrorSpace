using App.Installers;
using App.LifetimeScopes;
using Avastrad.Utils;
using UnityEngine;
using VContainer.Unity;

namespace App.CursorBehaviour
{
	public class CursorVisibilityChanger : INonLazy, IPostLateTickable
	{
		private readonly RequestCounter _requestCounter;
		private bool _lastValueRequestMoreZero;
		
		public CursorVisibilityChanger()
		{
			_requestCounter = new RequestCounter(null);
			UpdateVisibility(_requestCounter.RequestMoreZero);
		}

		public void AddRequest() 
			=> _requestCounter.AddRequest();

		public void RemoveRequest() 
			=> _requestCounter.RemoveRequest();
		
		public void PostLateTick()
		{
			if (_lastValueRequestMoreZero != _requestCounter.RequestMoreZero)
			{
				UpdateVisibility(_requestCounter.RequestMoreZero);
				_lastValueRequestMoreZero = _requestCounter.RequestMoreZero;
			}
		}
		
		private static void UpdateVisibility(bool isVisible)
		{
			Cursor.visible = isVisible;
			Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}
}