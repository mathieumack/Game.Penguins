using Game.Penguins.Framework;
using Game.Penguins.ViewModels;

namespace Game.Penguins.Commands
{
    class PlacePenguinViewModelCommand
        : Command
    {
        private readonly CurrentGameViewModel currentGameViewModel;

        public PlacePenguinViewModelCommand(CurrentGameViewModel contextViewModel) : base()
        {
            currentGameViewModel = contextViewModel;
        }

        public override void Execute(object parameter)
        {
            currentGameViewModel.PlayPlacePenguinHuman();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
            return currentGameViewModel.SelectedCell != null;
        }
    }
}
