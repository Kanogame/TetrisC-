using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainForm : Form
    {
        private Game game;

        private HorizontalMovement HorizontalMovement;

        private bool toRight;
        private bool toLeft;
        private TetrisOneLove some;

        public MainForm()
        {
            InitializeComponent();
            //some = new TetrisOneLove(this.Handle.ToInt32());
            HorizontalMovement = HorizontalMovement.NoMovement;
            game = new Game(some);
            game.RepaintRequired += Game_RepaintRequired;
        }

        private void Game_RepaintRequired()
        {
            Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            game.display(g, ClientSize);
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            game.setRectangle(ClientRectangle);
            Invalidate();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                SetHorizontalMovement(HorizontalMovement.ToLeft);
                toLeft = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                SetHorizontalMovement(HorizontalMovement.ToRight);
                toRight = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                game.rotate();
            }
            else if (e.KeyCode == Keys.Space)
            {
                game.quick();
            }
        }

        private void SetHorizontalMovement(HorizontalMovement horizontalMovement)
        {
            if (this.HorizontalMovement != horizontalMovement) 
            {
                MakeMovement(horizontalMovement);
                keyboardtimer.Stop();
                if (horizontalMovement != HorizontalMovement.NoMovement)
                {
                    keyboardtimer.Start();
                }
                this.HorizontalMovement = horizontalMovement;
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                toLeft = false;
                if (!toRight)
                {
                    SetHorizontalMovement(HorizontalMovement.NoMovement);
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                toRight = false;
                if (!toLeft)
                {
                    SetHorizontalMovement(HorizontalMovement.NoMovement);
                }
            }
        }

        private void MakeMovement(HorizontalMovement horizontalMovement)
        {
            if (HorizontalMovement == HorizontalMovement.ToLeft)
            {
                game.toLeft();
            }
            else if (HorizontalMovement == HorizontalMovement.ToRight)
            {
                game.toRight();
            }
        }

        private void keyboardtimer_Tick(object sender, EventArgs e)
        {
            MakeMovement(this.HorizontalMovement);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            game.mouseMove(e.Location);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            game.setRectangle(ClientRectangle);
        }
    }
}
