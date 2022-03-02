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
        private Timer secondsTimer;

        private MainMenu mainMenu;
        private MainMenu GameOverMenu;
        private MainMenu ChoosePlayersCount;
        private TetrisOneLove TetrisOneLove;
        private PlayerPanel[] playerPanels;
        private Rectangle rect;

        private int secondsPassed = 0;
        private int playerleft;

        private GamesState gamesState;
        private Cursor cursor;

        public event CursorchangeDelegate CursorShouldBeChanged;
        public event Action RepaintRequired;

        public Game(TetrisOneLove killer)
        {
            this.rect = new Rectangle(0, 0, 100, 100);
            this.cursor = Cursors.Arrow;
            gamesState = GamesState.Menu;
            this.TetrisOneLove = killer;
            this.playerPanels = new PlayerPanel[0];

            this.mainMenu = createMainMenu();
            this.mainMenu.RepaintRequired += invokeRepaintRequired;
            this.mainMenu.HoverButtonDelegate += mainMenu_HoverButton;
            this.mainMenu.ButtonClick += mainMenu_ButtonClicked;

            this.GameOverMenu = createGameOverMenu();
            this.GameOverMenu.RepaintRequired += invokeRepaintRequired;
            this.GameOverMenu.HoverButtonDelegate += mainMenu_HoverButton;
            this.GameOverMenu.ButtonClick += GameOverMenu_ButtonClicked;

            secondsTimer = new Timer();
            secondsTimer.Interval = 1000;
            secondsTimer.Tick += secondsTimer_Tick;
        }

        internal void keyDown(Keys Key)
        {
            if (gamesState != GamesState.Gaming)
            {
                return;
            }
            foreach (var pnl in playerPanels)
            {
                pnl.keyDown(Key);
            }
        }

        internal void keyUp(Keys Key)
        {
            if (gamesState != GamesState.Gaming)
            {
                return;
            }
            foreach (var pnl in playerPanels)
            {
                pnl.KeyUp(Key);
            }
        }

        public void Dispose()
        {
            this.mainMenu.RepaintRequired -= invokeRepaintRequired;
            this.mainMenu.HoverButtonDelegate -= mainMenu_HoverButton;
            this.mainMenu.ButtonClick -= mainMenu_ButtonClicked;
            this.GameOverMenu.RepaintRequired -= invokeRepaintRequired;
            this.GameOverMenu.HoverButtonDelegate -= mainMenu_HoverButton;
            this.GameOverMenu.ButtonClick -= GameOverMenu_ButtonClicked;
            this.ChoosePlayersCount.RepaintRequired -= invokeRepaintRequired;
            this.ChoosePlayersCount.HoverButtonDelegate -= mainMenu_HoverButton;
            this.ChoosePlayersCount.ButtonClick -= ChoosePlayersCount_ButtonClicked;
            if (secondsTimer != null)
            {
                secondsTimer.Dispose();
                secondsTimer = null;
            }
        }

        private void ChoosePlayersCount_ButtonClicked(int Buttonindex)
        {
            if (Buttonindex == 0)
            {
                start(1);
            }
            else if (Buttonindex == 1)
            {
                start(3);
            }
            else if (Buttonindex == 2)
            {
                start(2);
            }
            else if (Buttonindex == 3)
            {
                Application.Exit();
            }
        }
        private MainMenu createChoosePlayersCount()
        {
            var Buttontexts = new string[]
            {
                "1",
                "2",
                "3",
                "4"
             };
            return new MainMenu(Buttontexts, 170, 38, 20, true, Config.MainMenu);
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

        private void start(int PlayersCount)
        {
            foreach (var pnl in playerPanels)
            {
                pnl.RepaintRequired -= invokeRepaintRequired;
                pnl.GameOverForPlayer -= pnl_GameOverForPlayer;
                pnl.Dispose();
            }
            playerPanels = new PlayerPanel[PlayersCount];
            for (int i = 0; i < playerPanels.Length; i++)
            {
                PlayerPanelLayout layout;
                KeyboardManager kbdManager;
                if (PlayersCount == 1)
                {
                    kbdManager = Config.keyboardManagers[2];
                    layout = PlayerPanelLayout.OnePlayer;
                }
                else if (PlayersCount == 2)
                {
                    if (i == 0)
                    {
                        kbdManager = Config.keyboardManagers[0];
                        layout = PlayerPanelLayout.FieldOnLeft;
                    }
                    else
                    {
                        kbdManager = Config.keyboardManagers[2];
                        layout = PlayerPanelLayout.FieldonRight;
                    }
                }
                else
                {
                    int kbdIndex = i % Config.keyboardManagers.Length;
                    kbdManager = Config.keyboardManagers[kbdIndex];
                    layout = PlayerPanelLayout.FieldOnLeft;
                }
                var pnl = new PlayerPanel(new Rectangle(0,0,100,100), kbdManager, layout);
                pnl.RepaintRequired += invokeRepaintRequired;
                pnl.GameOverForPlayer += pnl_GameOverForPlayer;
                playerPanels[i] = pnl;
            }
            setRectForPlayerPanels();
            gamesState = GamesState.Gaming;
            cursor = Cursors.Arrow;
            playerleft = PlayersCount;
            secondsPassed = 0;
            secondsTimer.Start();
            foreach (var pnl in playerPanels)
            {
                pnl.start();
            }
            invokeRepaintRequired();
        }

        private void pnl_GameOverForPlayer()
        {
            playerleft--;
            if (playerleft <= 0)
            {
                stop();
                gamesState = GamesState.GameOver;
            }
        }

        private void stop()
        {
            secondsTimer.Stop();
            changeCursor(Cursors.Arrow);
        }

        private void mainMenu_ButtonClicked(int Buttonindex)
        {
            if (Buttonindex == 0)
            {
                start(1);
            }
            else if (Buttonindex == 1)
            {
                //TetrisOneLove.kill();
                start(3);
            }
            else if (Buttonindex == 2)
            {
                start(2);
            }
            else if (Buttonindex == 3)
            {
                Application.Exit();
            }
        }

        private void GameOverMenu_ButtonClicked(int Buttonindex)
        {
            if (Buttonindex == 0)
            {
                start(playerPanels.Length);
            }
            else if (Buttonindex == 1)
            {
                stop();
                gamesState = GamesState.Menu;
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

        private void secondsTimer_Tick(object sender, EventArgs e)
        {
            secondsPassed++;
            foreach (var pnl in playerPanels)
            {
                pnl.setSecondsPassed(secondsPassed);
            }
        }

        private void invokeRepaintRequired()
        {
            if (RepaintRequired != null)
            {
                RepaintRequired();
            }
        }

        public void display(Graphics g, Size containerSize)
        {
            if (gamesState == GamesState.Menu)
            {
                mainMenu.display(g);
            }else
            {
                if (gamesState == GamesState.GameOver)
                {
                    var ClientRect = new Rectangle(0, 0, containerSize.Width, containerSize.Height);
                    ImageDrawer.fit(g, ClientRect, Config.GameOverImage);
                }
                foreach (var pnl in playerPanels)
                {
                    pnl.display(g, gamesState);
                }
                if (gamesState == GamesState.GameOver)
                {
                    GameOverMenu.display(g);
                }
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
            this.rect = rect;
            mainMenu.setRectangle(rect);
            setRectForPlayerPanels();
        }

        private void setRectForPlayerPanels()
        {
            if (playerPanels.Length <= 0)
            {
                return;
            }    
            int cols = playerPanels.Length >= 3 ? 3 : playerPanels.Length;
            int rows = (int)Math.Ceiling((double)playerPanels.Length / cols);
            int playerPanelWidth = this.rect.Width / cols;
            int playerPanelHeight = this.rect.Height / rows;
            for (int i = 0; i < playerPanels.Length; i++)
            {
                var pnl = playerPanels[i];
                int col = i % 3;
                int row = i / 3;
                var rect = new Rectangle(this.rect.Left + this.rect.Width * col / cols, this.rect.Top + this.rect.Height * row / rows,
                    playerPanelWidth, playerPanelHeight);
                pnl.setRectangle(rect);
            }
        }
    }
}
