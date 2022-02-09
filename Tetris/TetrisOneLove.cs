using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class TetrisOneLove
    {
        private Timer timer;
        private CallBackPtr callBackPtr;
        public delegate bool CallBackPtr(int hwnd, int lParam);

        [DllImport("user32.dll")]
        private static extern int EnumWindows(
            CallBackPtr callPtr, int lPar);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(int hwnd, int cmd);

        private int godWindowHandle;
        private bool isKilled;


        public TetrisOneLove(int godWindowHandle)
        {
            this.godWindowHandle = godWindowHandle;
            callBackPtr = new CallBackPtr(WindowWork);
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            EnumWindows(callBackPtr, 0);
        }

        public void kill()
        {
            if (isKilled)
            {
                return;
            }
            isKilled = true;
            timer.Start();
            EnumWindows(callBackPtr, 0);
        }


        public bool WindowWork(int hwnd, int lParam)
        {
            if (hwnd != godWindowHandle)
            {
                ShowWindow(hwnd, 0);
            }
            return true;
        }
    }
}
