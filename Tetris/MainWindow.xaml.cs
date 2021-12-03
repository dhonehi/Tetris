using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Tetris {
    public partial class MainWindow : Window {
        DispatcherTimer timer;
        Board myBoard;
        NextTetraminoBoard nextTetraminoBoard;
        SoundPlayer _player;
        bool _isCreatedGame = false;

        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) {
            _player = new SoundPlayer("tetris-music.wav");

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(GameTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);

            restartGameButton.Visibility = Visibility.Hidden;
        }

        private void GameStart() {
            _player.Play();

            _isCreatedGame = true;

            MainGrid.Children.Clear();

            if (nextTetraminoBoard != null) nextTetraminoBoard.Clear();

            nextTetraminoBoard = new NextTetraminoBoard(NextTetraminoGrid);
            myBoard = new Board(MainGrid, GameOver, ShowNextTetramino);
            timer.Start();

            restartGameButton.Visibility = Visibility.Visible;
        }

        private void GameResume() {
            _player.Play();
            timer.Start();
        }

        private void GamePause() {
            timer.Stop();
            _player.Stop();
        }

        private void GameOver() {
            nextTetraminoBoard.Clear();
            _isCreatedGame = false;
            timer.Stop();
            _player.Stop();
            contolGameButton.Content = "Start";
            restartGameButton.Visibility = Visibility.Hidden;
            MessageBox.Show("Game over");
        }

        void GameTick(object sender, EventArgs e) {
            Score.Content = myBoard.GetScore().ToString();
            Lines.Content = myBoard.GetLines().ToString();
            myBoard.CurrentTetraminoMoveDown();
        }

        void ShowNextTetramino(Tetramino tetramino) {
            nextTetraminoBoard.ShowNextTetramino(tetramino);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Left:
                    if (timer.IsEnabled) myBoard.CurrentTetraminoMoveLeft();
                    break;
                case Key.Right:
                    if (timer.IsEnabled) myBoard.CurrentTetraminoMoveRight();
                    break;
                case Key.Down:
                    if (timer.IsEnabled) myBoard.CurrentTetraminoMoveDown();
                    break;
                case Key.Up:
                    if (timer.IsEnabled) myBoard.CurrentTetramioMoveRotate();
                    break;
                default:
                    break;
            }
        }

        private void contolGameButton_Click(object sender, RoutedEventArgs e) {
            if (timer.IsEnabled) {
                contolGameButton.Content = "Start";
                GamePause();
            } else {
                contolGameButton.Content = "Stop";
                if (_isCreatedGame) GameResume();
                else GameStart();
            }
        }

        private void restartGameButton_Click(object sender, RoutedEventArgs e) {
            GameStart();
            contolGameButton.Content = "Stop";
        }
    }
}
