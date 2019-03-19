using Game.Penguins.Core.Interfaces.Game;
using Game.Penguins.Core.Interfaces.Game.Actions;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using System;

namespace Game.Penguins.Core.Interfaces.AI
{
    public interface AIEngine
    {
        /// <summary>
        /// Init the AI
        /// </summary>
        /// <param name="board">boad object</param>
        /// <param name="playerIdentifier">Identifier of the player object in the board for this AIEngine</param>
        void Init(IBoard board, Guid playerIdentifier);

        /// <summary>
        /// Call to the AI in order to play
        /// </summary>
        IPlayAction Play();
    }
}
