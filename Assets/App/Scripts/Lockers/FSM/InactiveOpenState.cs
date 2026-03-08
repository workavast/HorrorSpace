namespace App.Lockers.FSM
{
	public class InactiveOpenState : LockerStateBase
	{
		public InactiveOpenState(LockerContext context) : base(context)
		{
		}

		public override void Enter(string[] args)
		{
			_context.StateView.SetState(LockerState.InactiveLocked);
			_context.DoorBlocker.SetState(false);
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