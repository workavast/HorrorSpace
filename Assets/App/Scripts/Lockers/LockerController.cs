using System;
using System.Collections.Generic;
using App.Lockers.FSM;
using Avastrad.FSM;
using UnityEngine;

namespace App.Lockers
{
	public class LockerController : MonoBehaviour
	{
		[SerializeField] private LockerState initialState;
		[SerializeField] private LockerStateView view;
		[SerializeField] private LockerDoorBlocker doorBlocker;
		
		private LockerContext _context;
		private StateMachine _fsm;
		
		private void Awake()
		{
			_context = new LockerContext(view, doorBlocker);

			var states = new List<StateBase>
			{
				new InactiveOpenState(_context),
				new ActiveOpenState(_context),
				new InactiveCloseState(_context),
				new ActiveCloseState(_context)
			};
			_fsm = new StateMachine(states);

			switch (initialState)
			{
				case LockerState.InactiveLocked:
					_fsm.Init<InactiveCloseState>();
					break;
				case LockerState.InactiveUnLocked:
					_fsm.Init<InactiveOpenState>();
					break;
				case LockerState.ActiveLocked:
					_fsm.Init<ActiveOpenState>();
					break;
				case LockerState.ActiveUnlocked:
					_fsm.Init<ActiveCloseState>();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void SetUnLockState(bool unlocked)
		{
			if (unlocked)
				_fsm.SetState<ActiveOpenState>();
			else
				_fsm.SetState<ActiveCloseState>();
		}
	}
}