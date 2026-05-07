using System;
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

        public void SetScreen<TScreen>()
        {
            if (_blockCounter.RequestMoreZero)
                return;
            
            var command = _commandsFactory.SetScreen(typeof(TScreen));
            _commandsRepository.ExecuteCommand(command);
        }
        
        public void SetScreen(Type screenType)
        {
            if (_blockCounter.RequestMoreZero)
                return;
            
            var command = _commandsFactory.SetScreen(screenType);
            _commandsRepository.ExecuteCommand(command);
        }

        public void SetScreens(IReadOnlyList<Type> screens)
        {
            if (_blockCounter.RequestMoreZero)
                return;
            
            var command = _commandsFactory.SetScreens(screens);
            _commandsRepository.ExecuteCommand(command);
        }
        
        public void ToggleScreen(Type screenType)
        {
            if (_blockCounter.RequestMoreZero)
                return;
            
            var command = _commandsFactory.ToggleScreen(screenType);
            _commandsRepository.ExecuteCommand(command);
        }

        public void ToggleScreen(Type screenType, bool show)
        {
            if (_blockCounter.RequestMoreZero)
                return;
            
            var command = _commandsFactory.ToggleScreen(screenType, show);
            _commandsRepository.ExecuteCommand(command);
        }

        public TScreen ToggleScreen<TScreen>(bool show)
            where TScreen : ScreenBase
        {
            if (_blockCounter.RequestMoreZero)
                return null;
            
            var screenType = typeof(TScreen);
            
            var command = _commandsFactory.ToggleScreen(screenType, show);
            _commandsRepository.ExecuteCommand(command);
            
            return _screensControllerInternal.GetScreen<TScreen>();
        }

        public void Revert()
        {
            if (_blockCounter.RequestMoreZero)
                return;
            
            _commandsRepository.UndoCommand();
        }
    }
}