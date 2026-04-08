using System.Collections.Generic;

namespace App.Mutant.FSM
{
	public class ChaseState : MutantAiState
	{
		public ChaseState(MutantContext ctx, MutantParams @params) : base(ctx, @params)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			_ctx.TargetSpeed = _params.ChaseSpeed;
			_ctx.AnimationController.SetIsRun(true);
			CalculatePath(_ctx.Target.Position, _ctx.Path);
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

			if (CanSeeTarget())
			{
				_ctx.LastSeenPlayerPosition = _ctx.Target.Position;

				if (TargetCanBeAttacked())
				{
					SetState<AttackState>();
					return;
				}
			}
			else
			{
				SetState<CheckState>();
				return;
			}

			if (TryGetTargetPosition(_ctx.Path, out var targetPosition))
			{
				RotateTowards(targetPosition);
				MoveTowards(targetPosition);
			}
		}

		public override void ManualFixedUpdate()
		{
			if (_ctx.Target != null)
				CalculatePath(_ctx.Target.Position, _ctx.Path);
		}
	}
}