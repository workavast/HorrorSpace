using UnityEngine;

namespace App
{
	public class FpsLimiter : MonoBehaviour
	{
		[SerializeField] private int targetFps = 60;
		[SerializeField, Range(-1, 4)] private int vSyncCount = 1;
		
		private void Start()
		{
			Application.targetFrameRate = targetFps;

			if (vSyncCount < 0)
				return;
			
			QualitySettings.vSyncCount = vSyncCount;
		}
	}
}