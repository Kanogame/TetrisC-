using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class ImageDrawer
    {
        public static void fit(Graphics g, Rectangle rect, Image img)
        {
            double areaAspectRatio = (double)rect.Width / rect.Height;
            double imgaAspectRatio = (double)img.Width / img.Height;
            Rectangle srcRect, destRect;
            if (areaAspectRatio < imgaAspectRatio)
            {
                int ScaledHeight = (int)(rect.Width / imgaAspectRatio);
                int verticalPadding = (rect.Height - ScaledHeight) / 2;
                srcRect = new Rectangle(0, 0, img.Width, img.Height);
                destRect = new Rectangle(rect.X, rect.Y + verticalPadding, rect.Width, ScaledHeight);
            }else
            {
                int Scaledwidth = (int)(rect.Height / imgaAspectRatio);
                int horizontalPadding = (rect.Width - Scaledwidth) / 2;
                srcRect = new Rectangle(0, 0, img.Width, img.Height);
                destRect = new Rectangle(rect.X + horizontalPadding, rect.Y, Scaledwidth, rect.Height);
            }
            g.DrawImage(img, destRect, srcRect, GraphicsUnit.Pixel);
        } 
        public static void stretch(Graphics g, Rectangle rect, Image img)
        {
            g.DrawImage(img, rect, new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
        }

        public static void cover(Graphics g, Rectangle rect, Image img)
        {
            double areaAspectRatio = (double)rect.Width / rect.Height;
            double imgaAspectRatio = (double)img.Width / img.Height;
            Rectangle srcRect, destRect;
            destRect = rect;
            if (areaAspectRatio > imgaAspectRatio)
            {
                int PartHeight = (int)(img.Width / areaAspectRatio);
                int verticalPadding = (img.Height - PartHeight) / 2;
                srcRect = new Rectangle(0, verticalPadding, img.Width, PartHeight);
            }
            else
            {
                int PartWidth = (int)(img.Height * areaAspectRatio);
                int horizontalPadding = (img.Width - PartWidth) / 2;
                srcRect = new Rectangle(horizontalPadding, 0, PartWidth, img.Height);
            }
            g.DrawImage(img, destRect, srcRect, GraphicsUnit.Pixel);
        }
    }
}
