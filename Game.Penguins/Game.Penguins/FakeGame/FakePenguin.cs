using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Core.Interfaces.Game.Players;

namespace Game.Penguins
{
    class FakePenguin : IPenguin
    {
        public FakePenguin()
        {
            Player = new FakePlayer();
        }
        /// <summary>
        /// Linked player object
        /// </summary>
        public IPlayer Player { get; }
    }
}
