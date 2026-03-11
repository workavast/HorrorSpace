using System;
using App.Character;
using App.Usable;
using UnityEngine;

namespace App.IsolationBlock
{
	public class IsolationBlockController : MonoBehaviour, IUsable
	{
		[SerializeField] private CellsManager cellsManager;
		[SerializeField] private IsolationTable[] tables;

		private bool CellsIsOpen => !cellsManager.CellsIsClosed;
		public bool IsUsable { get; private set; } = true;
		
		public event Action OnUse;
		public event Action<bool> OnTryUse;
		public event Action OnUsableStateChanged;

		private void Start()
		{
			cellsManager.Initialize();
			
			SetState(false);
		}

		public void Interact(PlayerContext playerContext)
		{
			if (!IsUsable)
			{
				OnTryUse?.Invoke(false);
				return;
			}


			if (!SetState(!CellsIsOpen))
			{
				OnTryUse?.Invoke(false);
				return;
			}
			
			OnUse?.Invoke();
			OnTryUse?.Invoke(true);
			OnUsableStateChanged?.Invoke();
		}

		private bool SetState(bool isOpen)
		{
			if (!isOpen && !cellsManager.AllCellsCanBeClosed())
				return false;
			
			cellsManager.SetState(isOpen);
			foreach (var table in tables) 
				table.SetState(isOpen);
			
			return true;
		}
	}
}