using Avastrad.FSM;

namespace App.Lockers.FSM
{
	public abstract class LockerStateBase : StateBase
	{
		protected readonly LockerContext _context;

		protected LockerStateBase(LockerContext context)
		{
			_context = context;
		}
	}
}