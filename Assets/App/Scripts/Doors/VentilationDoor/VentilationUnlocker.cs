using UnityEngine;

namespace App.Doors.VentilationDoor
{
    public class VentilationUnlocker : MonoBehaviour
    {
        [SerializeField] private DoorBtn activeBtn;
        [SerializeField] private DoorBtn lockedBtn;
        
        private void Awake()
        {
            activeBtn.OnUse += Unlock;
        }

        private void OnDestroy()
        {
            activeBtn.OnUse -= Unlock;
        }

        private void Unlock()
        {
            lockedBtn.SetSelfInteractable(true);
            activeBtn.OnUse -= Unlock;
        }
    }
}