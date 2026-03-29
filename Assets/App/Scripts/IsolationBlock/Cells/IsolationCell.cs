using System;
using System.Collections.Generic;
using App.IsolationBlock.Cells.FSM;
using Avastrad.FSM;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace App.IsolationBlock.Cells
{
	public class IsolationCell : MonoBehaviour
	{
		[SerializeField] private CellDoor door;
		[SerializeField] private IsolationCellTriggerZone triggerZone;

		private StateMachine _fsm;
		private CellContext _ctx;
		
		public bool IsClosed => _ctx.IsClosed;
		public bool IsTargetInside { get; private set; }
		public event Action<Target> OnTargetEnter;
		public event Action<Target> OnTargetExit;

		public void Initialize()
		{
			triggerZone.OnEnter += TargetEnter;
			triggerZone.OnExit += TargetExit;
			
			_ctx = new CellContext(door, triggerZone);
			var states = new List<StateBase>()
			{
				new CellIsOpen(_ctx),
				new CellCloseProcess(_ctx),
				new CellIsClosed(_ctx)
			};
			_fsm = new StateMachine(states);
			_fsm.Init<CellCloseProcess>();
		}

		private void OnDestroy()
		{
			triggerZone.OnEnter -= TargetEnter;
			triggerZone.OnExit -= TargetExit;
		}
		
		private void Update()
		{
			_fsm.Update();
		}

		public void SetState(bool isOpen)
		{
			if (isOpen)
				_fsm.SetState<CellIsOpen>();
			else
			{
				if (HasObstacles())
				{
					Debug.LogError("Obstacles detected");
					return;
				}
				
				_fsm.SetState<CellCloseProcess>();
			}
		}
		
		public bool HasObstacles() 
			=> door.HasObstacles();

		private void TargetEnter(Target target)
		{
			IsTargetInside = true;
			OnTargetEnter?.Invoke(target);
		}

		private void TargetExit(Target target)
		{
			IsTargetInside = false;
			OnTargetExit?.Invoke(target);
		}
	}
}