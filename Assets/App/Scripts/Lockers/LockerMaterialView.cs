using System;
using UnityEngine;

namespace App.Lockers
{
	public class LockerMaterialView : MonoBehaviour
	{
		[SerializeField] private MeshRenderer[] meshRenderers;
		[SerializeField] private Material inactiveMat;
		[SerializeField] private Material lockedMat;
		[SerializeField] private Material unlockedMat;
		
		public void SetState(LockerState state)
		{
			var material = state switch
			{
				LockerState.Inactive => inactiveMat,
				LockerState.Locked => lockedMat,
				LockerState.Unlocked => unlockedMat,
				_ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
			};
			
			foreach (var meshRenderer in meshRenderers) 
				meshRenderer.material = material;
		}
	}
}