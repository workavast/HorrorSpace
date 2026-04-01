using UnityEngine;

namespace App.Mutant
{
	public class MutantAttentionZone : MonoBehaviour
	{
		[SerializeField] private InterfaceReference<IMutantTarget> mutantTarget;
		[SerializeField] private MutantTriggerZone attentionZone;

		private void Awake() 
			=> attentionZone.OnEnter += AttractAttention;

		private void OnDestroy() 
			=> attentionZone.OnEnter -= AttractAttention;

		private void AttractAttention(MutantController mutant) 
			=> mutant.AttractAttention(mutantTarget.Value);
	}
}