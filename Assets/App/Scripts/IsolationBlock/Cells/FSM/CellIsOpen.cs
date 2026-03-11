using System.Collections.Generic;

namespace App.IsolationBlock.Cells.FSM
{
	public class CellIsOpen : CellState
	{
		public CellIsOpen(CellContext ctx) : base(ctx)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			_ctx.IsClosed = false;
			_ctx.Door.SetState(true, true, false);
			_ctx.TriggerZone.SetActive(false);
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