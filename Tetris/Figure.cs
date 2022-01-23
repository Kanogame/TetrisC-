using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Figure
    {
        private static int[][][,] figures = new int[][][,] {
            new int [][,] {
                //  *
                // ***
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 1, 1, 1, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 1, 1, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 1, 1, 1, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 1, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
            },
            new int [][,] {
                // *
                // *
                // **
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 1, 1, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 1, 1, 1, 0 },
                   { 0, 1, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 1, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 1, 0 },
                   { 0, 1, 1, 1, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
            },
            new int [][,] {
                //  *
                //  *
                // **
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 1, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 1, 0, 0, 0 },
                   { 0, 1, 1, 1, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 1, 1, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 1, 1, 1, 0 },
                   { 0, 0, 0, 1, 0 },
                   { 0, 0, 0, 0, 0 },
                },
            },
            new int [][,] {
                // *
                // *
                // *
                // *
                new int[,] {
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 1, 1, 1, 1, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
            },
            new int [][,] {
                // **
                // **
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 1, 1, 0, 0 },
                   { 0, 1, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
            },
            new int [][,] {
                //  **
                // **
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 1, 1, 0 },
                   { 0, 1, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 1, 0, 0, 0 },
                   { 0, 1, 1, 0, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
            },
            new int [][,] {
                // **
                //  **
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 1, 1, 0, 0 },
                   { 0, 0, 1, 1, 0 },
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
                new int[,] {
                   { 0, 0, 0, 0, 0 },
                   { 0, 0, 0, 1, 0 },
                   { 0, 0, 1, 1, 0 },
                   { 0, 0, 1, 0, 0 },
                   { 0, 0, 0, 0, 0 },
                },
            },
            };

        private static Random random = new Random();
        private int figureType;
        private int rotation;
        private int color;

        public Figure()
        {
            this.figureType = random.Next(figures.Length);
            this.rotation = random.Next(figures[this.figureType].Length);
            this.color = random.Next(Config.Colors.Length);
        }

        public void rotateCW()
        {
            int cnt = figures[this.figureType].Length;
            this.rotation++;
            if (this.rotation >= cnt)
            {
                this.rotation = 0;
            }
        }

        public void rotateCCW()
        {
            int cnt = figures[this.figureType].Length;
            this.rotation--;
            if (this.rotation < 0)
            {
                this.rotation = cnt - 1;
            }
        }

        public int[,] get()
        {
            int[,] matr = figures[figureType][rotation];
            int[,] res = new int[matr.GetLength(0), matr.GetLength(1)];
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(1); j++)
                {
                    if (matr[i, j] == 0)
                    {
                        res[i, j] = -1;
                    } else
                    {
                        res[i, j] = this.color;
                    }
                }
            }
            return res;
        }

        public int getLeftOffset()
        {
            int res = 5;
            int[,] fig = get();
            for (int row = 0; row < fig.GetLength(0); row++)
            {
                int col;
                for (col = 0; col < fig.GetLength(1) && fig[row, col] == -1; col++)
                {
                    //nothing
                }

                if (color < res)
                {
                    res = col;
                }
            }
            return res;
        }

        public int getRightOffset()
        {
            int res = 5;
            int[,] fig = get();
            int n = fig.GetLength(1);
            for (int row = 0; row < fig.GetLength(0); row++)
            {
                int col;
                for (col = 0; col < fig.GetLength(1) && fig[row, n - col - 1] == -1; col++)
                {
                    //nothing
                }

                if (color < res)
                {
                    res = col;
                }
            }
            return res;
        }
    }
}
