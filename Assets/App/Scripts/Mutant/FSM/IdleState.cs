using System.Collections.Generic;
using Avastrad.FSM;
using UnityEngine;

namespace App.Mutant.FSM
{
	public class IdleState : MutantAiState
	{
		public IdleState(MutantContext ctx, MutantParams @params) : base(ctx, @params)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			StopMovement();
			_ctx.AnimationController.SetIsWalk(false);
			_ctx.AnimationController.SetIsRun(false);
		}

		public override void Exit()
		{
		}

		public override void ManualUpdate()
		{
			if (CanSeeTarget())
			{
				Debug.Log("Can see target");
				_ctx.LastSeenPlayerPosition = _ctx.Target.Position;
				SetState<ChaseState>();
			}
		}

		public override void ManualFixedUpdate()
		{
		}
	}
}