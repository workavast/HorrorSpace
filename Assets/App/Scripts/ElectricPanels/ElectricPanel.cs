using System;
using App.KeySystem;
using UnityEngine;

namespace App.ElectricPanels
{
	public class ElectricPanel : MonoBehaviour
	{
		[SerializeField] private Transform fuseHoldPoint;
		[SerializeField] private KeyReader keyReader;

		public event Action OnFix;
		
		private void Awake()
		{
			keyReader.OnValidKey += OnFuseEnter;
		}

		private void OnFuseEnter(IKeyObject obj)
		{
			var fuse = obj as Fuse;

			if (fuse == null)
			{
				Debug.LogError($"Cant cast to Fuse: {obj.GetType()}");
				return;
			}
			
			keyReader.SetActivityState(false);
			
			fuse.SetState(false);
			fuse.transform.SetPositionAndRotation(fuseHoldPoint.position, fuseHoldPoint.rotation);
			OnFix?.Invoke();
		}
	}
}