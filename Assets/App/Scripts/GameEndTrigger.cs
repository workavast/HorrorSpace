using App.Usable;
using Avastrad.ScenesLoading;
using Avastrad.UI.UiSystem;
using Avastrad.UI.UiSystem.Ids;
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

		private UiRoot _uiRoot;
		
		[Inject]
		public void Construct(UiRoot uiRoot)
		{
			_uiRoot = uiRoot;
		}
		
		private void Awake()
		{
			usableObj.OnUse += EndGame;
		}

		private void EndGame()
		{
			_uiRoot.Set(screenId);
		}
	}
}