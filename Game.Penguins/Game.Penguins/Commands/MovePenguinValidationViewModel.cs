using Game.Penguins.Framework;
using Game.Penguins.ViewModels;

namespace Game.Penguins.Commands
{
    class MovePenguinValidationViewModel
        : Command
    {
        private readonly CurrentGameViewModel currentGameViewModel;

        public MovePenguinValidationViewModel(CurrentGameViewModel contextViewModel) : base()
        {
            currentGameViewModel = contextViewModel;
        }

        public override void Execute(object parameter)
        {
            currentGameViewModel.MovePenguinHuman();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
