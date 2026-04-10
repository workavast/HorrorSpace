using System.Collections.Generic;
using App.Health;

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
			_ctx.AnimationController.StopMovement();
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

				TryAttack();
			}
		}

		public override void ManualFixedUpdate()
		{
		}

		private void TryAttack()
		{
			if(_ctx.Target == null)
				return;
			
			if (_ctx.AnimationController.AttackAnimationIsPlaing())
				return;

			if (_ctx.Target.gameObject.TryGetComponent(out IDamageable damageable))
			{
				if (!damageable.CanBeDamaged())
					return;
				
				damageable.TakeDamage(_params.Damage);
				_ctx.AnimationController.TriggerAttack();
			}
		}
	}
}