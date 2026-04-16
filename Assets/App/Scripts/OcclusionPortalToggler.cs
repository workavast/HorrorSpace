using App.Doors;
using UnityEngine;

namespace App
{
    public class OcclusionPortalToggler : MonoBehaviour
    {
        [SerializeField] private OcclusionPortal occlusionPortal;
        [SerializeField] private DoorController doorController;
        [SerializeField] private DoorAnimator doorAnimator;

        private void Start()
        {
            doorAnimator.OnOpenStarted += TogglePortal;
            doorAnimator.OnAnimationEnd += TogglePortal;
            TogglePortal();
        }

        private void TogglePortal()
        {
            occlusionPortal.open = doorController.IsOpen;
        }
    }
}