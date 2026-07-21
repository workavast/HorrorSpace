using App.Usable;
using Avastrad.ScenesLoading;
using Avastrad.UI.UiSystem;
using Avastrad.Utils.ScenesReferencing;
using UnityEngine;
using VContainer;

namespace App
{
	public class GameEndTrigger : MonoBehaviour
	{
		[SerializeField] private ScreenIdReference screenId;
		[SerializeField] private LoadingConfig loadingConfig;
		[SerializeField] private SceneReference sceneRef;
		[SerializeField] private UsableObj usableObj;

		private ScreensController _screensController;
		
		[Inject]
		public void Construct(ScreensController screensController)
		{
			_screensController = screensController;
		}
		
		private void Awake()
		{
			usableObj.OnUse += EndGame;
		}

		private void EndGame()
		{
			_screensController.SetScreen(screenId);
		}
	}
}