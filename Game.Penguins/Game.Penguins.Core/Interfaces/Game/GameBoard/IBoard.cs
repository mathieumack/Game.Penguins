namespace Game.Penguins.Core.Interfaces.Game.GameBoard
{
    public interface IBoard
    {
        /// <summary>
        /// Current board
        /// </summary>
        ICell[,] Board { get; }
    }
}
