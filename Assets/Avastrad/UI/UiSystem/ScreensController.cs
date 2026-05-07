using System.Collections.Generic;
using Avastrad.UI.UiSystem.Commands;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    [DisallowMultipleComponent]
    public class ScreensController : MonoBehaviour
    {
        private readonly CommandsRepository _commandsRepository = new(8, 8);
        private ScreensControllerInternal _screensControllerInternal;
        private CommandsFactory _commandsFactory;

        private readonly RequestCounter _blockCounter = new();
        
        private void Awake()
        {
            _screensControllerInternal = GetComponentInChildren<ScreensControllerInternal>();
            _screensControllerInternal.Initialize();
            
            _commandsFactory = new CommandsFactory(_screensControllerInternal);
        }

        public void BlockRequest(bool block)
        {
            if (block)
                _blockCounter.AddRequest();
            else
                _blockCounter.RemoveRequest();
        }

        private bool IsRequestBlocked(string screenId) 
            => _blockCounter.RequestMoreZero || string.IsNullOrEmpty(screenId);
        
        private bool IsRequestBlocked(IReadOnlyList<string> screenIds) 
            => _blockCounter.RequestMoreZero || screenIds == null || screenIds.Count == 0;

        public void SetScreen(ScreenIdReference screenIdReference) 
            => SetScreen(screenIdReference.Id);

        public void SetScreen(string screenId)
        {
            if (IsRequestBlocked(screenId))
                return;
            
            var command = _commandsFactory.SetScreen(screenId);
            _commandsRepository.ExecuteCommand(command);
        }

        public void SetScreens(IReadOnlyList<string> screenIds)
        {
            if (IsRequestBlocked(screenIds))
                return;
            
            var command = _commandsFactory.SetScreens(screenIds);
            _commandsRepository.ExecuteCommand(command);
        }
        
        public void ToggleScreen(ScreenIdReference screenIdReference)
            => ToggleScreen(screenIdReference.Id);
        
        public void ToggleScreen(string screenId)
        {
            if (IsRequestBlocked(screenId))
                return;
            
            var command = _commandsFactory.ToggleScreen(screenId);
            _commandsRepository.ExecuteCommand(command);
        }

        public void ToggleScreen(ScreenIdReference screenIdReference, bool show)
            => ToggleScreen(screenIdReference.Id, show);
        
        public void ToggleScreen(string screenId, bool show)
        {
            if (IsRequestBlocked(screenId))
                return;
            
            var command = _commandsFactory.ToggleScreen(screenId, show);
            _commandsRepository.ExecuteCommand(command);
        }

        public TScreen ToggleScreen<TScreen>(string screenId, bool show)
            where TScreen : ScreenBase
        {
            if (IsRequestBlocked(screenId))
                return null;
            
            var screen = _screensControllerInternal.GetScreen(screenId) as TScreen;
            if (screen == null)
            {
                Debug.LogError($"Screen {screenId} invalid or not found");
                return null;
            }
            
            var command = _commandsFactory.ToggleScreen(screenId, show);
            _commandsRepository.ExecuteCommand(command);
            
            return _screensControllerInternal.GetScreen(screenId) as TScreen;
        }
        
        public void Revert()
        {
            if (_blockCounter.RequestMoreZero)
                return;
            
            _commandsRepository.UndoCommand();
        }
    }
}