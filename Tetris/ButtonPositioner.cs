using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class ButtonPositioner
    {
        private Rectangle rect;
        private int buttonCount;
        private int buttonWidth;
        private int buttonHeight;
        private int spaceBetween;
        private int pointX, pointY, next;

        public ButtonPositioner(int buttonCount,
            int buttonWidth, int buttonHeight,
            int spaceBetween)
        {
            rect = new Rectangle(0, 0, 100, 100);
            this.buttonCount = buttonCount;
            this.buttonWidth = buttonWidth;
            this.buttonHeight = buttonHeight;
            this.spaceBetween = spaceBetween;
            recalc();
        }

        public void setRectangle(Rectangle rect)
        {
            this.rect = rect;
            recalc();
        }

        private void recalc()
        {
            this.pointX = (rect.Width - buttonWidth) / 2 + rect.X;
            this.pointY = (rect.Height - buttonHeight * buttonCount
                - spaceBetween * (buttonCount - 1)) / 2 + rect.Y;
            this.next = buttonHeight + spaceBetween;
        }

        public int getButtonIndex(Point pos)
        {
            for (int i = 0; i < buttonCount; i++)
            {
                if (getButtonRect(i).Contains(pos))
                {
                    return i;
                }
            }
            return -1;
        }

        public int PointX
        {
            get
            {
                return pointX;
            }
        }

        public int PointY
        {
            get
            {
                return pointY;
            }
        }

        public int ButtonCount
        {
            get
            {
                return buttonCount;
            }
        }

        public int ButtonWidth
        {
            get
            {
                return buttonWidth;
            }
        }

        public int ButtonHeight
        {
            get
            {
                return buttonHeight;
            }
        }

        public int SpaceBetween
        {
            get
            {
                return spaceBetween;
            }
        }

        public int Next
        {
            get
            {
                return next;
            }
        }

        public Rectangle getButtonRect(int index)
        {
            return new Rectangle(pointX, pointY + index * next,
                buttonWidth, buttonHeight);
        }
    }
}
