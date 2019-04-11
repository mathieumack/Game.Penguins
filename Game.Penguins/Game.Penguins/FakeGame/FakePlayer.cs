using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.Players;

namespace Game.Penguins
{
    class FakePlayer : IPlayer
    {
        public FakePlayer(string playerName, PlayerType playerType)
        {
            Name = playerName;
            PlayerType = playerType;
        }

        public FakePlayer()
        {

        }

        /// <summary>
        /// Unique identifier of the player in this game
        /// </summary>
        public Guid Identifier { get; }

        /// <summary>
        /// Informations about the user type (human or AI)
        /// </summary>
        public PlayerType PlayerType { get; }

        /// <summary>
        /// Define the color for the player
        /// </summary>
        public PlayerColor Color { get; }

        /// <summary>
        /// Name of the current player
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Current score for this player
        /// </summary>
        public int Points { get; }

        /// <summary>
        /// Number of available penguins
        /// </summary>
        public int Penguins { get; }

        /// <summary>
        /// Fired when the state has changed (Points, Penguins count ...)
        /// </summary>
        public event EventHandler StateChanged;
    }
}
