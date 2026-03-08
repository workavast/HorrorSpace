using System;
using System.Collections.Generic;
using App.Lockers.FSM;
using App.Lockers.Views;
using Avastrad.FSM;
using UnityEngine;

namespace App.Lockers
{
	public class Locker : MonoBehaviour
	{
		[SerializeField] private LockerState initialState;
		[SerializeField] private LockerView view;
		[SerializeField] private LockerDoor door;
		
		private LockerContext _context;
		private StateMachine _fsm;
		
		private void Awake()
		{
			_context = new LockerContext(view, door);

			var states = new List<StateBase>
			{
				new InactiveUnLockedState(_context),
				new ActiveUnLockedState(_context),
				new InactiveLockedState(_context),
				new ActiveLockedState(_context)
			};
			_fsm = new StateMachine(states);

			switch (initialState)
			{
				case LockerState.InactiveLocked:
					_fsm.Init<InactiveLockedState>();
					break;
				case LockerState.InactiveUnLocked:
					_fsm.Init<InactiveUnLockedState>();
					break;
				case LockerState.ActiveLocked:
					_fsm.Init<ActiveLockedState>();
					break;
				case LockerState.ActiveUnlocked:
					_fsm.Init<ActiveUnLockedState>();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void SetUnLockState(bool unlocked)
		{
			if (unlocked)
				_fsm.SetState<ActiveUnLockedState>();
			else
				_fsm.SetState<ActiveLockedState>();
		}
	}
}