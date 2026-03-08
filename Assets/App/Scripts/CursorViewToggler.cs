using UnityEngine;

namespace App
{
	public class CursorViewToggler : MonoBehaviour
	{
		[SerializeField] private GameObject commonCursorIcon;
		[SerializeField] private GameObject grabCursorIcon;
		
		public void SetState(bool isGrabbing)
		{
			commonCursorIcon.SetActive(!isGrabbing);
			grabCursorIcon.SetActive(isGrabbing);
		}
	}
}