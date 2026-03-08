using System;
using UnityEngine;

namespace App.Lockers
{
	public class LockerLockView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer handIcon;
		[SerializeField] private Color lockedColor = Color.red;
		[SerializeField] private Color unlockedColor = Color.green;
		
		public void SetState(LockerState state)
		{
			switch (state)
			{
				case LockerState.InactiveLocked:
					handIcon.gameObject.SetActive(false);
					break;
				case LockerState.ActiveLocked:
					handIcon.color = lockedColor;
					handIcon.gameObject.SetActive(true);
					break;
				case LockerState.ActiveUnlocked:
					handIcon.color = unlockedColor;
					handIcon.gameObject.SetActive(true);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}
	}
}