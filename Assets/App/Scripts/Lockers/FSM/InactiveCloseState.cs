namespace App.Lockers.FSM
{
	public class InactiveCloseState : LockerStateBase
	{
		public InactiveCloseState(LockerContext context) : base(context)
		{
		}

		public override void Enter(string[] args)
		{
			_context.StateView.SetState(LockerState.InactiveLocked);
			_context.DoorBlocker.SetState(true);
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