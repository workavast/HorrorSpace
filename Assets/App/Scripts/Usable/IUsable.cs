using App.Character;

namespace App.Usable
{
    public interface IUsable
    {
        public bool IsInteractable { get; }
        
        public void Interact(PlayerContext playerContext);
    }
}
