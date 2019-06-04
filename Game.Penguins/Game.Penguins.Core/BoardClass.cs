using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;

namespace Game.Penguins
{
    public class BoardClass : IBoard
    {
        public BoardClass()
        {
            Board = new Cell[8, 8];
            Helpers = new Helpers();
            InitBoard();
        }

        public Helpers Helpers { get; set; }

        public ICell[,] Board { get; set; }

        void InitBoard()
        {
            int compteur = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j ++)
                {
                    if (compteur < 34)
                    {
                        Board[i, j] = new Cell(1);
                    } 
                    else if (compteur >= 34 && compteur < 54)
                    {
                        Board[i, j] = new Cell(2);
                    }
                    else if (compteur >= 54)
                    {
                        Board[i, j] = new Cell(3);
                    }

                    compteur++;
                }
            }

            Random random = new Random();
            Helpers.Shuffle(random, Board);
        }
    }
}
