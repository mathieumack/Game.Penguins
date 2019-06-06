using Game.Penguins.Commands;
using Game.Penguins.Core.Interfaces.Game.Actions;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Core.Interfaces.Game.Players;
using Game.Penguins.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Game.Penguins.ViewModels
{
    class CurrentGameViewModel
        : ViewModel
        , IApplicationContentView
    {
        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();

        public ObservableCollection<PlayerViewModel> Players { get; } = new ObservableCollection<PlayerViewModel>();

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
                if (value == null || game.CurrentPlayer.PlayerType != PlayerType.Human || game.NextAction == NextActionType.Nothing)
                    return;

                // Check that the select is valid :
                if (game.NextAction == NextActionType.MovePenguin && selectFirst && value.Cell.CellType != CellType.FishWithPenguin)
                    return;

                if (game.NextAction == NextActionType.MovePenguin && selectFirst && value.Cell.CellType == CellType.FishWithPenguin && value.Cell.CurrentPenguin.Player.Identifier != game.CurrentPlayer.Identifier)
                    return;

                if (game.NextAction == NextActionType.MovePenguin && !selectFirst && value.Cell.CellType != CellType.Fish)
                    return;

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

                // A cell is selected :
                if (game.NextAction == NextActionType.PlacePenguin)
                    PlacePenguinCommand.Execute(null);
                else if (game.NextAction == NextActionType.MovePenguin && value.IsSelectedSecond)
                {
                    MovePenguinValidationViewModel.Execute(null);
                }
                else if (game.NextAction == NextActionType.MovePenguin && value.IsSelectedFirst)
                {
                    MovePenguinSelectorCommand.Execute(null);
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

        #region Colors

        public bool IsBlue { get; private set; }

        public bool IsYellow { get; private set; }

        public bool IsGreen { get; private set; }

        public bool IsRed { get; private set; }

        #endregion

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

        public MovePenguinSelectorViewModelCommand MovePenguinSelectorCommand
        {
            get
            {
                return new MovePenguinSelectorViewModelCommand(this);
            }
        }

        public MovePenguinValidationViewModel MovePenguinValidationViewModel
        {
            get
            {
                return new MovePenguinValidationViewModel(this);
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

        public void SelectedOriginPenguin()
        {
            if (SelectedCell != null)
            {
                selectedFirst = SelectedCell;
                selectFirst = false;
            }
        }

        public void MovePenguinHuman()
        {
            if (SelectedCell != null)
            {
                selectedSecond = SelectedCell;
                game.MoveManual(selectedFirst.Cell, selectedSecond.Cell);

                selectedFirst = null;
                selectedSecond = null;

                CheckActions();
            }
        }

        #endregion

        public CurrentGameViewModel(IList<ConfigurationPlayer> players)
            : base()
        {
            // TODO : Initialize with the right implementation
            game = (IGame)new CustomGame();
            //game = new CustomGame();
            
            game.StateChanged += Game_StateChanged;

            InitGame(players);
            LoadMap();
        }

        private void Game_StateChanged(object sender, EventArgs e)
        {
            CheckActions();
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
                var createdPlayer = game.AddPlayer(player.PlayerName, player.PlayerType);
                Players.Add(new PlayerViewModel(createdPlayer));
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
            foreach (var cell in Cells)
            {
                cell.IsSelectedFirst = false;
                cell.IsSelectedSecond = false;
            }

            selectFirst = true;

            CurrentPlayerName = game.CurrentPlayer.Name;

            IsPlacePenguinAIAction = game.NextAction == NextActionType.PlacePenguin && 
                                        game.CurrentPlayer.PlayerType != PlayerType.Human;
            IsPlacePenguinAction = game.NextAction == NextActionType.PlacePenguin && 
                                    game.CurrentPlayer.PlayerType == PlayerType.Human;
            IsMovePenguinAIAction = game.NextAction == NextActionType.MovePenguin &&
                                    game.CurrentPlayer.PlayerType != PlayerType.Human;
            IsMoveMyPenguinAction = game.NextAction == NextActionType.MovePenguin &&
                                    game.CurrentPlayer.PlayerType == PlayerType.Human;


            IsBlue = game.CurrentPlayer.Color == PlayerColor.Blue;
            IsYellow = game.CurrentPlayer.Color == PlayerColor.Yellow;
            IsGreen = game.CurrentPlayer.Color == PlayerColor.Green;
            IsRed = game.CurrentPlayer.Color == PlayerColor.Red;

            RaisePropertyChanged(nameof(IsBlue));
            RaisePropertyChanged(nameof(IsYellow));
            RaisePropertyChanged(nameof(IsGreen));
            RaisePropertyChanged(nameof(IsRed));
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
