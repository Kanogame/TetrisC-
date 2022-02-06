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
        private Rectangle rect;
        private ButtonPositioner positioner;
        private int currentButton;

        public MainMenu()
        {
            currentButton = -1;
            rect = new Rectangle(0, 0, 100, 100);
            buttons = new string[]
            {
                "Одиночная игра",
                "Хардкор",
                "Выход"
            };
            positioner = new ButtonPositioner(buttons.Length, buttonWidth: 170, buttonHeight: 38, spaceBetween: 20);
        }

        public void display(Graphics g)
        {
            ImageDrawer.cover(g, rect, Config.MainMenu);
            var SFormat = new StringFormat();
            SFormat.Alignment = StringAlignment.Center;
            SFormat.LineAlignment = StringAlignment.Center;
            using (var b = new SolidBrush(Color.FromArgb(150, 255, 255, 255)))
            {
                using (var b2 = new SolidBrush(Color.FromArgb(200, 255, 255, 255)))
                {
                    g.FillRectangle(b, positioner.PointX - 10, positioner.PointY - 40, positioner.ButtonWidth + 20, positioner.ButtonHeight * 5 + 20);
                    for (int i = 0; i < positioner.ButtonCount; i++)
                    {
                        var btnRect = positioner.getButtonRect(i);
                        var requariedBrush = currentButton == i ? b2 : b;
                        g.FillRectangle(requariedBrush, btnRect);
                        using (var f = new Font("Segoe UI", 12))
                        {
                            g.DrawString(buttons[i], f, Brushes.Black, btnRect, SFormat);
                        }
                    }
                }
            }
            using (var f = new Font("Segoe UI", 14))
            {
                g.DrawString("Tetris Kanogamesa", f, Brushes.Black, new PointF(positioner.PointX, positioner.PointY - 37));
            }
        }
        public void mouseMove(Point mousePos)
        {
            int newcurrentButton = positioner.getButtonIndex(mousePos);
            if (newcurrentButton != currentButton)
            {
                currentButton = newcurrentButton;
                invokeRepaintRequired();
            }
        }

        public event Action RepaintRequired;

        public void invokeRepaintRequired()
        {
            if (RepaintRequired != null)
            {
                RepaintRequired();
            }
        }

        public void setRectangle(Rectangle rect)
        {
            this.rect = rect;
            positioner.setRectangle(rect);
        }
    }
}
