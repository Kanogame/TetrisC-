using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Field
    {
        private int rows;
        private int columns;
        private int padding;
        private int[,] data;
        private Figure figure;
        private Point figurePos;

        public event Action NewFiguerCreated;

        private void InvokeNewFiguerCreated()
        {
            if (NewFiguerCreated != null)
            {
                NewFiguerCreated();
            }
        }

        public Field(int rows, int columns, int padding)
        {
            this.rows = rows;
            this.columns = columns;
            this.padding = padding;

            this.data = new int[rows, columns];
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = -1;
                }
            }
        }
        private bool lineIsFull(int lineindex)
        {
            for (int col = 0; col < columns; col++)
            {
                if (data[lineindex, col] < 0)
                {
                    return false;
                }
            }
            return true;
        }

        private int findFullLine()
        {
            for (int row = 0; row < rows; row++)
            {
                if (lineIsFull(row))
                {
                    return row;
                }
            }
            return -1;
        }

        private void removeLine(int lineindex)
        {
            for (int row = lineindex; row >0; row--)
            {
                for (int col = 0; col < columns; col++)
                {
                    data[row, col] = data[row - 1, col];
                }
            }
            for (int col = 0; col < columns; col++)
            {
                data[0, col] = -1;
            }
        }

        private void removeLines()
        {
            while (true)
            {
                int lineindex = findFullLine();
                if (lineindex < 0)
                {
                    return;
                }
                removeLine(lineindex);
            }
        }

        #region
        public void start()
        {
            newFigure();
        }

        private void newFigure()
        {
            this.figure = new Figure();
            this.figurePos = new Point(columns / 2 - 2, 0);
            InvokeNewFiguerCreated();
        }

        //return ex
        public bool moveFigureDown()
        {
            if(figure == null)
            {
                return true;
            }
            figurePos.Y++;
            try
            {
                int[,] d = dataWithFigure;
            } catch  (Exception ex)
            {
                figurePos.Y--;
                data = dataWithFigure;
                removeLines();
                newFigure();
                try
                {
                    int[,] d2 = dataWithFigure;
                }
                catch (Exception ex2)
                {
                    figure = null;
                    return false;
                }
            }
            return true;
        }

        public void toLeft()
        {
            if (figure == null)
            {
                return;
            }
            figurePos.X--;
            try
            {
                int[,] d = dataWithFigure;
            } catch (Exception ex)
            {
                figurePos.X++;
            }
        }

        public void toRight()
        {
            if (figure == null)
            {
                return;
            }
            figurePos.X++;
            try
            {
                int[,] d = dataWithFigure;
            }
            catch (Exception ex)
            {
                figurePos.X--;
            }
        }
        public void rotate()
        {
            figure.rotateCW();
            try
            {
                int[,] d = dataWithFigure;
            }
            catch (Exception ex)
            {
                figure.rotateCCW();
            }
        }
        #endregion
        private int[,] dataWithFigure
        {
            get
            {
                int[,] res = new int[rows, columns];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        res[i, j] = data[i, j];
                    }
                }
                if (figure != null)
                {
                    int[,] f = figure.get();
                    int posX = figurePos.X;
                    int posY = figurePos.Y;
                    for (int i = 0; i < f.GetLength(0); i++)
                    {
                        for (int j = 0; j < f.GetLength(1); j++)
                        {
                            if (f[i, j] != -1)
                            {
                                int realY = posY + i;
                                int realX = posX + j;
                                if (realY < 0 || realX < 0
                                    || realX >= columns
                                    || realY >= rows
                                    // Если там уже есть блок
                                    || data[realY, realX] != -1)
                                {
                                    // Выбрасываем ошибку
                                    throw new Exception("figure cannot be placed");
                                }
                                res[realY, realX] = f[i, j];
                            }
                        }
                    }
                }
                return res;
            }
        }

        private int getCellSize(Size containerSize)
        {
            int w = containerSize.Width;
            int h = containerSize.Height;
            double clientRatio = ((double)w - padding) / (h - padding);
            double fieldRatio = (double)columns / rows;
            int cellSize;
            if (fieldRatio > clientRatio)
            {
                cellSize = (w - padding) / columns;
            }
            else
            {
                cellSize = (h - padding) / rows;
            }
            return cellSize;
        }

        public Point getLocation(Size containerSize)
        {
            int w = containerSize.Width;
            int h = containerSize.Height;
            int cellSize = getCellSize(containerSize);
            int left = (w - cellSize * columns) / 2;
            int top = (h - cellSize * rows) / 2;
            Point location = new Point(left, top);
            return location;
        }

        public int Rows
        {
            get
            {
                return rows;
            }
        }

        public int Columns
        {
            get
            {
                return columns;
            }
        }

        public void display(Graphics g, Size containerSize)
        {
            int cellSize = getCellSize(containerSize);
            Point location = getLocation(containerSize);
            int left = location.X;
            int top = location.Y;
            int[,] d = dataWithFigure;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int colorCode = d[i, j];
                    if (colorCode >= 0)
                    {
                        using (Brush brush = new SolidBrush(Config.Colors[colorCode]))
                        {
                            g.FillRectangle(
                                brush,
                                left + j * cellSize,
                                top + i * cellSize,
                                cellSize,
                                cellSize);
                            //g.FillRectangle(Brushes.Blue, 0, 0, j * cellSize, i * cellSize);
                        }
                    }
                    using (Pen pen = new Pen(Color.FromArgb(215, 215, 215)))
                    {
                        g.DrawRectangle(
                        pen,
                        left + j * cellSize,
                        top + i * cellSize,
                        cellSize,
                        cellSize);
                    }
                }
            }
        }
    }
}
