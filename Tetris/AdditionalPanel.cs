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

        public AdditionalPanel()
        {
            this.secondsPassed = 0;
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
            Bitmap img = Config.ClockImage;
            g.DrawImageUnscaled(img, rect.X + clockleft, rect.Y + Toppading);
            using (var f = new Font("Courier New", 14))
            {
                g.DrawString(getClockValue(), f, Brushes.LightGray, new PointF(rect.X + img.Width + clockleft
                    + clockTexthGap, rect.Y + Texttopoffset + Toppading));
            }
        }
    }
}
