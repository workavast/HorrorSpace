using System;
using App.EscProviding;
using App.LifetimeScopes;
using UnityEngine;

namespace App.CursorBehaviour
{
	public class CursorVisibilityChanger : INonLazy, IEscListener, IDisposable
	{
		private readonly EscProvider _escProvider;

		public CursorVisibilityChanger(EscProvider escProvider)
		{
			_escProvider = escProvider;
			_escProvider.Sub(this);
			OnEscPressed();
		}

		public void Dispose()
		{
			_escProvider.UnSub(this);
		}

		public void OnEscPressed()
		{
			Cursor.visible = !Cursor.visible;
			Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}
}