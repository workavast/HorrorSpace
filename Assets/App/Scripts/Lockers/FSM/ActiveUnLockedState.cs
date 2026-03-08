namespace App.Lockers.FSM
{
	public class ActiveUnLockedState : LockerStateBase
	{
		public ActiveUnLockedState(LockerContext context) : base(context)
		{
		}

		public override void Enter(string[] args)
		{
			_context.View.SetState(LockerState.ActiveUnlocked);
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