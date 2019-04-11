using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;

namespace Game.Penguins
{
    class FakeCell : ICell
    {
        public FakeCell()
        {
            CurrentPenguin = new FakePenguin();
        }

        /// <summary>
        /// Type of the cell
        /// </summary>
        public CellType CellType { get; }

        /// <summary>
        /// Number of fish on this cell
        /// (0 by default if it's not a cell with some fish)
        /// </summary>
        public int FishCount { get; }

        /// <summary>
        /// Current penguin that stay on the cell
        /// Can be null
        /// </summary>
        public IPenguin CurrentPenguin { get; }

        /// <summary>
        /// Fired when the state has changed (Type, FishCount, Penguin, ...)
        /// </summary>
        public event EventHandler StateChanged;
    }
}
