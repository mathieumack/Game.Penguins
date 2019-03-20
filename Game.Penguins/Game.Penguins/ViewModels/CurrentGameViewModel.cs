using Game.Penguins.Commands;
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

        #region Interactions

        private bool selectFirst = true;

        private CellViewModel selectedFirst;
        private CellViewModel selectedSecond;
        public CellViewModel SelectedCell
        {
            get
            {
                if (selectFirst)
                    return selectedFirst;
                else
                    return selectedSecond;
            }
            set
            {
                if (value == null)
                    return;

                // A cell is selected :
                if (selectFirst)
                {
                    if (selectedFirst != null)
                        selectedFirst.IsSelectedFirst = false;

                    value.IsSelectedFirst = true;
                    selectedFirst = value;
                }
                else
                {
                    if (selectedSecond != null)
                        selectedSecond.IsSelectedSecond = false;

                    value.IsSelectedSecond = true;
                    selectedSecond = value;
                }
            }
        }

        #endregion

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

        private bool isMovePenguinAIAction;
        public bool IsMovePenguinAIAction
        {
            get => isMovePenguinAIAction;
            set
            {
                if (isMovePenguinAIAction != value)
                {
                    isMovePenguinAIAction = value;
                    RaisePropertyChanged(nameof(IsMovePenguinAIAction));
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

        private bool isPlacePenguinAIAction;
        public bool IsPlacePenguinAIAction
        {
            get => isPlacePenguinAIAction;
            set
            {
                if (isPlacePenguinAIAction != value)
                {
                    isPlacePenguinAIAction = value;
                    RaisePropertyChanged(nameof(IsPlacePenguinAIAction));
                }
            }
        }

        public PlacePenguinViewModelCommand PlacePenguinCommand
        {
            get
            {
                return new PlacePenguinViewModelCommand(this);
            }
        }

        public PlayAIViewModelCommand PlayAICommand
        {
            get
            {
                return new PlayAIViewModelCommand(this);
            }
        }

        public void PlayPlacePenguinHuman()
        {
            game.PlacePenguinManual(SelectedCell.X, SelectedCell.Y);

            CheckActions();
        }

        public void PlayAI()
        {
            if(IsPlacePenguinAIAction)
                game.PlacePenguin();
            else if (IsMovePenguinAIAction)
                game.Move();

            CheckActions();
        }

        #endregion

        public CurrentGameViewModel(IList<ConfigurationPlayer> players)
            : base()
        {
            // TODO : Initialize with the right implementation
            game = (IGame)null;
            game = new CustomGame();
            
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
            selectFirst = true;
            SelectedCell = null;

            IsPlacePenguinAIAction = game.NextAction == NextActionType.PlacePenguin && 
                                        game.CurrentPlayer.PlayerType != PlayerType.Human;
            IsPlacePenguinAction = game.NextAction == NextActionType.PlacePenguin && 
                                    game.CurrentPlayer.PlayerType == PlayerType.Human;
            IsMovePenguinAIAction = game.NextAction == NextActionType.MovePenguin &&
                                    game.CurrentPlayer.PlayerType != PlayerType.Human;
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
