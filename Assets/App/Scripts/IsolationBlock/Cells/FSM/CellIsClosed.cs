using System.Collections.Generic;

namespace App.IsolationBlock.Cells.FSM
{
	public class CellIsClosed : CellState
	{
		public CellIsClosed(CellContext ctx) : base(ctx)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			_ctx.IsClosed = true;
			_ctx.Door.SetState(false, false, true);
			_ctx.Door.SetDoorObstacle(true);
			_ctx.TriggerZone.SetActive(true);
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