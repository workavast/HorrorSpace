namespace App.HandlingObjects.DraggedObjects
{
    public interface IDraggable : IHandleObject
    {
        public float Mass { get; }
        
        public bool CanBeDragged { get; }
    }
}
