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
        public event WhichButtonDelegate HoverButtonDelegate;
        public event WhichButtonDelegate ButtonClick;
        private Image Bgimage;
        private bool inmenu;

        public MainMenu(string[] buttons, int ButtonWidth, int ButtonHeight, int spasebetweet, bool inmenu, Image bgImage = null)
        {
            this.Bgimage = bgImage;
            currentButton = -1;
            rect = new Rectangle(0, 0, 100, 100);
            this.buttons = buttons;
            this.inmenu = inmenu;
            positioner = new ButtonPositioner(buttons.Length, ButtonWidth, ButtonHeight, spasebetweet);
        }

        private void invokeHoverButtonChanged(int ButtonIndex)
        {
            if (HoverButtonDelegate != null)
            {
                HoverButtonDelegate(ButtonIndex);
            }
        }

        private void invokeButtonClicked(int  Buttonindex)
        {
            if (ButtonClick != null)
            {
                ButtonClick(Buttonindex);
            }
        }

        public void display(Graphics g)
        {
            int BtnCol;
            int BtnPres;
            int btnCll;
            if (inmenu)
            {
                BtnCol = 150;
                BtnPres = 200;
                btnCll = 255;
            }
            else
            {
                BtnCol = 255;
                BtnPres = 150;
                btnCll = 0;
            }
            if (Bgimage != null)
            {
                ImageDrawer.cover(g, rect, Bgimage);
            }
            var SFormat = new StringFormat();
            SFormat.Alignment = StringAlignment.Center;
            SFormat.LineAlignment = StringAlignment.Center;
            using (var b = new SolidBrush(Color.FromArgb(BtnCol, btnCll, btnCll, btnCll)))
            {
                using (var b2 = new SolidBrush(Color.FromArgb(BtnPres, btnCll, btnCll, btnCll)))
                {
                    if (inmenu)
                    {
                        g.FillRectangle(b, positioner.PointX - 10, positioner.PointY - 40, positioner.ButtonWidth + 20, positioner.ButtonHeight * (buttons.Length + 2) + 35);
                    }
                    for (int i = 0; i < positioner.ButtonCount; i++)
                    {
                        var btnRect = positioner.getButtonRect(i);
                        var requariedBrush = currentButton == i ? b2 : b;
                        g.FillRectangle(requariedBrush, btnRect);
                        using (var f = new Font("Segoe UI", 12))
                        {
                            g.DrawString(buttons[i], f, inmenu == true ? Brushes.Black : Brushes.White, btnRect, SFormat);
                        }
                    }
                }
            }
            using (var f = new Font("Segoe UI", 14))
            {
                if (inmenu)
                g.DrawString("Tetris Kanogamesa", f, Brushes.Black, new PointF(positioner.PointX, positioner.PointY - 37));
            }
        }
        public void mouseMove(Point mousePos)
        {
            int newcurrentButton = positioner.getButtonIndex(mousePos);
            if (newcurrentButton != currentButton)
            {
                currentButton = newcurrentButton;
                invokeHoverButtonChanged(currentButton);
                invokeRepaintRequired();
            }
        }

        public void click(Point mousePos)
        {
            int btn = positioner.getButtonIndex(mousePos);
            if (btn != -1)
            {
                invokeButtonClicked(btn);
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
