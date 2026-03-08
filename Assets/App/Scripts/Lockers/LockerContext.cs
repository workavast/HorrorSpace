using App.Lockers.Views;

namespace App.Lockers
{
	public class LockerContext
	{
		public readonly LockerView View;
		public readonly LockerDoor Door;

		public LockerContext(LockerView view, LockerDoor door)
		{
			View = view;
			Door = door;
		}
	}
}