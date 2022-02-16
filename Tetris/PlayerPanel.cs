using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class PlayerPanel
    {
        private AdditionalPanel additionalPanel;
        private Field field;

        private Timer MoveDownTimer;
        private int normalSpeed;
        private int spacespeed;

        private int scores;
        private int levels;
        private int linesRemoved;

        public event Action RepaintRequired;
        public event Action GameOverForPlayer;

        public PlayerPanel(Rectangle rect)
        {
            additionalPanel = new AdditionalPanel();

            field = new Field(rows: 18, columns: 9, padding: 50);
            field.NewFiguerCreated += Field_NewFiguerCreated;
            field.LinesRemoved += Field_LinesRemoved;

            MoveDownTimer = new Timer();
            MoveDownTimer.Tick += Timer_Tick;

            setRectangle(rect);
        }

        public void setRectangle(Rectangle rect)
        {
            field.setRectangle(rect);
            var FieldArea = field.getFieldArea();
            var additionalPanelArea = new Rectangle(FieldArea.Right, FieldArea.Top,
                    rect.Right - FieldArea.Right, FieldArea.Height);
            additionalPanel.setRectangle(additionalPanelArea);
        }

        public void  start()
        {
            normalSpeed = 200;
            spacespeed = 30;
            scores = 0;
            levels = 1;
            linesRemoved = 0;

            additionalPanel.setScores(scores);
            additionalPanel.setLevel(levels);
            additionalPanel.setLinesRemoved(linesRemoved);

            field.start();

            MoveDownTimer.Interval = normalSpeed;
            MoveDownTimer.Start();
        }

        public void stop()
        {
            MoveDownTimer.Stop();
        }

        private void Field_LinesRemoved(int removedCount)
        {
            linesRemoved += removedCount;
            scores += removedCount * 100;
            additionalPanel.setLinesRemoved(linesRemoved);
            additionalPanel.setScores(scores);
            invokeRepaintRequired();
        }
        public void setSecondsPassed(int secondsPassed)
        {
            additionalPanel.setSecondsPassed(secondsPassed);
        }

        private void Field_NewFiguerCreated(Figure current, Figure next)
        {
            additionalPanel.setFigure(next);
            if (MoveDownTimer != null)
            {
                MoveDownTimer.Interval = normalSpeed;
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
            if (!field.moveFigureDown())
            {
                stop();
                invokeGameOverForPlayer();
            }
            invokeRepaintRequired();
        }
        private void invokeGameOverForPlayer()
        {
            if (GameOverForPlayer != null)
            {
                GameOverForPlayer();
            }
        }

        public void Dispose()
        {
            if (MoveDownTimer != null)
            {
                MoveDownTimer.Dispose();
                MoveDownTimer = null;
            }
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
                MoveDownTimer.Interval = spacespeed;
        }
        
        public void display(Graphics g, Rectangle rect, GamesState gamesState)
        {
            field.display(g);
            additionalPanel.display(g, gamesState);
        }
        
    }
}
