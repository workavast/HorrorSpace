using App.KeySystem;
using UnityEngine;

namespace App.ElectricPanels
{
	public class ElectricPanel : MonoBehaviour
	{
		[SerializeField] private Transform fuseHoldPoint;
		[SerializeField] private KeyObjLockProvider lockProvider;

		private void Awake()
		{
			lockProvider.OnUnlocked += OnFuseEnter;
		}

		private void OnFuseEnter(IKeyObject obj)
		{
			var fuse = obj as Fuse;

			if (fuse == null)
			{
				Debug.LogError($"Cant cast to Fuse: {obj.GetType()}");
				return;
			}
			
			lockProvider.SetActivityState(false);
			fuse.SetState(false);
			fuse.transform.SetPositionAndRotation(fuseHoldPoint.position, fuseHoldPoint.rotation);
		}
	}
}