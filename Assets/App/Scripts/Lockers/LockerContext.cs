using App.InteractiveObjects;

namespace App.Lockers
{
	public class LockerContext
	{
		public LockerState State { get; set; }
		public readonly LockerStateView StateView;
		public readonly LockerDoorBlocker DoorBlocker;

		public LockerContext(LockerStateView stateView, LockerDoorBlocker doorBlocker)
		{
			StateView = stateView;
			DoorBlocker = doorBlocker;
		}
	}
}