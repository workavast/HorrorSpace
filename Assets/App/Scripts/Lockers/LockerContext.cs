using App.DraggedDoors;
using App.Lockers.Views;

namespace App.Lockers
{
	public class LockerContext
	{
		public readonly LockerView View;
		public readonly DraggedDoor Door;

		public LockerContext(LockerView view, DraggedDoor door)
		{
			View = view;
			Door = door;
		}
	}
}