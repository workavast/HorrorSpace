using System.Collections.Generic;
using App.Character;
using App.Mutant.FSM;
using App.TriggerZones;
using App.Utils.Debug;
using Avastrad.FSM;
using KinematicCharacterController;
using UnityEngine;

namespace App.Mutant
{
	public class MutantController : MonoBehaviour
	{
		[SerializeField] private MutantMove agentMove;
		[SerializeField] private KinematicCharacterMotor motor;
		
		[SerializeField] private MutantParams mutantParams;
		[SerializeField] private Transform lookPoint;
		[SerializeField] private MutantTargetTriggerZone mutantTriggerZone;
	
		public ReadOnlyQueue<string> DebugQueue = new(8);
		
		private MutantContext _ctx;
		private StateMachine _fsm;

		private void Awake()
		{
			Initialize();
			agentMove.Initialize(_ctx, mutantParams);
		}

		public void Initialize()
		{
			_ctx = new MutantContext(transform, lookPoint, motor, agentMove);
			_ctx.TargetSpeed = mutantParams.ChaseSpeed;

			var states = new List<StateBase>
			{
				new IdleState(_ctx, mutantParams),
				new ChaseState(_ctx, mutantParams),
				new AttackState(_ctx, mutantParams),
				new CheckState(_ctx, mutantParams),
				new WaitState(_ctx, mutantParams),
				new WanderState(_ctx, mutantParams),
			};
			
			_fsm = new StateMachine(states);
			_fsm.OnStateSwitched += type => DebugQueue.Enqueue(type.Name);
			_fsm.Init<IdleState>();
		}
		
		private void TryAcquireTargetFromZone()
		{
			var candidate = mutantTriggerZone.CurrentTarget;
			if (candidate == null)
				return;

			if (_ctx.Target != null && _ctx.Target.Priority >= candidate.Priority)
				return;

			if (MutantExt.CanSeeTarget(candidate, _ctx, mutantParams, true))
			{
				_ctx.Target = candidate;
				_ctx.LastSeenPlayerPosition = candidate.MiddlePoint;
			}
		}

		public void AttractAttention(IMutantTarget target)
		{
			_ctx.AttractAttentionTarget = target;
		}
		
		private void Update()
		{
			TryAcquireTargetFromZone();
			TryAcquireTargetFromAttention();
			_fsm.Update();
		}

		private void TryAcquireTargetFromAttention()
		{
			if (_ctx.AttractAttentionTarget != null && (_ctx.Target == null || _ctx.AttractAttentionTarget.Priority > _ctx.Target.Priority))
				_ctx.Target = _ctx.AttractAttentionTarget;
		}

		private void FixedUpdate() 
			=> _fsm.FixedUpdate();
	}
}