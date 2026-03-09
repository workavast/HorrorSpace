using System.Collections.Generic;

namespace App.Lockers.FSM
{
	public class ActiveUnLockedState : LockerStateBase
	{
		public ActiveUnLockedState(LockerContext context) : base(context)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			_context.View.SetState(LockerState.ActiveUnlocked);
			_context.Door.SetState(true);
		}

		public override void Exit()
		{
		}

		public override void ManualUpdate()
		{
		}

		public override void ManualFixedUpdate()
		{
		}
	}
}