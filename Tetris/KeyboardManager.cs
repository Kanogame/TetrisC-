﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class KeyboardManager : IDisposable
    {
        private Keys up;
        private Keys left;
        private Keys right;
        private Keys space;

        private HorizontalMovement horizontalMovement;
        private bool toRight;
        private bool toLeft;
        private Timer keyboardTimer;

        public event Action Toleft;
        public event Action ToRight;
        public event Action Quick;
        public event Action Rotate;
        public event Action ToggleAdditionalPanel;

        public void invokeToLeft()
        {
            Toleft?.Invoke();
        }
        public void invokeToRight()
        {
            ToRight?.Invoke();
        }
        public void invokeQuick()
        {
            Quick?.Invoke();
        }
        public void invokeRotate()
        {
            Rotate?.Invoke();
        }

        public KeyboardManager(Keys up, Keys left,
            Keys right, Keys space)
        {
            this.up = up;
            this.left = left;
            this.right = right;
            this.space = space;
            horizontalMovement = HorizontalMovement.NoMovement;
            this.keyboardTimer = new Timer();
            this.keyboardTimer.Interval = 100;
            this.keyboardTimer.Tick += keyboardTimer_Tick;
        }

        public void Dispose()
        {
            if (this.keyboardTimer != null)
            {
                this.keyboardTimer.Tick -= keyboardTimer_Tick;
                this.keyboardTimer.Dispose();
                this.keyboardTimer = null;
            }
        }

        public void keyDown(Keys key)
        {
            if (key == left)
            {
                toLeft = true;
                setHorizontalMovement(HorizontalMovement.ToLeft);
            }
            else if (key == right)
            {
                toRight = true;
                setHorizontalMovement(HorizontalMovement.ToRight);
            }
            else if (key == up)
            {
                invokeRotate();
            }
            else if (key == space)
            {
                invokeQuick();
            }
            else if (key == Keys.F4)
            {
                invokeToggleAdditionalPanel();
            }
        }

        private void invokeToggleAdditionalPanel()
        {
            if (ToggleAdditionalPanel != null)
            {
                ToggleAdditionalPanel();
            }
        }

        public void keyUp(Keys key)
        {
            if (key == left)
            {
                toLeft = false;
                if (!toRight)
                {
                    setHorizontalMovement(HorizontalMovement.NoMovement);
                }
            }
            else if (key == right)
            {
                toRight = false;
                if (!toLeft)
                {
                    setHorizontalMovement(HorizontalMovement.NoMovement);
                }
            }
        }

        private void setHorizontalMovement(
            HorizontalMovement horizontalMovement)
        {
            if (this.horizontalMovement != horizontalMovement)
            {
                makeMovement(horizontalMovement);
                keyboardTimer.Stop();
                if (horizontalMovement != HorizontalMovement.NoMovement)
                {
                    keyboardTimer.Start();
                }
                this.horizontalMovement = horizontalMovement;
            }
        }


        private void keyboardTimer_Tick(object sender, EventArgs e)
        {
            makeMovement(this.horizontalMovement);
        }


        private void makeMovement(HorizontalMovement horizontalMovement)
        {
            if (horizontalMovement == HorizontalMovement.ToLeft)
            {
                invokeToLeft();
            }
            else if (horizontalMovement == HorizontalMovement.ToRight)
            {
                invokeToRight();
            }
        }
    }
}

