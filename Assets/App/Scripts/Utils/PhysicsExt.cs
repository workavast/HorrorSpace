using UnityEngine;

namespace App.Utils
{
	public static class PhysicsExt
	{
		public static bool CheckBox(this BoxCollider collider, int layerMask = -5)
		{
			var position = collider.center + collider.transform.position;
			return Physics.CheckBox(position, collider.size / 2, collider.transform.rotation, layerMask);
		}
	}
}