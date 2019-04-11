using Game.Penguins.Framework;
using Game.Penguins.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Penguins.Commands
{
    class MovePenguinSelectorViewModelCommand
        : Command
    {
        private readonly CurrentGameViewModel currentGameViewModel;

        public MovePenguinSelectorViewModelCommand(CurrentGameViewModel contextViewModel) : base()
        {
            currentGameViewModel = contextViewModel;
        }

        public override void Execute(object parameter)
        {
            currentGameViewModel.SelectedOriginPenguin();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
