using System;
using App.Character;
using App.DraggedDoors;
using App.Usable;
using UnityEngine;

namespace App.IsolationBlock
{
	public class IsolationBlockController : MonoBehaviour, IUsable
	{
		[SerializeField] private DraggedDoor[] draggedDoors;

		public bool IsUsable { get; private set; } = true;
		
		public event Action OnUse;
		public event Action<bool> OnTryUse;
		public event Action OnUsableStateChanged;

		private void Start()
		{
			foreach (var draggedDoor in draggedDoors)
				draggedDoor.SetState(false);
		}

		public void Interact(PlayerContext playerContext)
		{
			if (!IsUsable)
			{
				OnTryUse?.Invoke(false);
				return;
			}
			
			foreach (var draggedDoor in draggedDoors)
				draggedDoor.SetState(true);
			
			IsUsable = false;
			OnUse?.Invoke();
			OnTryUse?.Invoke(true);
			OnUsableStateChanged?.Invoke();
		}
	}
}