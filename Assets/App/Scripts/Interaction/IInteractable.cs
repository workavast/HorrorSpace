using App.Character;

namespace App.Interaction
{
    public interface IInteractable
    {
        public bool IsInteractable { get; }
        
        public void Interact(PlayerContext playerContext);
    }
}
