using System.Collections.Generic;
using Avastrad.Extensions;
using DCFApixels;
using UnityEngine;

namespace App.Mutant.FSM
{
	public class CheckState : MutantAiState
	{
		public CheckState(MutantContext ctx, MutantParams @params) : base(ctx, @params)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			_ctx.TargetSpeed = _params.ChaseSpeed;
			_ctx.AnimationController.SetIsWalk(false);
			_ctx.AnimationController.SetIsRun(true);
			CalculatePath(_ctx.LastSeenPlayerPosition, _ctx.Path);
		}

		public override void Exit()
		{
			
		}

		public override void ManualUpdate()
		{
			if (CanSeeTarget())
			{
				_ctx.LastSeenPlayerPosition = _ctx.Target.Position;
				SetState<ChaseState>();
				return;
			}

			if (_ctx.Path.corners.Length <= 0)
			{
				SetState<WaitState>();
				return;
			}
			
			DebugX.Draw(Color.blue).Sphere(_ctx.LastSeenPlayerPosition, 0.1f);
			var distanceToCheckPoint = (_ctx.Path.corners[^1] - _ctx.Transform.position).XZ().magnitude;
			if (distanceToCheckPoint < 0.1f)
			{
				SetState<WaitState>();
				return;
			}

			DrawPath(_ctx.Path);

			var targetPosition = GetTargetPosition(_ctx.Path);
			RotateTowards(targetPosition);
			MoveTowards(targetPosition);
		}

		public override void ManualFixedUpdate()
		{
			CalculatePath(_ctx.LastSeenPlayerPosition, _ctx.Path);
		}
	}
}