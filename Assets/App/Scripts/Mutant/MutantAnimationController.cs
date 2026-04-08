using UnityEngine;

namespace App.Mutant
{
	public class MutantAnimationController : MonoBehaviour
	{
		[SerializeField] private Animator animator;
		[SerializeField] private string isWalkParam = "IsWalk";
		[SerializeField] private string attackTriggerParam = "Attack";
		[SerializeField] private string isRunParam = "IsRun";

		public void SetIsWalk(bool isWalking) 
			=> animator.SetBool(isWalkParam, isWalking);

		public void TriggerAttack() 
			=> animator.SetTrigger(attackTriggerParam);

		public void SetIsRun(bool isRunning) 
			=> animator.SetBool(isRunParam, isRunning);
	}
}