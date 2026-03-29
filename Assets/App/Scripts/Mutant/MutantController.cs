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
		[SerializeField] private MutantTriggerZone mutantTriggerZone;
	
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
			if (_ctx.Target != null)
				return;

			var candidate = mutantTriggerZone.CurrentTarget;
			if (candidate == null)
				return;

			if (MutantExt.CanSeeTarget(candidate, _ctx, mutantParams, true))
			{
				_ctx.Target = candidate;
				_ctx.LastSeenPlayerPosition = candidate.MiddlePoint;
			}
		}

		private void Update()
		{
			TryAcquireTargetFromZone();
			_fsm.Update();
		}

		private void FixedUpdate() 
			=> _fsm.FixedUpdate();
	}
}