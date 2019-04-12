using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Core.Interfaces.Game.Players;

namespace Game.Penguins
{
    class GameClass : IGame
    {
        public GameClass()
        {
            Players = new List<IPlayer>();
            Board = new BoardClass();
        }

        public IBoard Board { get; set; }

        public NextActionType NextAction { get; set; }

        public IPlayer CurrentPlayer { get; set; }

        public IList<IPlayer> Players { get; set; }

        public event EventHandler StateChanged;

        public int PenguinsByPlayer { get; set; }

        public int Turn { get; set; }

        public int NumberOfPenguins()
        {
            if (Players.Count() == 2)
            {
                PenguinsByPlayer = 4;
            } 
            else if (Players.Count() == 3)
            {
                PenguinsByPlayer = 3;
            }
            else if (Players.Count() == 4)
            {
                PenguinsByPlayer = 2;
            }

            return PenguinsByPlayer;
        }

        public IPlayer AddPlayer(string playerName, PlayerType playerType)
        {
            PlayerColor color = ChoosePlayerColor();

            IPlayer player = new PlayerClass(playerType, color, playerName);
            Players.Add(player);

            return player;
        }

        PlayerColor ChoosePlayerColor()
        {
            if (Players.Count() == 0)
            {
                return PlayerColor.Blue;
            }
            else if (Players.Count() == 1)
            {
                return PlayerColor.Green;
            }
            else if (Players.Count() == 2)
            {
                return PlayerColor.Red;
            }
            else
            {
                return PlayerColor.Yellow;
            }
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void MoveManual(ICell origin, ICell destination)
        {
            if (origin.CurrentPenguin.Player == CurrentPlayer && destination.CellType == CellType.Fish)
            {
                Cell start = SearchCell(origin);
                start.CellType = CellType.Water;
                start.FishCount = 0;
                start.CurrentPenguin = null;

                Cell end = SearchCell(destination);
                end.CellType = CellType.FishWithPenguin;
                end.CurrentPenguin = new Penguins(CurrentPlayer);

                NextAction = NextActionType.MovePenguin;
                NextPlayer();
                StateChanged(this, null);
                start.ChangeState();
                end.ChangeState();
            }
        }

        public Cell SearchCell(ICell cellToFind)
        {
            for (int i = 0; i < Board.Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.Board.GetLength(1); j++)
                {
                    if (Board.Board[i, j] == cellToFind)
                    {
                        return (Cell)Board.Board[i, j];
                    }
                }
            }

            return null;
        }

        public int[] SearchIndexOfCell(Cell cell)
        {
            int[] index = new int[2];

            for (int i = 0; i < Board.Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.Board.GetLength(1); j++)
                {
                    if (Board.Board[i, j] == cell)
                    {
                        index[0] = i;
                        index[1] = j;

                        return index;
                    }
                }
            }

            return null;
        }

        public List<Cell> FindAvalaibleDeplacement(Cell origin, Cell destination)
        {
            List<Cell> avalaibleDeplacement = new List<Cell>();
            int[] cellIndexOrigin = SearchIndexOfCell(origin);
            int[] cellIndexDest = SearchIndexOfCell(destination);

            for (int i = 0; i < Board.Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.Board.GetLength(1); j++)
                {
                    if (i == cellIndexOrigin[0])
                    {
                        avalaibleDeplacement.Add((Cell)Board.Board[i, j]);
                    }
                    else if ()
                }
            }
        }

        public void PlacePenguin()
        {
            throw new NotImplementedException();
        }

        public void PlacePenguinManual(int x, int y)
        {
            Cell cell = (Cell)Board.Board[x, y];
            cell.CellType = CellType.FishWithPenguin;
            cell.CurrentPenguin = new Penguins(CurrentPlayer);

            NextAction = NextActionType.PlacePenguin;
            if (Turn == Players.Count() * PenguinsByPlayer)
            {
                NextAction = NextActionType.MovePenguin;
            }

            Turn++;
            NextPlayer();
            cell.ChangeState();
            StateChanged.Invoke(this, null);
        }

        public void StartGame()
        {
            Turn = 1;
            CurrentPlayer = Players[0];
            PenguinsByPlayer = NumberOfPenguins();
            NextAction = NextActionType.PlacePenguin;
            StateChanged.Invoke(this, null);
        }

        public void NextPlayer()
        {
            int nextPlayerIndex = Players.IndexOf(CurrentPlayer) + 1;

            if(nextPlayerIndex == Players.Count())
            {
                nextPlayerIndex = 0;
            }

            CurrentPlayer = Players[nextPlayerIndex];
        }
    }
}
