using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Tetris {
    class NextTetraminoBoard {
        private Label[,] _blockControls;
        private int _rows;
        private int _cols;
        private Grid _nextTetraminoGrid;
        private Tetramino _prevTetramino;
        public NextTetraminoBoard(Grid nextTetraminoGrid) {
            _nextTetraminoGrid = nextTetraminoGrid;

            _rows = nextTetraminoGrid.RowDefinitions.Count;
            _cols = nextTetraminoGrid.ColumnDefinitions.Count;

            _blockControls = new Label[_rows, _cols];

            for (int i = 0; i < _rows; ++i) {
                for (int j = 0; j < _cols; ++j) {
                    Label label = new Label();

                    label.Background = Brushes.Transparent;
                    label.BorderBrush = Brushes.Gray;

                    Grid.SetRow(label, j);
                    Grid.SetColumn(label, i);

                    _nextTetraminoGrid.Children.Add(label);

                    _blockControls[i, j] = label;
                }
            }
        }

        public void ShowNextTetramino(Tetramino tetramino) {
            if (_prevTetramino != null) {
                ErasePrevTetramino();
            }

            _prevTetramino = tetramino;

            Point[] shape = tetramino.GetCurrentShape();
            Brush color = tetramino.GetCurrentColor();

            foreach (Point s in shape) {
                _blockControls[
                    (int)((s.X + _cols / 2) - 1),
                    (int)(s.Y + 1)
                    ].Background = color;
            }
        }

        public void Clear() {
            _nextTetraminoGrid.Children.Clear();
        }

        private void ErasePrevTetramino() {
            Point[] shape = _prevTetramino.GetCurrentShape();

            foreach (Point s in shape) {
                _blockControls[
                     (int)((s.X + _cols / 2) - 1),
                    (int)(s.Y + 1)
                    ].Background = Brushes.Transparent;
            }
        }
    }
}
