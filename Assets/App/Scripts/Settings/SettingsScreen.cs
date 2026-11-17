using App.EscProviding;
using Avastrad.Settings;
using Avastrad.UI.UiSystem;
using UnityEngine;
using VContainer;

namespace App.Settings
{
    public class SettingsScreen : ScreenBase, IEscListener
    {
        [SerializeField] private SettingsWindow settingsWindow;
        
        [Inject] private readonly SettingsRepository _settingsRepository;
        [Inject] private UiRoot _uiRoot;
        [Inject] private readonly EscProvider _escProvider;
        
        public override void Initialize()
        {
            settingsWindow.Initialize(_settingsRepository);
        }

        protected override void OnEnable() 
            => _escProvider.Sub(this);

        protected override void OnDisable() 
            => _escProvider.UnSub(this);

        public void OnEscPressed() 
            => _uiRoot.Undo();
    }
}