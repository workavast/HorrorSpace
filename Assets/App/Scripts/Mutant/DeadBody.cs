using UnityEngine;

namespace App.Mutant
{
	public class DeadBody : MonoBehaviour, IMutantTarget
	{
		[SerializeField] private Transform chestTransform;

		public int Priority => 0;
		public MutantTargetType Type => MutantTargetType.Smell;
		public Vector3 Position => chestTransform.position;
		public Vector3 MiddlePoint => chestTransform.position;
	}
}