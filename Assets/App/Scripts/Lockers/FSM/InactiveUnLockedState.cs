namespace App.Lockers.FSM
{
	public class InactiveUnLockedState : LockerStateBase
	{
		public InactiveUnLockedState(LockerContext context) : base(context)
		{
		}

		public override void Enter(string[] args)
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