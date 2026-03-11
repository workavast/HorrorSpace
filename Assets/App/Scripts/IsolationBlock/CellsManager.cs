using System.Linq;
using App.IsolationBlock.Cells;
using UnityEngine;

namespace App.IsolationBlock
{
	public class CellsManager : MonoBehaviour
	{
		[SerializeField] private IsolationCell[] cells;
		
		public bool CellsIsClosed => cells.All(cell => cell.IsClosed);
		
		public void Initialize()
		{
			foreach (var cell in cells)
				cell.Initialize();
		}

		public bool AllCellsCanBeClosed()
		{
			foreach (var cell in cells)
				if (cell.HasObstacles())
					return false;
			
			return true;
		}
		
		public void SetState(bool isOpen)
		{
			foreach (var cell in cells)
				cell.SetState(isOpen);
		}
	}
}