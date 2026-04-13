using App.Levels;
using UnityEngine;
using VContainer;

namespace App.IsolationBlock
{
	public class IsolationBlockController : MonoBehaviour, IContainerInitializable
	{
		[SerializeField] private CellsController cellsController;
		[SerializeField] private CellsTargetChecker cellsTargetChecker;

		public void Initialize(IObjectResolver container)
		{
			cellsController.Initialize();
			cellsTargetChecker.Initialize(container);
		}
	}
}