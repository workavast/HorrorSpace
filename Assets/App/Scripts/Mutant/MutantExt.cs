using UnityEngine;

namespace App.Mutant
{
	public static class MutantExt
	{
		public static bool CanSeeTarget(IMutantTarget target, MutantContext ctx, MutantParams @params, bool checkAngle)
		{
			if (target == null)
				return false;

			var targetMidPoint = target.MiddlePoint;
			var toTarget = targetMidPoint - ctx.LookPoint.position;

			var distance = toTarget.magnitude;
			if (distance > @params.ViewDistance)
				return false;

			if (target.Type == MutantTargetType.Smell)
				return true;
			
			if (checkAngle)
			{
				var angle = Vector3.Angle(ctx.Transform.forward, toTarget);
				if (angle > @params.ViewAngle * 0.5f)
					return false;
			}
			if (Physics.Linecast(ctx.LookPoint.position, targetMidPoint, out var hitInfo, @params.ViewLayerMask))
				if (hitInfo.collider.TryGetComponent(out IMutantTarget hitTarget) && hitTarget == target)
					return true;

			return false;
		}
	}
}