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
            some = new TetrisOneLove(this.Handle.ToInt32());
            HorizontalMovement = HorizontalMovement.NoMovement;
            game = new Game(some);
            game.RepaintRequired += Game_RepaintRequired;
            game.CursorShouldBeChanged += Game_CursorShouldBeChanged;
        }

        private void Game_CursorShouldBeChanged(Cursor cursor)
        {
            this.Cursor = cursor;
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
          game.keyDown(e.KeyCode);
        }

        private void SetHorizontalMovement(HorizontalMovement horizontalMovement)
        {

        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            game.keyUp(e.KeyCode);
        }

        private void MakeMovement(HorizontalMovement horizontalMovement)
        {

        }


        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            game.mouseMove(e.Location);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            game.setRectangle(ClientRectangle);
            Invalidate();
        }

        private void gale_cursorShoudBeChanged(Cursor cursor)
        {
            this.Cursor = cursor;
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            game.click(e.Location);
        }
    }
}
