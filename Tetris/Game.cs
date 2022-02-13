using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Timer MoveDownTimer;
        //timeout ms when moving down
        private Timer secondsTimer;
        private int normalSpeed;
        private int spacespeed;

        private GamesState gamesState;
        private Cursor cursor;

        private int secondsPassed = 0;

        private AdditionalPanel additionalPanel;
        private MainMenu mainMenu;
        private MainMenu GameOverMenu;

        public event CursorchangeDelegate CursorShouldBeChanged;
        public event Action RepaintRequired;

        private int scores;
        private int levels;
        private int linesRemoved;

        private TetrisOneLove TetrisOneLove;

        public Game(TetrisOneLove killer)
        {
            this.cursor = Cursors.Arrow;
            gamesState = GamesState.Menu;
            this.TetrisOneLove = killer;

            this.mainMenu = createMainMenu();
            this.mainMenu.RepaintRequired += invokeRepaintRequired;
            this.mainMenu.HoverButtonDelegate += mainMenu_HoverButton;
            this.mainMenu.ButtonClick += mainMenu_ButtonClicked;

            this.GameOverMenu = createGameOverMenu();
            this.GameOverMenu.RepaintRequired += invokeRepaintRequired;
            this.GameOverMenu.HoverButtonDelegate += mainMenu_HoverButton;
            this.GameOverMenu.ButtonClick += GameOverMenu_ButtonClicked;

            additionalPanel = new AdditionalPanel();

            field = new Field(rows: 18, columns: 9, padding: 50);
            field.NewFiguerCreated += Field_NewFiguerCreated;
            field.LinesRemoved += Field_LinesRemoved;

            MoveDownTimer = new Timer();
            MoveDownTimer.Tick += Timer_Tick;

            secondsTimer = new Timer();
            secondsTimer.Interval = 1000;
            secondsTimer.Tick += secondsTimer_Tick;
        }
        /*
        public void Dispose()
        {
            this.mainMenu.RepaintRequired -= invokeRepaintRequired;
            this.mainMenu.HoverButtonDelegate -= mainMenu_HoverButton;
            this.mainMenu.ButtonClick -= mainMenu_ButtonClicked;
            this.GameOverMenu.RepaintRequired -= invokeRepaintRequired;
            this.GameOverMenu.HoverButtonDelegate -= mainMenu_HoverButton;
            this.GameOverMenu.ButtonClick -= GameOverMenu_ButtonClicked;
            if (secondsTimer != null)
            {
                secondsTimer.Dispose();
                secondsTimer = null;
            }
        */

        private void GameOverMenu_ButtonClicked(int Buttonindex)
        {
            if (Buttonindex == 0)
            {
                start();
            }
            else if (Buttonindex == 1)
            {
                stop();
                gamesState = GamesState.Menu;
            }
        }

        private MainMenu createGameOverMenu()
        {
            var Buttontexts = new string[]
            {
                "Начать Сначала",
                "В главное меню"
             };
            var res = new MainMenu(Buttontexts, 100, 48, 15, false);
            var rect = new Rectangle(0, 0, 140, 140);
            res.setRectangle(rect);
            return res;
        }

        private MainMenu createMainMenu()
        {
            var Buttontexts = new string[]
            {
                "Одиночная игра",
                "Хардкор",
                "Мультиплеер",
                "Выход"
             };
            return new MainMenu(Buttontexts, 170, 38, 20, true, Config.MainMenu);
        }

        private void start()
        {
            gamesState = GamesState.Gaming;
            cursor = Cursors.Arrow;
            MoveDownTimer.Start();
            secondsTimer.Start();
            normalSpeed = 200;
            spacespeed = 30;
            secondsPassed = 0;
            scores = 0;
            levels = 1;
            linesRemoved = 0;

            additionalPanel.setScores(scores);
            additionalPanel.setLevel(levels);
            additionalPanel.setLinesRemoved(linesRemoved);

            field.start();

            MoveDownTimer.Interval = normalSpeed;
            secondsTimer.Start();
            MoveDownTimer.Start();
        }

        private void stop()
        {
            MoveDownTimer.Stop();
            secondsTimer.Stop();
            changeCursor(Cursors.Arrow);
        }

        private void mainMenu_ButtonClicked(int Buttonindex)
        {
            if (Buttonindex == 0)
            {
                start();
            }
            else if (Buttonindex == 1)
            {
                //TetrisOneLove.kill();
                start();
            }
            else if (Buttonindex == 2)
            {
                Application.Exit();
            }
        }

        private void mainMenu_HoverButton(int Buttonindex)
        {
            var cursor = Buttonindex != -1 ? Cursors.Hand : Cursors.Arrow;
            changeCursor(cursor);
        }

        private void changeCursor(Cursor cursor)
        {
            if (cursor != this.cursor)
            {
                this.cursor = cursor;
                if (CursorShouldBeChanged != null)
                {
                    CursorShouldBeChanged(cursor);
                }
            }
        }

        private void Field_LinesRemoved(int removedCount)
        {
            linesRemoved += removedCount;
            scores += removedCount * 100;
            additionalPanel.setLinesRemoved(linesRemoved);
            additionalPanel.setScores(scores);
            invokeRepaintRequired();
        }

        private void secondsTimer_Tick(object sender, EventArgs e)
        {
            secondsPassed++;
            //PlayerPanel.setSecondsPassed(secondsPassed);
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
            if(!field.moveFigureDown())
            {
                stop();              
                gamesState = GamesState.GameOver;
            }
            invokeRepaintRequired();
        }

        public void Dispose()
        {
            if (MoveDownTimer != null)
            {
                MoveDownTimer.Dispose();
                MoveDownTimer = null;
            }
        }

        public void display(Graphics g, Size containerSize)
        {
            var ClientRect = new Rectangle(0, 0, containerSize.Width, containerSize.Height);

            if (gamesState == GamesState.Menu)
            {
                mainMenu.display(g);
            }else
            {
                if (gamesState == GamesState.GameOver)
                {
                    GameOverMenu.display(g);
                    ImageDrawer.fit(g, ClientRect, Config.GameOverImage);
                }
                if (gamesState == GamesState.Gaming)
                {
                    field.display(g, containerSize);
                }
                //PlayerPanel.display(g, clientRect, gamesState);
                var FieldArea = field.GetRectangle(containerSize);
                var additionalPanelArea = new Rectangle(FieldArea.Right, FieldArea.Top,
                        containerSize.Width - FieldArea.Right, FieldArea.Height);
                    additionalPanel.display(g, additionalPanelArea, gamesState);
            }
        }

        public void toLeft()
        {
            if (gamesState == GamesState.Gaming)
            {
                field.toLeft();
                invokeRepaintRequired();
            }
        }

        public void toRight()
        {
            if (gamesState == GamesState.Gaming)
            {
                field.toRight();
                invokeRepaintRequired();
            }
        }

        public void rotate()
        {
            if (gamesState == GamesState.Gaming)
            {
                field.rotate();
                invokeRepaintRequired();
            }
        }

        public void quick()
        {
            if (gamesState == GamesState.Gaming)
            {
                MoveDownTimer.Interval = spacespeed;
            }
        }

        public void mouseMove(Point mousePos)
        {
            if (gamesState == GamesState.Menu)
            {
                mainMenu.mouseMove(mousePos);
            }
            else if (gamesState == GamesState.GameOver)
            {
                GameOverMenu.mouseMove(mousePos);
            }
        }

        public void click(Point mousePos)
        {
            if (gamesState == GamesState.Menu)
            {
                mainMenu.click(mousePos);
            }
            else if (gamesState == GamesState.GameOver)
            {
                GameOverMenu.click(mousePos);
            }
        }

        public void setRectangle(Rectangle rect)
        {
            mainMenu.setRectangle(rect);
        }
    }
}
