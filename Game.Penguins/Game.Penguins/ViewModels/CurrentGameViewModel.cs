using Game.Penguins.Core.Interfaces.Game.Actions;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Core.Interfaces.Game.Players;
using Game.Penguins.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Penguins.ViewModels
{
    class CurrentGameViewModel
        : ViewModel
        , IApplicationContentView
    {
        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();

        private readonly IGame game;

        public string Name => "Game";

        public string PreviousButtonContent => "Configuration";

        public string NextButtonContent => "";

        public bool HasPreviousView => true;

        public bool HasNextView => false;

        #region Game state

        private string currentPlayerName;
        public string CurrentPlayerName
        {
            get => currentPlayerName;
            set
            {
                if (currentPlayerName != value)
                {
                    currentPlayerName = value;
                    RaisePropertyChanged(nameof(CurrentPlayerName));
                }
            }
        }

        #endregion

        #region Actions

        private bool isPlacePenguinAction;
        public bool IsPlacePenguinAction
        {
            get => isPlacePenguinAction;
            set
            {
                if (isPlacePenguinAction != value)
                {
                    isPlacePenguinAction = value;
                    RaisePropertyChanged(nameof(IsPlacePenguinAction));
                }
            }
        }

        private bool isMoveMyPenguinAction;
        public bool IsMoveMyPenguinAction
        {
            get => isMoveMyPenguinAction;
            set
            {
                if (isMoveMyPenguinAction != value)
                {
                    isMoveMyPenguinAction = value;
                    RaisePropertyChanged(nameof(IsMoveMyPenguinAction));
                }
            }
        }

        private bool isPlayAIAction;
        public bool IsPlayAIAction
        {
            get => isPlayAIAction;
            set
            {
                if (isPlayAIAction != value)
                {
                    isPlayAIAction = value;
                    RaisePropertyChanged(nameof(IsPlayAIAction));
                }
            }
        }

        #endregion

        public CurrentGameViewModel(IList<ConfigurationPlayer> players)
            : base()
        {
            // TODO : Initialize with the right implementation
            game = (IGame)null;
            
            game.StateChanged += Game_StateChanged;

            InitGame(players);
            LoadMap();
        }

        private void Game_StateChanged(object sender, EventArgs e)
        {
            CheckActions();
            CurrentPlayerName = game.CurrentPlayer.Name;
        }

        /// <summary>
        /// Init the game object with players
        /// </summary>
        /// <param name="players"></param>
        private void InitGame(IList<ConfigurationPlayer> players)
        {
            // First we initialize the game object with players :
            foreach (var player in players)
            {
                game.AddPlayer(player.PlayerName, player.PlayerType);
            }

            game.StartGame();
        }

        /// <summary>
        /// Load the complete map on the screen
        /// </summary>
        private void LoadMap()
        {
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cells.Add(new CellViewModel(i, j, game.Board.Board[i, j]));
                }
            }
        }

        /// <summary>
        /// Refresh the UI to enable the right buttons for the current action
        /// </summary>
        private void CheckActions()
        {
            IsPlayAIAction = game.NextAction == NextActionType.PlacePenguin && 
                                game.CurrentPlayer.PlayerType != PlayerType.Human;
            IsPlacePenguinAction = game.NextAction == NextActionType.PlacePenguin && 
                                    game.CurrentPlayer.PlayerType == PlayerType.Human;
            IsMoveMyPenguinAction = game.NextAction == NextActionType.MovePenguin &&
                                    game.CurrentPlayer.PlayerType == PlayerType.Human;
        }

        public IApplicationContentView GetPreviousView()
        {
            return new WelcomeScreenViewModel();
        }

        public IApplicationContentView GetNextView()
        {
            throw new NotImplementedException();
        }
    }
}
