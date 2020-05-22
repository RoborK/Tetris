using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
        }
        DispatcherTimer Timer;
        Table myBoard;
        int GameSpeed = 1000;
        int SpeedStep = 50;
        int MaxLevel;
        void GameWindow_Initialized(object sender, EventArgs e)
        {
            MaxLevel = GameSpeed / SpeedStep;
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(GameTick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, GameSpeed);
            GameStart();
        }
        private void GameStart()
        {
            MainGrid.Children.Clear();
            MainGrid.Background = GridField.fieldBrush;
            myBoard = new Table(MainGrid, NextFigGrid);
            GameSpeed = 1000;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, GameSpeed);
            Timer.Start();
            LvlText.Content = "Level: " + myBoard.LVL;
        }
        private void GamePause()
        {
            if (Timer.IsEnabled) Timer.Stop();
            else Timer.Start();
        }
        private void GameOver()
        {
            Timer.Stop();
            MessageBox.Show(
                String.Format("Игра окончена.\nНабрано очков: {0}\nУдалено линий: {1}.", myBoard.Score, myBoard.Lines),
                "Конец игры!",
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
           if (RecordsWindow.result.Count < 10 || RecordsWindow.result.Last().Score< myBoard.Score)
            {
                InputName recordsWindow = new InputName(myBoard.Score, myBoard.Lines);
                recordsWindow.Owner = this;
                recordsWindow.ShowDialog();
            }
            Close();
        }
        void GameTick(object sender, EventArgs e)
        {
            if (MaxLevel > myBoard.LVL)
                LevelProgressBar.Value = myBoard.GetLvlProc();
            Score.Content = "Score: " + (myBoard.Score).ToString();
            Lines.Content = "Lines: " + myBoard.Lines.ToString();
            myBoard.CurTetraminoMovDown();

            if (myBoard.GameOver)
                GameOver();
            if (myBoard.LvlUp && MaxLevel > myBoard.LVL)
            {
                Timer.Interval = new TimeSpan(0, 0, 0, 0, GameSpeed - SpeedStep * (myBoard.LVL - 1));
                myBoard.LvlUp = false;
                LvlText.Content = "Level: " + myBoard.LVL;
                if (MaxLevel == myBoard.LVL)
                {
                    myBoard.StepLvl = int.MaxValue;
                    LevelProgressBar.Value = LevelProgressBar.Maximum;
                }
            }
        }
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (Timer.IsEnabled && myBoard.GameOver)
                GameOver();
            switch (e.Key)
            {
                case Key.Left:
                    if (Timer.IsEnabled) myBoard.CurentTetraminoMoveLeft();
                    break;
                case Key.Right:
                    if (Timer.IsEnabled) myBoard.CurTetraminoMoveRight();
                    break;
                case Key.Down:
                    if (Timer.IsEnabled) myBoard.CurTetraminoMovDown(true);
                    break;
                case Key.Up:
                    if (Timer.IsEnabled) myBoard.CurTetraminoMoveRotate();
                    break;
                case Key.Enter:
                    GameStart();
                    break;
                case Key.Space:
                    GamePause();
                    break;
                case Key.Escape: Button_Click(sender, e);
                    break;

                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameOver();
            Close();
        }
    }
}
