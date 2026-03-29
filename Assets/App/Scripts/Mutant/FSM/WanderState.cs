using System.Collections.Generic;
using Avastrad.Extensions;
using Avastrad.FSM;
using DCFApixels;
using UnityEngine;
using UnityEngine.AI;

namespace App.Mutant.FSM
{
	public class WanderState : MutantAiState
	{
		private Vector3 _point;

		public WanderState(MutantContext ctx, MutantParams @params) : base(ctx, @params)
		{
		}

		public override void Enter(Dictionary<string, object> args)
		{
			_ctx.TargetSpeed = _params.WanderSpeed;
			PickNewWanderPoint();
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

			if (Vector3.Distance(_ctx.Transform.position, _ctx.Path.corners[^1]) <= 0.1f)
			{
				SetState<WaitState>();
				return;
			}
			
			DebugX.Draw(Color.yellow).Sphere(_point, 0.2f);
			DrawPath(_ctx.Path);
			if (Vector3.Distance(_ctx.Transform.position, _point) <= 0.1f)
				PickNewWanderPoint();
			
			var targetPosition = GetTargetPosition(_ctx.Path);
			MoveTowards(targetPosition);
			RotateTowards(targetPosition);
		}

		public override void ManualFixedUpdate()
		{
			CalculatePath(_point, _ctx.Path);
		}
		
		private void PickNewWanderPoint()
		{
			var wanderRadius = _params.WanderRadius;
			var randomPoint = (Random.insideUnitCircle * wanderRadius).X0Y();
			randomPoint += _ctx.Transform.position;

			if (NavMesh.SamplePosition(randomPoint, out var hit, wanderRadius, NavMesh.AllAreas))
			{
				_point = hit.position;
				CalculatePath(_point, _ctx.Path);
			}
		}
	}
}