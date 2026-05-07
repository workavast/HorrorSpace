using System.Collections.Generic;
using UnityEngine;

namespace Avastrad.UI.UiSystem.Commands
{
    internal class SetScreensCommand : ICommand
    {
        private readonly IReadOnlyList<string> _targetScreens;
        private readonly ScreensControllerInternal _screensController;

        private IReadOnlyList<string> _lastActiveScreens;

        public bool IsExecuted { get; private set; }

        public SetScreensCommand(IReadOnlyList<string> targetScreens, ScreensControllerInternal screensController)
        {
            _targetScreens = targetScreens;
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
            _screensController.SetScreens(_targetScreens);
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