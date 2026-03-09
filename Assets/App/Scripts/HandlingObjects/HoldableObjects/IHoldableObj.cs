namespace App.HandlingObjects.HoldableObjects
{
    public interface IHoldableObj : IHandleObject
    {
        public void OnPickup();
    }
}