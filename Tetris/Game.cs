using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    class Game : IDisposable
    {
        private Field field;
        // System.Windows.Forms.Timer
        private Timer timer;
        //timeout ms when moving down
        private int normalSpeed;
        //timeout ms when quick moving down
        private int spacespeed;

        public event Action RepaintRequired;

        public Game()
        {
            normalSpeed = 170;
            spacespeed = 30;
            field = new Field(rows: 18, columns: 9, padding: 50);
            field.NewFiguerCreated += Field_NewFiguerCreated;
            field.start();
            timer = new Timer();
            timer.Interval = normalSpeed;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Field_NewFiguerCreated()
        {
            if (timer != null)
            {
                timer.Interval = normalSpeed;
            }
        }

        private void invokeRepaintRequired()
        {
            if (RepaintRequired != null)
            {
                RepaintRequired();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(!field.moveFigureDown())
            {
                MessageBox.Show("Игра окончена, научись играть");
            }
            invokeRepaintRequired();
        }

        public void Dispose()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        public void display(Graphics g, Size containerSize) {
            field.display(g, containerSize);
        }

        public void toLeft()
        {
            field.toLeft();
            invokeRepaintRequired();
        }

        public void toRight()
        {
            field.toRight();
            invokeRepaintRequired();
        }

        public void rotate()
        {
            field.rotate();
            invokeRepaintRequired();
        }

        public void quick()
        {
            timer.Interval = spacespeed;
        }
    }
}
