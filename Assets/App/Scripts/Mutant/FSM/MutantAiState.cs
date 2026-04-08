using Avastrad.Extensions;
using Avastrad.FSM;
using DCFApixels;
using UnityEngine;
using UnityEngine.AI;

namespace App.Mutant.FSM
{
	public abstract class MutantAiState : StateBase
	{
		protected readonly MutantContext _ctx;
		protected readonly MutantParams _params;

		protected MutantAiState(MutantContext ctx, MutantParams @params)
		{
			_ctx = ctx;
			_params = @params;
		}

		protected bool CanSeeTarget()
		{
			if (_ctx.Target == null)
				return false;

			if (!MutantExt.CanSeeTarget(_ctx.Target, _ctx, _params, false))
			{
				_ctx.Target = null;
				return false;
			}

			return true;
		}

		protected bool TargetCanBeAttacked()
		{
			if (_ctx.Target == null)
				return false;

			var distance = Vector3.Distance(_ctx.Transform.position, _ctx.Target.Position);
			if (distance > _params.AttackDistance)
				return false;
			
			return !Physics.Linecast(_ctx.LookPoint.position, _ctx.Target.MiddlePoint, _params.AttackObstaclesLayerMask);
		}

		protected void CalculatePath(Vector3 targetPosition, NavMeshPath path) 
			=> NavMesh.CalculatePath(_ctx.Transform.position, targetPosition, NavMesh.AllAreas, path);

		protected static bool TryGetTargetPosition(NavMeshPath path, out Vector3 targetPos)
		{
			if (path.corners.Length <= 0)
			{
				targetPos = default;
				return false;
			}
			
			targetPos = path.corners[0];
			DebugX.Draw(Color.red).Sphere(targetPos, 0.25f);
			if (path.corners.Length > 1)
			{
				targetPos = path.corners[1];
				DebugX.Draw(Color.red).Line(targetPos, path.corners[0]);
				DebugX.Draw(Color.blue).Sphere(targetPos, 0.25f);
			}
			
			return true;
		}

		protected void DrawPath(NavMeshPath path)
		{
			for (var i = 0; i < path.corners.Length - 1; i++)
			{
				DebugX.Draw(Color.black).Sphere(path.corners[i], 0.2f);
				DebugX.Draw(Color.red).Line(path.corners[i], path.corners[i + 1]);
			}
			DebugX.Draw(Color.black).Sphere(path.corners[^1], 0.2f);
		}
		
		protected void MoveTowards(Vector3 targetPosition)
		{
			var direction = (targetPosition - _ctx.Transform.position).X0Z().normalized;
			_ctx.MoveDirection = direction;
		}
		
		protected void RotateTowards(Vector3 targetPosition)
		{
			var direction = (targetPosition - _ctx.Transform.position).X0Z().normalized;
			var targetRotation = Quaternion.LookRotation(direction);
			_ctx.TargetRotation = targetRotation;
		}

		protected void StopMovement()
		{
			_ctx.MoveDirection = Vector3.zero;
			_ctx.Path.ClearCorners();
		}
	}
}