using App.EscProviding;
using Avastrad.UI.UiSystem;
using VContainer;

namespace App.UI.GameplayMenu
{
    public class GameplayMenuScreen : ScreenBase, IEscListener
    {
        [Inject] private UiRoot _uiRoot;
        [Inject] private readonly EscProvider _escProvider;

        private void OnEnable() 
            => _escProvider.Sub(this);

        private void OnDisable() 
            => _escProvider.UnSub(this);

        public void OnEscPressed() 
            => _uiRoot.Undo();
    }
}