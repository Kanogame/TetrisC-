using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class PlayerPanel : IDisposable
    {
        private AdditionalPanel additionalPanel;
        private Field field;
        private KeyboardManager KeyboardManager;
        private PlayerPanelLayout layout;

        private Timer MoveDownTimer;
        private int normalSpeed;
        private int spacespeed;
        private bool hideAdditionalPanel;
        private Rectangle rect;

        private int scores;
        private int levels;
        private int linesRemoved;

        public event Action RepaintRequired;
        public event Action GameOverForPlayer;

        public PlayerPanel(Rectangle rect, KeyboardManager keyboardManager, PlayerPanelLayout layout)
        {
            this.layout = layout;
            this.KeyboardManager = keyboardManager;
            this.KeyboardManager.Toleft += toLeft;
            this.KeyboardManager.ToRight += toRight;
            this.KeyboardManager.Rotate += rotate;
            this.KeyboardManager.Quick += quick;
            this.KeyboardManager.ToggleAdditionalPanel += KeyboardManager_ToggleAdditionalPanel;

            additionalPanel = new AdditionalPanel();

            field = new Field(rows: 18, columns: 9, padding: 50);
            field.NewFiguerCreated += Field_NewFiguerCreated;
            field.LinesRemoved += Field_LinesRemoved;

            MoveDownTimer = new Timer();
            MoveDownTimer.Tick += Timer_Tick;

            setRectangle(rect);
        }

        private void KeyboardManager_ToggleAdditionalPanel()
        {
            hideAdditionalPanel = !hideAdditionalPanel;
            setRectangle(this.rect);
            invokeRepaintRequired();
        }

        public void setRectangle(Rectangle rect)
        {
            this.rect = rect;
            if (hideAdditionalPanel)
            {
                field.setRectangle(rect);
                return;
            }
            int additionalPanelwight = 170;
            int topMargin = 25;
            if (layout == PlayerPanelLayout.OnePlayer)
            {
                field.setRectangle(rect);
                var FieldArea = field.getFieldArea();
                var additionalPanelArea = new Rectangle(FieldArea.Right, FieldArea.Top,
                        rect.Right - FieldArea.Right, FieldArea.Height);
                additionalPanel.setRectangle(additionalPanelArea);
            }
            else if (layout == PlayerPanelLayout.FieldOnLeft)
            {
                var fieldRect = new Rectangle(rect.X, rect.Y, rect.Width - additionalPanelwight, rect.Height);
                field.setRectangle(fieldRect);
                var additionalPanelArea = new Rectangle(rect.Right - additionalPanelwight, rect.Y + topMargin,
                        additionalPanelwight, rect.Height - topMargin);
                additionalPanel.setRectangle(additionalPanelArea);
            }
            else if (layout == PlayerPanelLayout.FieldonRight)
            {
                var fieldRect = new Rectangle(rect.X + additionalPanelwight, rect.Y, rect.Width - additionalPanelwight, rect.Height);
                field.setRectangle(fieldRect);
                var additionalPanelArea = new Rectangle(rect.X, rect.Y + topMargin,
                        additionalPanelwight, rect.Height - topMargin);
                additionalPanel.setRectangle(additionalPanelArea);
            }
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
            if (linesRemoved % 5 == 0)
            {
                levels++;
                normalSpeed = (int)(normalSpeed / 1.2);
            }
            additionalPanel.setLinesRemoved(linesRemoved);
            additionalPanel.setScores(scores);
            additionalPanel.setLevel(levels);
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
            this.KeyboardManager.Toleft -= toLeft;
            this.KeyboardManager.ToRight -= toRight;
            this.KeyboardManager.Rotate -= rotate;
            this.KeyboardManager.Quick -= quick;
            this.KeyboardManager.ToggleAdditionalPanel -= KeyboardManager_ToggleAdditionalPanel;
            field.NewFiguerCreated -= Field_NewFiguerCreated;
            field.LinesRemoved -= Field_LinesRemoved;
            if (MoveDownTimer != null)
            {
                MoveDownTimer.Tick -= Timer_Tick;
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
        
        public void display(Graphics g, GamesState gamesState)
        {
            field.display(g);
            if (!hideAdditionalPanel)
            {
                additionalPanel.display(g, gamesState);
            }
        }
        
        public void keyDown(Keys key)
        {
            if (KeyboardManager != null)
            {
                KeyboardManager.keyDown(key);
            }
        }

        public void KeyUp(Keys key)
        {
            if (KeyboardManager != null)
            {
                KeyboardManager.keyUp(key);
            }
        }
    }
}
