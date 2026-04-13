using VContainer;

namespace App.Levels
{
    public interface IContainerInitializable
    {
        public void Initialize(IObjectResolver container);
    }
}