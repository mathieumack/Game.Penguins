using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Helpers;

namespace Game.Penguins
{
    class FakeBoard : IBoard
    {
        public FakeBoard()
        {
            Board = new FakeCell[8,8];
            initBoard();
        }

        public void initBoard()
        {
            int compteur = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    compteur++;
                    if (compteur < 34)
                    {
                        Board[i, j] = new FakeCell(CellType.Fish, 1, null);
                    }
                    else if (compteur >= 34 && compteur < 54)
                    {
                        Board[i, j] = new FakeCell(CellType.Fish, 2, null);
                    }
                    else if (compteur >= 54)
                    {
                        Board[i, j] = new FakeCell(CellType.Fish, 3, null);
                    }
                }
            }

            Random rnd = new Random();
            Helper.Shuffle(rnd, Board);
        }

        /// <summary>
        /// Current board
        /// </summary>
        public ICell[,] Board { get; }
    }
}
