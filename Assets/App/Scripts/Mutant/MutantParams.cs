using System;
using UnityEngine;

namespace App.Mutant
{
	[Serializable]
	public class MutantParams
	{
		[Header("Movement")]
		[SerializeField] private float chaseSpeed = 5f;
		[SerializeField] private float wanderSpeed = 2f;
		public AnimationCurve velocityModifierPerGrabObjMass;
		public float acceleration;
		public float deceleration;
		public float airDrag;
		public float airAcceleration;
		[Header("View")]
		[SerializeField] private float viewDistance = 20f;
		[SerializeField] private float viewAngle = 90f;
		[SerializeField] private LayerMask vewLayerMask = ~1;
		[Header("Other")]
		[SerializeField] private float wanderRadius = 10f;
		[SerializeField] private float attackDistance = 2f;
		[SerializeField] private float waitTime = 5f;
		[SerializeField] private LayerMask attackObstaclesLayerMask = ~1;

		public float ViewDistance => viewDistance;
		public float ViewAngle => viewAngle;
		public LayerMask ViewLayerMask => vewLayerMask;
		public LayerMask AttackObstaclesLayerMask => attackObstaclesLayerMask;

		public float AttackDistance => attackDistance;
		public float ChaseSpeed => chaseSpeed;
		public float WanderSpeed => wanderSpeed;
		public float WaitTime  => waitTime;
		public float WanderRadius => wanderRadius;

		public AnimationCurve VelocityModifierPerGrabObjMass => velocityModifierPerGrabObjMass;
		public float Acceleration => acceleration;
		public float Deceleration => deceleration;
		public float AirDrag => airDrag;
		public float AirAcceleration => airAcceleration;
	}
}