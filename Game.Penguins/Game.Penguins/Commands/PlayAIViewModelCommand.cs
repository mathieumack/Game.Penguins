using Game.Penguins.Framework;
using Game.Penguins.ViewModels;

namespace Game.Penguins.Commands
{
    class PlayAIViewModelCommand
        : Command
    {
        private readonly CurrentGameViewModel currentGameViewModel;

        public PlayAIViewModelCommand(CurrentGameViewModel contextViewModel) : base()
        {
            currentGameViewModel = contextViewModel;
        }


        public override void Execute(object parameter)
        {
            currentGameViewModel.PlayAI();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
