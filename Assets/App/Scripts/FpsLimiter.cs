using UnityEngine;

namespace App
{
	public class FpsLimiter : MonoBehaviour
	{
		[SerializeField] private int targetFps = 60;
		[SerializeField] private int vSyncCount = 1;
		
		private void Start()
		{
			Application.targetFrameRate = targetFps;
			QualitySettings.vSyncCount = vSyncCount;
		}
	}
}