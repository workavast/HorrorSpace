using UnityEngine;

namespace App.Mutant
{
	public class MutantAnimationController : MonoBehaviour
	{
		[SerializeField] private Animator animator;
		[SerializeField] private string isWalkParam = "IsWalk";
		[SerializeField] private string attackTriggerParam = "Attack";
		[SerializeField] private string isRunParam = "IsRun";
		[SerializeField] private string attackAnimationTag = "Attack";

		public void StopMovement()
		{
			animator.SetBool(isWalkParam, false);
			animator.SetBool(isRunParam, false);
		}
		
		public void SetIsWalk(bool isWalking)
		{
			animator.SetBool(isWalkParam, isWalking);
			
			if (isWalking)
				animator.SetBool(isRunParam, false);
		}

		public void TriggerAttack() 
			=> animator.SetTrigger(attackTriggerParam);

		public void SetIsRun(bool isRunning)
		{
			animator.SetBool(isRunParam, isRunning);
			
			if (isRunning)
				animator.SetBool(isWalkParam, false);
		}

		public bool AttackAnimationIsPlaing()
		{
			return animator.GetNextAnimatorStateInfo(1).IsTag(attackAnimationTag) ||
			       animator.GetCurrentAnimatorStateInfo(1).IsTag(attackAnimationTag);
		}
	}
}