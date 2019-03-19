using Game.Penguins.Commands;
using Game.Penguins.Framework;

namespace Game.Penguins.ViewModels
{
    class ApplicationViewModel
        : ViewModel
    {
        private ViewModel _content;

        public ViewModel Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    RaisePropertyChanged(nameof(Content));

                    NextViewCommand.RaiseCanExecuteChanged();
                    PreviousViewCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    RaisePropertyChanged(nameof(IsEnabled));
                }
            }
        }

        private ApplicationNextViewCommand _nextViewCommand;

        public ApplicationNextViewCommand NextViewCommand => _nextViewCommand ?? (_nextViewCommand = new ApplicationNextViewCommand(this));

        private ApplicationPreviousViewCommand _previousViewCommand;

        public ApplicationPreviousViewCommand PreviousViewCommand => _previousViewCommand ?? (_previousViewCommand = new ApplicationPreviousViewCommand(this));

        private ApplicationToggleIsEnabledCommand _toggleIsEnabledCommand;

        public ApplicationToggleIsEnabledCommand ToggleIsEnabledCommand => _toggleIsEnabledCommand ?? (_toggleIsEnabledCommand = new ApplicationToggleIsEnabledCommand(this));

        public ApplicationViewModel()
        {
            _content = new WelcomeScreenViewModel();
            IsEnabled = true;
        }
    }
}
