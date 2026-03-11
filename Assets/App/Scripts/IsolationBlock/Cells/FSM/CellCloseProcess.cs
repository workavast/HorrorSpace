using System.Collections.Generic;

namespace App.IsolationBlock.Cells.FSM
{
	public class CellCloseProcess : CellState
	{
		public CellCloseProcess(CellContext ctx) : base(ctx)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			_ctx.IsClosed = false;
			_ctx.Door.SetState(false, true, true);
			_ctx.TriggerZone.SetActive(false);
		}

		public override void Exit()
		{
			
		}

		public override void ManualUpdate()
		{
			if (_ctx.Door.IsClosed)
			{
				SetState<CellIsClosed>();
				return;
			}			
		}

		public override void ManualFixedUpdate()
		{
			
		}
	}
}