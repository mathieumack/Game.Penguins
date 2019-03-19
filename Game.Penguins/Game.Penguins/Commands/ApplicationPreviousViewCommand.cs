using Game.Penguins.Framework;
using Game.Penguins.ViewModels;

namespace Game.Penguins.Commands
{
    class ApplicationPreviousViewCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        public ApplicationPreviousViewCommand(ApplicationViewModel contextViewModel) : base(contextViewModel)
        {
        }

        public override void Execute(ApplicationViewModel contextViewModel, object parameter)
        {
            if (!(contextViewModel.Content is IApplicationContentView currentView))
                return;

            contextViewModel.Content = currentView.GetPreviousView() as ViewModel;
        }

        public override bool CanExecute(ApplicationViewModel contextViewModel, object parameter)
        {
            if (!(contextViewModel.Content is IApplicationContentView currentView))
                return false;

            return currentView.HasPreviousView;
        }
    }
}
