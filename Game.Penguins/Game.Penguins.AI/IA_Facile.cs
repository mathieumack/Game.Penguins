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
        List<Cell> currentCellPenguins { get; set; }

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

        public void MovePenguins()
        {

        }
    }
}
