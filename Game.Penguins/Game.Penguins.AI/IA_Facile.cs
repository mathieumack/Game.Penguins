using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Core.Interfaces.Game.Players;

namespace Game.Penguins
{
    public class IA_Facile
    {
        public IA_Facile()
        {
        }

        public void PlacePenguins(BoardClass Board, PlayerClass CurrentPlayer)
        {
            List<Cell> availableCell = GetAvailablePlacementCell(Board);
            Random rnd = new Random();

            int rndIndex = rnd.Next(availableCell.Count);

            Cell cell = availableCell[rndIndex];
            cell.CellType = CellType.FishWithPenguin;
            cell.CurrentPenguin = new Penguins(CurrentPlayer);
            cell.ChangeState();
        }

        public List<Cell> GetAvailablePlacementCell(BoardClass Board)
        {
            List<Cell> availableCell = new List<Cell>();

            foreach (Cell cell in Board.Board)
            {
                if (cell.FishCount == 1 && cell.CellType == CellType.Fish)
                {
                    availableCell.Add(cell);
                }
            }

            return availableCell;
        }

        public bool MovePenguins(BoardClass Board, PlayerClass CurrentPlayer)
        {
            Random rnd = new Random();
            List<Cell> myPenguinsCell = GetMyPenguinsCell(Board, CurrentPlayer);

            if (myPenguinsCell.Count != 0)
            {
                int rndIndex = rnd.Next(myPenguinsCell.Count);
                List<Cell> availableCell = FindAvailableCellNeighbor(myPenguinsCell[rndIndex], Board);

                if (availableCell.Count == 0)
                {
                    Cell toDelete = myPenguinsCell[rndIndex];

                    PlayerClass currentPlayer = CurrentPlayer;
                    currentPlayer.Points += toDelete.FishCount;
                    currentPlayer.ChangeState();

                    toDelete.CellType = CellType.Water;
                    toDelete.FishCount = 0;
                    toDelete.CurrentPenguin = null;
                    toDelete.ChangeState();

                    MovePenguins(Board, CurrentPlayer);
                }
                else
                {
                    Cell start = myPenguinsCell[rndIndex];

                    rndIndex = rnd.Next(availableCell.Count);
                    Cell end = availableCell[rndIndex];

                    PlayerClass currentPlayer = CurrentPlayer;
                    currentPlayer.Points += start.FishCount;
                    currentPlayer.ChangeState();

                    start.CellType = CellType.Water;
                    start.FishCount = 0;
                    start.CurrentPenguin = null;

                    end.CellType = CellType.FishWithPenguin;
                    end.CurrentPenguin = new Penguins(CurrentPlayer);

                    start.ChangeState();
                    end.ChangeState();
                }

                return false;
            }

            return true;
        }

        public List<Cell> GetMyPenguinsCell(BoardClass Board, PlayerClass CurrentPlayer)
        {
            List<Cell> myPenguinsCell = new List<Cell>();

            foreach (Cell cell in Board.Board)
            {
                if (cell.CurrentPenguin != null && cell.CurrentPenguin.Player == CurrentPlayer)
                {
                    myPenguinsCell.Add(cell);
                }
            }

            return myPenguinsCell;
        }

        public List<Cell> FindAvailableCellNeighbor(Cell start, BoardClass Board)
        {
            List<Cell> neighbor = new List<Cell>();
            int[] startCellIndex = SearchIndexOfCell(start, Board);
            int modifier = 0;

            if (startCellIndex[1] % 2 == 0)
            {
                modifier = -1;
            }
            else
            {
                modifier = 1;
            }
            Console.WriteLine("I : {0}, J : {1}", startCellIndex[0], startCellIndex[1]);
            if (startCellIndex[0] + modifier >= 0 && startCellIndex[0] + modifier <= 7 && startCellIndex[1] - 1 >= 0)
                neighbor.Add((Cell)Board.Board[startCellIndex[0] + modifier, startCellIndex[1] - 1]);
            if (startCellIndex[1] - 1 >= 0)
                neighbor.Add((Cell)Board.Board[startCellIndex[0], startCellIndex[1] - 1]);
            if (startCellIndex[0] + modifier >= 0 && startCellIndex[0] + modifier <= 7 && startCellIndex[1] + 1 <= 7)
                neighbor.Add((Cell)Board.Board[startCellIndex[0] + modifier, startCellIndex[1] + 1]);
            if (startCellIndex[1] + 1 <= 7)
                neighbor.Add((Cell)Board.Board[startCellIndex[0], startCellIndex[1] + 1]);
            if (startCellIndex[0] - 1 >= 0)
                neighbor.Add((Cell)Board.Board[startCellIndex[0] - 1, startCellIndex[1]]);
            if (startCellIndex[0] + 1 <= 7)
                neighbor.Add((Cell)Board.Board[startCellIndex[0] + 1, startCellIndex[1]]);

            neighbor.RemoveAll(e => e.CellType != CellType.Fish);

            return neighbor;
        }

        public int[] SearchIndexOfCell(Cell cell, BoardClass Board)
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
    }
}
