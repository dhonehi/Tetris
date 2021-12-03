using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tetris {
    class Board {
        private int _rows;
        private int _cols;
        private int _score;
        private int _linesFilled;
        private Tetramino _currentTetramino;
        private Tetramino _nextTetramino;
        private Label[,] _blockControls;

        public delegate void NewTetraminoHandler(Tetramino tetramino);
        public delegate void GameOverHandler();
        public event GameOverHandler NotifyGameOverHandler;
        public event NewTetraminoHandler NotifyNewTetraminoHandler;

        static private Brush _noBrush = Brushes.Transparent;
        static private Brush _silverBrush = Brushes.Gray;

        public Board(Grid tetrisGrid, GameOverHandler gameOverHandler, NewTetraminoHandler newTatraminoHandler) {
            NotifyGameOverHandler = gameOverHandler;
            NotifyNewTetraminoHandler = newTatraminoHandler;

            _rows = tetrisGrid.RowDefinitions.Count;
            _cols = tetrisGrid.ColumnDefinitions.Count;

            _score = 0;
            _linesFilled = 0;

            _blockControls = new Label[_cols, _rows];
            for (int i = 0; i < _cols; ++i) {
                for (int j = 0; j < _rows; ++j) {
                    Label label = new Label();

                    label.Background = _noBrush;
                    label.BorderBrush = _silverBrush;
                    label.BorderThickness = new Thickness(1, 1, 1, 1);

                    Grid.SetRow(label, j);
                    Grid.SetColumn(label, i);

                    tetrisGrid.Children.Add(label);
                    _blockControls[i, j] = label;
                }
            }

            _currentTetramino = new Tetramino();
            _nextTetramino = new Tetramino();

            NotifyNewTetraminoHandler(_nextTetramino);
        }

        public int GetScore() {
            return _score;
        }

        public int GetLines() {
            return _linesFilled;
        }

        private bool IsEndGame() {
            Point position = _currentTetramino.GetCurrentPosition();
            Point[] shape = _currentTetramino.GetCurrentShape();

            foreach (Point s in shape) {
                if (s.Y + position.Y == 1) return true;
            }

            return false;
        }

        private void CurrentTetraminoDraw() {
            Point position = _currentTetramino.GetCurrentPosition();
            Point[] shape = _currentTetramino.GetCurrentShape();
            Brush color = _currentTetramino.GetCurrentColor();

            foreach (Point s in shape) {
                _blockControls[
                    (int)(s.X + position.X) + ((_cols / 2) - 1),
                    (int)(s.Y + position.Y)
                    ].Background = color;
            }
        }

        private void CurrentTetraminoErase() {
            Point position = _currentTetramino.GetCurrentPosition();
            Point[] shape = _currentTetramino.GetCurrentShape();

            foreach (Point s in shape) {
                _blockControls[
                    (int)(s.X + position.X) + ((_cols / 2) - 1),
                    (int)(s.Y + position.Y)
                    ].Background = _noBrush;
            }
        }

        private void CheckRows() {
            bool full;

            for (int i = _rows - 1; i > 0; --i) {
                full = true;

                for (int j = 0; j < _cols; ++j) {
                    if (_blockControls[j, i].Background == _noBrush) {
                        full = false;
                    }
                }

                if (full) {
                    RemoveRow(i);
                    ++i;
                    _score += 100;
                    _linesFilled += 1;
                }
            }
        }

        private void RemoveRow(int row) {
            for (int i = row; i > 0; --i) {
                for (int j = 0; j < _cols; ++j) {
                    _blockControls[j, i].Background = _blockControls[j, i - 1].Background;
                }
            }
        }

        public void CurrentTetraminoMoveLeft() {
            Point position = _currentTetramino.GetCurrentPosition();
            Point[] shape = _currentTetramino.GetCurrentShape();
            bool move = true;
            CurrentTetraminoErase();
            foreach (Point s in shape) {
                if (((int)(s.X + position.X) + ((_cols / 2) - 1) - 1) < 0) {
                    move = false;
                } else if (
                    _blockControls[
                        (int)(s.X + position.X) + ((_cols / 2) - 1) - 1,
                        (int)(s.Y + position.Y)
                        ].Background != _noBrush) {
                    move = false;
                }
            }

            if (move) {
                _currentTetramino.MoveLeft();
                CurrentTetraminoDraw();
            } else {
                CurrentTetraminoDraw();
            }
        }

        public void CurrentTetraminoMoveRight() {
            Point position = _currentTetramino.GetCurrentPosition();
            Point[] shape = _currentTetramino.GetCurrentShape();

            bool move = true;

            CurrentTetraminoErase();

            foreach (Point s in shape) {
                if (((int)(s.X + position.X) + ((_cols / 2) - 1) + 1) >= _cols) {
                    move = false;

                } else if (
                    _blockControls[
                        (int)(s.X + position.X) + ((_cols / 2) - 1) + 1,
                        (int)(s.Y + position.Y)
                        ].Background != _noBrush) {
                    move = false;
                }
            }

            if (move) {
                _currentTetramino.MoveRight();
                CurrentTetraminoDraw();
            } else {
                CurrentTetraminoDraw();
            }
        }

        public void CurrentTetraminoMoveDown() {
            Point position = _currentTetramino.GetCurrentPosition();
            Point[] shape = _currentTetramino.GetCurrentShape();

            bool move = true;

            CurrentTetraminoErase();

            foreach (Point s in shape) {
                if (((int)(s.Y + position.Y) + 1) >= _rows) {
                    move = false;

                } else if (
                    _blockControls[
                        (int)(s.X + position.X) + ((_cols / 2) - 1),
                        (int)(s.Y + position.Y) + 1
                        ].Background != _noBrush) {
                    move = false;
                }
            }

            if (move) {
                _currentTetramino.MoveDown();
                CurrentTetraminoDraw();

            } else {
                CurrentTetraminoDraw();
                CheckRows();

                if (IsEndGame()) {
                    NotifyGameOverHandler();

                } else {
                    _currentTetramino = _nextTetramino;
                    _nextTetramino = new Tetramino();

                    NotifyNewTetraminoHandler(_nextTetramino);
                }
            }
        }

        public void CurrentTetramioMoveRotate() {
            Point position = _currentTetramino.GetCurrentPosition();
            Point[] s = new Point[4];
            Point[] shape = _currentTetramino.GetCurrentShape();

            bool move = true;

            shape.CopyTo(s, 0);

            CurrentTetraminoErase();

            for (int i = 0; i < s.Length; ++i) {
                double x = s[i].X;

                s[i].X = s[i].Y * -1;
                s[i].Y = Math.Abs(x);

                if (((int)(s[i].Y + position.Y) + 1) >= _rows) {
                    move = false;

                } else if (((int)(s[i].X + position.X) + ((_cols / 2) - 1)) < 0) {
                    move = false;

                } else if (((int)(s[i].X + position.X) + ((_cols / 2) - 1)) >= _rows) {
                    move = false;

                } else if (
                    _blockControls[
                        (int)(s[i].X + position.X) + ((_cols / 2) - 1),
                        (int)(s[i].Y + position.Y)
                        ].Background != _noBrush) {
                    move = false;
                }
            }
            if (move) {
                _currentTetramino.MoveRotate();
                CurrentTetraminoDraw();
            } else {
                CurrentTetraminoDraw();
            }
        }
    }
}
