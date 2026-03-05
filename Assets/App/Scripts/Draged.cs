using App.ObjectHandling;
using UnityEngine;

namespace App
{
	public class Draged : MonoBehaviour, IDraggable, IHandleObject
	{
		public bool CanBeHandled => true;
		public bool CanBeDragged => true; // Этот объект можно перетягивать
    
		public Transform Transform => transform;
		public Rigidbody Rb { get; private set; }
    
		private void Start()
		{
			Rb = GetComponent<Rigidbody>();
		}

		public void OnPickup() { /* Логика при подборе */ }
		public void OnDrop() { /* Логика при сбросе */ }
    
		public void OnDragStart()
		{
			// Вызывается в начале перетягивания
			// Например: отключить частицы, звуки и т.д.
		}

		public void OnDragEnd()
		{
			// Вызывается в конце перетягивания
			// Например: включить эффекты обычного поведения
		}
	}
}