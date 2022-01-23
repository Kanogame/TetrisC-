using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class AdditionalPanel
    {
        private int secondsPassed;
        private Figure figure;
        private int scores;
        private int linesRemoved;
        private int level;

        public void setScores(int scores)
        {
            this.scores = scores;
        }

        public void setLinesRemoved(int linesRemoved)
        {
            this.linesRemoved = linesRemoved;
        }

        public void setLevel(int level)
        {
            this.level = level;
        }

        public AdditionalPanel()
        {
            this.secondsPassed = 0;
        }

        public void setFigure(Figure figure)
        {
            this.figure = figure;
        }

        public void setSecondsPassed(int value)
        {
            this.secondsPassed = value;
        }

        private string getClockValue()
        {
            int mm = secondsPassed / 60;
            int ss = secondsPassed % 60;
            string mmPadded = mm.ToString().PadLeft(2, '0');
            string ssPadded = ss.ToString().PadLeft(2, '0');
            return $"{mmPadded}{":"}{ssPadded}";
        }

        public void display(Graphics g, Rectangle rect)
        {
            int Toppading = 20;
            int clockleft = 50;
            int clockTexthGap = 5;
            int Texttopoffset = 3;
            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Far;
            Bitmap img = Config.ClockImage;
            g.DrawImageUnscaled(img, rect.X + clockleft, rect.Y + Toppading);
            using (var f = new Font("Courier New", 14))
            {
                using (var brush = new SolidBrush(Color.FromArgb(119, 127, 148)))
                {
                    g.DrawString(getClockValue(), f, brush, new PointF(rect.X + img.Width + clockleft
                        + clockTexthGap, rect.Y + Texttopoffset + Toppading));
                }
            }
            using (var f = new Font("Segoe UI", 12))
            {
                g.DrawString("Следующая", f, Brushes.Gray, new PointF(rect.X + clockleft, rect.Y + 70));
            }
            using (var f = new Font("Segoe UI", 10))
            {
                g.DrawString("Очки: ", f, Brushes.Gray, new PointF(rect.X + clockleft, rect.Y + 230));
                g.DrawString(scores.ToString() , f, Brushes.Gray, new RectangleF(rect.X + 30, rect.Y + 230, 150, 35), stringFormat);
                g.DrawString("Линии: ", f, Brushes.Gray, new PointF(rect.X + clockleft, rect.Y + 260));
                g.DrawString(linesRemoved.ToString() , f, Brushes.Gray, new RectangleF(rect.X + 30, rect.Y + 260, 150, 35), stringFormat);
                g.DrawString("Уровень: ", f, Brushes.Gray, new PointF(rect.X + clockleft, rect.Y + 290));
                g.DrawString(level.ToString(), f, Brushes.Gray, new RectangleF(rect.X + 30, rect.Y + 290, 150, 35), stringFormat);
            }
            if (figure != null)
            {
                drawfigure(g, rect, clockleft);
            }
        }
        private void drawfigure(Graphics g, Rectangle rect, int clockleft)
        {
            int cellsize = 15;
            var matr = figure.get();
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(1); j++)
                {
                    if (matr[i, j] == -1)
                    {
                        continue;
                    }
                    using (var brush = new SolidBrush(Config.Colors[matr[i, j]]))
                    {
                        g.FillRectangle(brush, rect.X + (cellsize + 1) * j + 60,
                        100 + rect.Y + (cellsize + 1) * i, cellsize, cellsize);
                    }
                }
            }
        }
    }
}
