using System.Collections.Generic;
using Avastrad.CustomTimer;
using Avastrad.FSM;
using UnityEngine;

namespace App.Mutant.FSM
{
	public class WaitState : MutantAiState
	{
		private readonly Timer _waitTimer;
		
		public WaitState(MutantContext ctx, MutantParams @params) : base(ctx, @params)
		{
			_waitTimer = new Timer(@params.WaitTime);
		}

		public override void Enter(Dictionary<string, object> args)
		{
			StopMovement();
			_waitTimer.SetMaxTime(_params.WaitTime);
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

			_waitTimer.Tick(Time.deltaTime);
			if (_waitTimer.TimerIsEnd)
			{
				SetState<WanderState>();
				return;
			}
		}

		public override void ManualFixedUpdate()
		{
		}
	}
}