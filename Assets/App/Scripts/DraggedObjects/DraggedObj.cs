using App.ObjectHandling;
using UnityEngine;

namespace App.DraggedObjects
{
	[RequireComponent(typeof(Rigidbody))]
	public class DraggedObj : MonoBehaviour, IDraggable, IHandleObject
	{
		public float Mass => Rb.mass;
		public bool CanBeHandled { get; private set; } = true;
		public bool CanBeDragged { get; private set; } = true;
    
		public Transform Transform => transform;
		public Rigidbody Rb { get; private set; }
    
		private void Awake()
		{
			Rb = GetComponent<Rigidbody>();
		}

		public void OnPickup() { /* Логика при подборе */ }
		public void OnDrop() { /* Логика при сбросе */ }
    
		public void OnDragStart()
		{
			
		}

		public void OnDragEnd()
		{
			
		}
		
		public void SetStateWithRb(bool isDragged)
		{
			CanBeDragged = CanBeHandled = isDragged;
			Rb.isKinematic = !isDragged;
		}
	}
}