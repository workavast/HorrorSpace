using App.Character;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.AI;

namespace App.Mutant
{
	public class MutantContext
	{
		public Transform Transform { get; }
		public readonly Transform LookPoint;
		public readonly MutantMove AgentMove;
		public readonly NavMeshPath Path = new NavMeshPath();
		
		public KinematicCharacterMotor Motor { get; }
		public Vector3 MoveDirection { get; set; }
		public Quaternion TargetRotation { get; set; }
		public float GravityVelocity { get; set; }
		public float TargetSpeed { get; set; }
		public bool IsInAirJump { get; } = false;
		public float DraggedObjMass { get; } = 0;
		
		
		public IMutantTarget Target { get; set; }
		public Vector3 LastSeenPlayerPosition { get; set; }

		public MutantContext(Transform transform, Transform lookPoint, 
			KinematicCharacterMotor motor, MutantMove agentMove)
		{
			Transform = transform;
			Motor = motor;
			AgentMove = agentMove;
			LookPoint = lookPoint ?? transform;
			LastSeenPlayerPosition = transform.position;
		}


	}
}