namespace App.Lockers.FSM
{
	public class ActiveCloseState : LockerStateBase
	{
		public ActiveCloseState(LockerContext context) : base(context)
		{
		}

		public override void Enter(string[] args)
		{
			_context.StateView.SetState(LockerState.ActiveLocked);
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