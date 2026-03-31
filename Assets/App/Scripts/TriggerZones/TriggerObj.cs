using UnityEngine;

namespace App.TriggerZones
{
	public class TriggerObj : MonoBehaviour
	{
		[SerializeField] private GameObject owner;
		
		public GameObject Owner => owner;
	}
}