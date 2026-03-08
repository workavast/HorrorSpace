namespace App.Lockers.FSM
{
	public class ActiveOpenState : LockerStateBase
	{
		public ActiveOpenState(LockerContext context) : base(context)
		{
		}

		public override void Enter(string[] args)
		{
			_context.StateView.SetState(LockerState.ActiveUnlocked);
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