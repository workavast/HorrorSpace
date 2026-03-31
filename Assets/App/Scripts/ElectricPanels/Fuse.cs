using App.HandlingObjects.HoldableObjects;
using App.InteractiveObjects;
using App.KeySystem;
using UnityEngine;

namespace App.ElectricPanels
{
	public class Fuse : KeyObject
	{
		[SerializeField] private Rigidbody rb;
		[SerializeField] private InteractiveObj interactiveObj;
		[SerializeField] private HoldableObj holdableObj;
		
		public void SetState(bool isInteractive)
		{
			interactiveObj.SetActivityState(isInteractive);
			holdableObj.SetState(isInteractive);
			rb.isKinematic = !isInteractive;
		}
	}
}