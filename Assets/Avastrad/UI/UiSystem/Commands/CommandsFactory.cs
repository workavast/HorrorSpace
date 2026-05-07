using System.Collections.Generic;

namespace Avastrad.UI.UiSystem.Commands
{
    internal class CommandsFactory
    {
        private readonly ScreensControllerInternal _screensController;

        public CommandsFactory(ScreensControllerInternal screensController)
            => _screensController = screensController;

        public ICommand SetScreen(string screenId)
            => new SetScreenCommand(screenId, _screensController);

        public ICommand ToggleScreen(string screenId)
            => new ToggleScreenCommand(screenId, _screensController, ToggleType.Auto);

        public ICommand SetScreens(IReadOnlyList<string> screens)
            => new SetScreensCommand(screens, _screensController);

        public ICommand ToggleScreen(string screenId, bool show)
        {
            var toggleType = show ? ToggleType.Show : ToggleType.Hide;
            return new ToggleScreenCommand(screenId, _screensController, toggleType);
        }
    }
}