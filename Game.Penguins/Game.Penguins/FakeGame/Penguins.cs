using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Core.Interfaces.Game.Players;

namespace Game.Penguins
{
    class Penguins : IPenguin
    {
        public Penguins(IPlayer player)
        {
            Player = player;
        }
        public IPlayer Player { get; set; }
    }
}
