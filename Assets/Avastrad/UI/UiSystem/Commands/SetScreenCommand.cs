using System.Collections.Generic;
using UnityEngine;

namespace Avastrad.UI.UiSystem.Commands
{
    internal class SetScreenCommand : ICommand
    {
        private readonly string _targetScreenId;
        private readonly ScreensControllerInternal _screensController;

        private IReadOnlyList<string> _lastActiveScreens;

        public bool IsExecuted { get; private set; }

        public SetScreenCommand(string targetScreenId, ScreensControllerInternal screensController)
        {
            _targetScreenId = targetScreenId;
            _screensController = screensController;
        }

        public void Execute()
        {
            if (IsExecuted)
            {
                Debug.LogError("Command is already executed");
                return;
            }

            IsExecuted = true;
            _lastActiveScreens = new List<string>(_screensController.GetActiveScreens());
            _screensController.SetScreen(_targetScreenId);
        }

        public void Undo()
        {
            if (!IsExecuted)
            {
                Debug.LogError("Command is not executed");
                return;
            }

            IsExecuted = false;
            _screensController.SetScreens(_lastActiveScreens);
        }
    }
}