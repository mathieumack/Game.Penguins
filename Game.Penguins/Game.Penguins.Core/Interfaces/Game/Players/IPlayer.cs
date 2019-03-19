using System;

namespace Game.Penguins.Core.Interfaces.Game.Players
{
    public interface IPlayer
    {
        /// <summary>
        /// Unique identifier of the player in this game
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// Informations about the user type (human or AI)
        /// </summary>
        PlayerType PlayerType { get; }

        /// <summary>
        /// Define the color for the player
        /// </summary>
        PlayerColor Color { get; }

        /// <summary>
        /// Name of the current player
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Current score for this player
        /// </summary>
        int Points { get; }

        /// <summary>
        /// Number of available penguins
        /// </summary>
        int Penguins { get; }
        
        /// <summary>
        /// Fired when the state has changed (Points, Penguins count ...)
        /// </summary>
        event EventHandler StateChanged;
    }
}
