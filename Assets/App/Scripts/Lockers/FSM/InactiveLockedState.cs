using System.Collections.Generic;

namespace App.Lockers.FSM
{
	public class InactiveLockedState : LockerStateBase
	{
		public InactiveLockedState(LockerContext context) : base(context)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			_context.View.SetState(LockerState.InactiveLocked);
			_context.Door.SetState(false);
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