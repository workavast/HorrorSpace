using App.Usable;
using Avastrad.ScenesLoading;
using Avastrad.Utils.ScenesReferencing;
using UnityEngine;
using VContainer;

namespace App.SecurityCheckpoint
{
	public class GameEndTrigger : MonoBehaviour
	{
		[SerializeField] private SceneReference sceneRef;
		[SerializeField] private UsableObj usableObj;

		private ISceneLoader _sceneLoader;
		
		public void Initialize(IObjectResolver container)
		{
			_sceneLoader = container.Resolve<ISceneLoader>();
		}
		
		private void Awake()
		{
			usableObj.OnUse += EndGame;
		}

		private void EndGame()
		{
			Debug.Log("Game ended");
			_sceneLoader.LoadScene(sceneRef.SceneIndex, 1);
		}
	}
}