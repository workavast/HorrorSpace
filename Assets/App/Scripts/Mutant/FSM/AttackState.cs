using System.Collections.Generic;

namespace App.Mutant.FSM
{
	public class AttackState : MutantAiState
	{
		public AttackState(MutantContext ctx, MutantParams @params) : base(ctx, @params)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			StopMovement();
		}

		public override void Exit()
		{
		}

		public override void ManualUpdate()
		{
			if (_ctx.Target == null)
			{
				SetState<CheckState>();
				return;
			}

			if (!TargetCanBeAttacked())
			{
				if (CanSeeTarget())
				{
					SetState<ChaseState>();
					return;
				}
				else
				{
					SetState<CheckState>();
					return;
				}
			}
			else if (CanSeeTarget())
			{
				_ctx.LastSeenPlayerPosition = _ctx.Target.Position;
				RotateTowards(_ctx.Target.Position);
			}
		}

		public override void ManualFixedUpdate()
		{
		}
	}
}