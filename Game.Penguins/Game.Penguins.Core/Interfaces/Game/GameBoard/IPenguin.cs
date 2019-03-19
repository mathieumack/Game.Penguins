using Game.Penguins.Core.Interfaces.Game.Players;

namespace Game.Penguins.Core.Interfaces.Game.GameBoard
{
    public interface IPenguin
    {
        /// <summary>
        /// Linked player object
        /// </summary>
        IPlayer Player { get; }
    }
}
