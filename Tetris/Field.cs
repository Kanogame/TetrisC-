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
        private Figure nextFigure;
        private Point figurePos;
        private Rectangle rect;

        private int cellSize;
        private Rectangle FieldArea;

        public event NewFigureCreatedDelegate NewFiguerCreated;

        private void InvokeNewFiguerCreated()
        {
            if (NewFiguerCreated != null)
            {
                NewFiguerCreated(figure, nextFigure);
            }
        }

        public Field(int rows, int columns, int padding)
        {
            this.rows = rows;
            this.columns = columns;
            this.padding = padding;

            this.data = new int[rows, columns];
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

        private void clearData()
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = -1;
                }
            }
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
            int cnt = 0;
            while (true)
            {
                int lineindex = findFullLine();
                if (lineindex < 0)
                {
                    break;
                }
                removeLine(lineindex);
                cnt++;
            }
            if (cnt > 0)
            {
                invokeLinesRemoved(cnt);
            }
        }

        private void invokeLinesRemoved(int RemoveCount)
        {
            if (LinesRemoved != null)
            {
                LinesRemoved(RemoveCount);
            }
        }

        public event LinesRemovedDelegate LinesRemoved;

        #region
        public void start()
        {
            newFigure(true);
            clearData();
        }

        public void setRectangle(Rectangle rect)
        {
            this.cellSize = getCellSize(rect.Size);
            var r = getFieldAreaRectangle(rect.Size);
            this.FieldArea = new Rectangle(rect.Left + r.Left, rect.Top + r.Top, r.Width, r.Height);
        }

        private void newFigure(bool createNextFigure)
        {
            if (createNextFigure)
            {
                nextFigure = new Figure();
            }
            this.figure = nextFigure;
            this.nextFigure = new Figure();
            this.figurePos = new Point(columns / 2 - 2, -2);
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
            } catch  (Exception)
            {
                figurePos.Y--;
                data = dataWithFigure;
                removeLines();
                newFigure(false);
                try
                {
                    int[,] d2 = dataWithFigure;
                }
                catch (Exception)
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
            if (figure == null)
            {
                return;
            }
            figure.rotateCW();
            figure.get();
            int leftOffset = figure.getLeftOffset();
            int rightOffset = figure.getRightOffset();
            int offsetX = 0;
            int n = 5;
            if (-figurePos.X > leftOffset)
            {
                offsetX = -figurePos.X - leftOffset;
            }
            else if (figurePos.X + n - columns > rightOffset)
            {
                offsetX = -(figurePos.X + n - columns - rightOffset);
            }
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
                                if (realX < 0
                                    || realX >= columns
                                    || realY >= rows
                                    // Если там уже есть блок
                                    || (realY >= 0 && data[realY, realX] != -1))
                                {
                                    // Выбрасываем ошибку
                                    throw new Exception("figure cannot be placed");
                                }
                                if (realY >= 0)
                                {
                                    res[realY, realX] = f[i, j];
                                }
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

        private Point getFieldAreaLocation (Size containerSize)
        {
            int w = containerSize.Width;
            int h = containerSize.Height;
            int cellSize = getCellSize(containerSize);
            int left = (w - cellSize * columns) / 2;
            int top = (h - cellSize * rows) / 2;
            Point location = new Point(left, top);
            return location;
        }

        public Rectangle getFieldArea()
        {
            return FieldArea;
        }

        private Rectangle getFieldAreaRectangle(Size ContainerSize)
        {
            Point location = getFieldAreaLocation(ContainerSize);
            int cellsize = getCellSize(ContainerSize);
            int width = cellsize * columns;
            int height = cellsize * rows;
            return new Rectangle(location.X, location.Y, width, height);
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

        public void display(Graphics g)
        {
            int left = FieldArea.X;
            int top = FieldArea.Y;
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
