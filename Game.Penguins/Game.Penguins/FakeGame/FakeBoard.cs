using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;

namespace Game.Penguins
{
    class FakeBoard : IBoard
    {
        public FakeBoard()
        {
            Board = new FakeCell[8,8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[i, j] = new FakeCell();
                }
            }
        }

        /// <summary>
        /// Current board
        /// </summary>
        public ICell[,] Board { get; }
    }
}
