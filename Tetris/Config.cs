using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public static class Config
    {
        public static Color[] Colors = new Color[] {
                Color.FromArgb(176,36,195), // светлосиреневый
            Color.FromArgb(145,2,172), // сиреневый
            Color.FromArgb(0,223,223), // голубой
            Color.FromArgb(0,124,204), // светлосиний
            Color.FromArgb(0,0,205), // синий
            Color.FromArgb(238,129,0), // оранжевый
            Color.FromArgb(251,191,32), // светлооранжевый
            Color.FromArgb(39,144,0), // зеленый
            Color.FromArgb(86,164,0), // светлозеленый
            };

        public static Bitmap ClockImage
        {
            get
            {
                if (_clockImage == null)
                {
                    string pth = Path.Combine(Application.StartupPath, "icon-timer.png");
                    _clockImage = new Bitmap(pth);
                }
                return _clockImage;
            }
        }
        private static Bitmap _clockImage;

        public static Bitmap GameOverImage
        {
            get
            {
                if (_gameOverImage == null)
                {
                    string pth = Path.Combine(Application.StartupPath, "228-2283685_angular2-tetris-transparent-game-over-png.png");
                    _gameOverImage = new Bitmap(pth);
                }
                return _gameOverImage;
            }
        }
        private static Bitmap _gameOverImage;

        public static Bitmap MainMenu
        {
            get
            {
                if (_mainmenu == null)
                {
                    string pth = Path.Combine(Application.StartupPath, "2020_04_tetris-8k-hd-iphone-pc-photos-pictures-backgrounds.jpg");
                    _mainmenu = new Bitmap(pth);
                }
                return _mainmenu;
            }
        }
        private static Bitmap _mainmenu;

        public static KeyboardManager[] keyboardManagers
        {
            get
            {
                return new KeyboardManager[]
                {
                    new KeyboardManager(Keys.W, Keys.A, Keys.D, Keys.S),
                    new KeyboardManager(Keys.I, Keys.J, Keys.L, Keys.K),
                    new KeyboardManager(Keys.Up, Keys.Left, Keys.Right, Keys.Space),
                    new KeyboardManager(Keys.NumPad8, Keys.NumPad4, Keys.NumPad6, Keys.NumPad5),
                };
            }
        }
    }
}
