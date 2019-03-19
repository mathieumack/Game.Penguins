using System;

namespace Game.Penguins.Core.Interfaces.Game.GameBoard
{
    public interface ICell
    {
        /// <summary>
        /// Type of the cell
        /// </summary>
        CellType CellType { get; }
        
        /// <summary>
        /// Number of fish on this cell
        /// (0 by default if it's not a cell with some fish)
        /// </summary>
        int FishCount { get; }

        /// <summary>
        /// Current penguin that stay on the cell
        /// Can be null
        /// </summary>
        IPenguin CurrentPenguin { get; }

        /// <summary>
        /// Fired when the state has changed (Type, FishCount, Penguin, ...)
        /// </summary>
        event EventHandler StateChanged;
    }
}
