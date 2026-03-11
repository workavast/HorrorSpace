using Avastrad.FSM;

namespace App.IsolationBlock.Cells.FSM
{
	public abstract class CellState : StateBase
	{
		protected readonly CellContext _ctx;

		protected CellState(CellContext ctx)
		{
			_ctx = ctx;
		}
	}
}