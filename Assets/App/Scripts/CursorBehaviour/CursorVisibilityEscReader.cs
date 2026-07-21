using System;
using App.EscProviding;
using App.Installers;
using App.LifetimeScopes;

namespace App.CursorBehaviour
{
	public class CursorVisibilityEscReader : INonLazy, IEscListener, IDisposable
	{
		private readonly CursorVisibilityChanger _cursorVisibilityChanger;
		private readonly EscProvider _escProvider;

		private bool _requested;

		public CursorVisibilityEscReader(CursorVisibilityChanger cursorVisibilityChanger, EscProvider escProvider)
		{
			_cursorVisibilityChanger = cursorVisibilityChanger;
			_escProvider = escProvider;
			_escProvider.Sub(this);
		}

		public void Dispose()
		{
			_escProvider.UnSub(this);
		}

		public void OnEscPressed()
		{
			_requested = !_requested;

			if (_requested)
				_cursorVisibilityChanger.AddRequest();
			else
				_cursorVisibilityChanger.RemoveRequest();
		}
	}
}