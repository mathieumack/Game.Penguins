using Game.Penguins.Framework;
using Game.Penguins.ViewModels;

namespace Game.Penguins.Commands
{
    class ApplicationNextViewCommand
        : ViewModelCommand<ApplicationViewModel>
    {
        public ApplicationNextViewCommand(ApplicationViewModel contextViewModel) : base(contextViewModel)
        {
        }

        public override void Execute(ApplicationViewModel contextViewModel, object parameter)
        {
            if (!(contextViewModel.Content is IApplicationContentView currentView))
                return;
            var nextView = currentView.GetNextView() as ViewModel;

            if (nextView != null)
                contextViewModel.Content = nextView;
        }

        public override bool CanExecute(ApplicationViewModel contextViewModel, object parameter)
        {
            if (!(contextViewModel.Content is IApplicationContentView currentView))
                return false;

            return currentView.HasNextView;
        }
    }
}
