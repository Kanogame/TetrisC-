using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class MainMenu
    {

        private string[] buttons;

        public MainMenu()
        {
            buttons = new string[]
            {
                "одиночная игра",
                "мультиплеер",
                "выход"
            }; 
        }

        public void display(Graphics g, Rectangle rect)
        {
            ImageDrawer.cover(g, rect, Config.MainMenu);
            const int btnWidth = 170;
            const int btnHeight = 38;
            int btnCount = buttons.Length;
            int spaceBetween = 20;
            int Px = (rect.Width - btnWidth) / 2;
            int Py = (rect.Height - btnHeight * btnCount - spaceBetween * (btnCount - 1)) / 2;
            int next = btnHeight + spaceBetween;
            using (var b = new SolidBrush(Color.FromArgb(150, 255, 255, 255)))
                for (int i = 0; i < btnCount; i++)
                {
                    var btnRect = new Rectangle(Px, Py + next * i, btnWidth, btnHeight);
                    g.FillRectangle(b, btnRect.X, btnRect.Y, btnWidth, btnHeight);
                    g.DrawRectangle(Pens.Gray, Px, Py + next * i, btnWidth, btnHeight);
                }
            using (var f = new Font("Segoe UI", 14))
            {
                g.DrawString("Tetris Kanogamesa", f, Brushes.Gray, new PointF(Px, Py - 37));
            }
        }
    }
}
