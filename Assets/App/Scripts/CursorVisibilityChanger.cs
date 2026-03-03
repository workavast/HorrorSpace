using App.EscProviding;
using UnityEngine;

namespace App
{
	public class CursorVisibilityChanger : MonoBehaviour, IEscListener
	{
		[SerializeField] private EscProvider escProvider;

		private void Awake()
		{
			escProvider.Sub(this);
			OnEscPressed();
		}

		private void OnDestroy()
		{
			escProvider.UnSub(this);
		}

		public void OnEscPressed()
		{
			Cursor.visible = !Cursor.visible;
			Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}
}