namespace App.Lockers.FSM
{
	public class ActiveLockedState : LockerStateBase
	{
		public ActiveLockedState(LockerContext context) : base(context)
		{
		}

		public override void Enter(string[] args)
		{
			_context.View.SetState(LockerState.ActiveLocked);
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