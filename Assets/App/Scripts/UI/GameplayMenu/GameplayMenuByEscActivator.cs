using App.EscProviding;
using Avastrad.UI.UiSystem;
using Avastrad.UI.UiSystem.Ids;
using UnityEngine;
using VContainer;

namespace App.UI.GameplayMenu
{
    public class GameplayMenuByEscActivator : MonoBehaviour, IEscListener
    {
        [SerializeField] private ScreenIdReference screenId;
        [SerializeField] private ScreenIdReference gameEndScreenId;
        
        private EscProvider _escProvider;
        private UiRoot _uiRoot;

        [Inject]
        public void Construct(EscProvider escProvider, UiRoot uiRoot)
        {
            _escProvider = escProvider;
            _uiRoot = uiRoot;
        }

        private void Start() 
            => _escProvider.Sub(this);

        private void OnDestroy() 
            => _escProvider?.UnSub(this);

        public void OnEscPressed()
        {
            if (_uiRoot.TryGetActive<GameEndedScreen>(gameEndScreenId, out _))
                return;

            _uiRoot.Set(screenId);
        }
    }
}