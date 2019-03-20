using Game.Penguins.Core.Interfaces.Game.Actions;
using Game.Penguins.Core.Interfaces.Game.Players;
using System;
using System.Collections.Generic;

namespace Game.Penguins.Core.Interfaces.Game.GameBoard
{
    public interface IGame
    {
        /// <summary>
        /// Current state of the board
        /// </summary>
        IBoard Board { get; }

        /// <summary>
        /// Next action that the UI must manage
        /// </summary>
        NextActionType NextAction { get; }

        /// <summary>
        /// Get informations about the current user that needs to play
        /// </summary>
        IPlayer CurrentPlayer { get; }

        /// <summary>
        /// Informations about players
        /// </summary>
        IList<IPlayer> Players { get; }
        
        /// <summary>
        /// Fired when the state has changed (Current player, ...)
        /// </summary>
        event EventHandler StateChanged;

        #region Initialization

        /// <summary>
        /// Add a new player on the game
        /// Must be called before StartGame()
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="playerType"></param>
        void AddPlayer(string playerName, PlayerType playerType);

        /// <summary>
        /// Start the game
        /// Do not forget to add players before strting the game
        /// </summary>
        void StartGame();

        #endregion

        #region Place penguins actions

        /// <summary>
        /// Place a penguin for the current user
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void PlacePenguinManual(int x, int y);

        /// <summary>
        /// Call the AI to place his penguin
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="playerType"></param>
        void PlacePenguin();

        #endregion

        #region Move penguin

        /// <summary>
        /// Execute a move for the current user if it's a human
        /// </summary>
        /// <param name="player"></param>
        /// <param name="action"></param>
        void MoveManual(IMove action);

        /// <summary>
        /// Execute a move for an AI
        /// </summary>
        /// <param name="player"></param>
        /// <param name="action"></param>
        void Move();

        #endregion
    }
}
