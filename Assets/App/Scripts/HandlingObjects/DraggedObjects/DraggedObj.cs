using UnityEngine;

namespace App.HandlingObjects.DraggedObjects
{
	[RequireComponent(typeof(Rigidbody))]
	public class DraggedObj : MonoBehaviour, IDraggable
	{
		public bool CanBeHandled { get; private set; } = true;
		public bool CanBeDragged { get; private set; } = true;
		public float Mass => Rb.mass;
    
		public Transform Transform => transform;
		public Rigidbody Rb { get; private set; }
    
		private void Awake()
		{
			Rb = GetComponent<Rigidbody>();
		}

		public void Drop() { }
    
		public void SetStateWithRb(bool isDragged)
		{
			CanBeDragged = CanBeHandled = isDragged;
			Rb.isKinematic = !isDragged;
		}
		
		public void SetState(bool isDragged, bool isPhysical)
		{
			CanBeDragged = CanBeHandled = isDragged;
			Rb.isKinematic = !isPhysical;
		}
	}
}