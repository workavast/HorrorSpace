using App.InteractiveObjects;
using UnityEngine;
using VContainer;

namespace App
{
	public class CursorViewToggler : MonoBehaviour
	{
		[SerializeField] private GameObject commonCursorIcon;
		[SerializeField] private GameObject grabCursorIcon;
		
		[Inject] private readonly InteractionContext _interactionContext;

		private void OnEnable()
		{
			_interactionContext.OnGrabStateChanged += SetState;
			SetState(_interactionContext.IsGrab);
		}

		private void OnDisable()
		{
			_interactionContext.OnGrabStateChanged -= SetState;
		}

		private void SetState(bool isGrabbing)
		{
			commonCursorIcon.SetActive(!isGrabbing);
			grabCursorIcon.SetActive(isGrabbing);
		}
	}
}