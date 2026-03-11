using UnityEngine;

namespace App.IsolationBlock
{
	public class IsolationTable : MonoBehaviour
	{
		[SerializeField] private GameObject openCellsMessage;
		[SerializeField] private GameObject closeCellsMessage;
		
		public void SetState(bool isOpen)
		{
			openCellsMessage.SetActive(isOpen);
			closeCellsMessage.SetActive(!isOpen);
		}
	}
}