using Game.Penguins.Framework;
using Game.Penguins.ViewModels;

namespace Game.Penguins.Commands
{
    class ApplicationToggleIsEnabledCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        public ApplicationToggleIsEnabledCommand(ApplicationViewModel contextViewModel) : base(contextViewModel)
        {
        }

        public override void Execute(ApplicationViewModel contextViewModel, object parameter)
        {
            contextViewModel.IsEnabled = !contextViewModel.IsEnabled;
        }
    }
}
