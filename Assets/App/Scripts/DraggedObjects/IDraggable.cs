namespace App.DraggedObjects
{
    /// <summary>
    /// Interface for objects that can be dragged by the player.
    /// Objects implementing this interface can be picked up and dragged
    /// differently from regular held objects.
    /// </summary>
    public interface IDraggable
    {
        public float Mass { get; }
        
        /// <summary>
        /// Determines whether this object can be dragged.
        /// </summary>
        public bool CanBeDragged { get; }

        /// <summary>
        /// Called when the object starts being dragged.
        /// </summary>
        public void OnDragStart();

        /// <summary>
        /// Called when the object stops being dragged.
        /// </summary>
        public void OnDragEnd();
    }
}
