using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;

namespace Game.Penguins
{
    public class Cell : ICell
    {
        public Cell(int fishCount)
        {
            CellType = CellType.Fish;
            FishCount = fishCount;
            CurrentPenguin = null;
        }

        public Cell()
        {

        }

        public CellType CellType { get; set; }

        public int FishCount { get; set; }

        public IPenguin CurrentPenguin { get; set; }

        public event EventHandler StateChanged;

        public void ChangeState()
        {
            StateChanged.Invoke(this, null);
        }
    }
}
