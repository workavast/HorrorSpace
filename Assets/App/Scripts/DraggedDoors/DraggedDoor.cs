using App.HandlingObjects.DraggedObjects;
using App.InteractiveObjects;
using UnityEngine;

namespace App.DraggedDoors
{
	public class DraggedDoor : MonoBehaviour
	{
		[SerializeField] private DraggedObj draggedObj;
		[SerializeField] private InteractiveObj interactiveObj;
		[SerializeField] private HingeJoint doorHingeJoint;

		private const float AngleError = 0.001f;
		public bool IsClosed ;
		
		public void SetState(bool isDragged)
		{
			draggedObj.SetStateWithRb(isDragged);
			interactiveObj.SetActivityState(isDragged);
		}
		
		public void SetState(bool isDragged, bool isPhysical, bool useSpring)
		{
			draggedObj.SetState(isDragged, isPhysical);
			interactiveObj.SetActivityState(isDragged);
			doorHingeJoint.useSpring = useSpring;
		}

		private void FixedUpdate()
		{
			IsClosed = doorHingeJoint.angle <=  AngleError;
		}
	}
}