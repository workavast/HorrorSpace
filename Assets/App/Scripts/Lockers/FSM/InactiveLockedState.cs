namespace App.Lockers.FSM
{
	public class InactiveLockedState : LockerStateBase
	{
		public InactiveLockedState(LockerContext context) : base(context)
		{
		}

		public override void Enter(string[] args)
		{
			_context.View.SetState(LockerState.InactiveLocked);
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