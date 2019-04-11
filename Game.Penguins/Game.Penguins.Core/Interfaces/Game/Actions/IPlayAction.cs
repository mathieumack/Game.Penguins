using System;

namespace Game.Penguins.Core.Interfaces.Game.Actions
{
    public interface IPlayAction
    {
        /// <summary>
        /// Identifier of the player that do the action
        /// </summary>
        Guid PlayerIdenfifier { get; }

        ///// <summary>
        ///// Move done by the user
        ///// </summary>
        //IMove Move { get; }
    }
}
