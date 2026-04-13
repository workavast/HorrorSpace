using App.LifetimeScopes;
using Avastrad.Utils;
using UnityEngine;

namespace App.CursorBehaviour
{
	public class CursorVisibilityChanger : INonLazy
	{
		private readonly RequestCounter _requestCounter;

		public CursorVisibilityChanger()
		{
			_requestCounter = new RequestCounter(UpdateVisibility);
			UpdateVisibility(_requestCounter.RequestMoreZero);
		}

		public void AddRequest() 
			=> _requestCounter.AddRequest();

		public void RemoveRequest() 
			=> _requestCounter.RemoveRequest();

		private static void UpdateVisibility(bool isVisible)
		{
			Cursor.visible = isVisible;
			Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}
}