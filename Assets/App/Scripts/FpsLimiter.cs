using UnityEngine;

namespace App
{
	public class FpsLimiter : MonoBehaviour
	{
		private void Start()
		{
			Application.targetFrameRate = 60;
		}
	}
}