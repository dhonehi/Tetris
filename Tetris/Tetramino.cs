using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Tetris {
    class Tetramino {
        private Point _currentPosition;
        private Point[] _currentShape;
        private Brush _currentColor;
        private bool _rotate;

        public Tetramino() {
            _currentPosition = new Point(0, 0);
            _currentColor = Brushes.Transparent;
            _currentShape = GetRandomShape();
        }

        public Brush GetCurrentColor() {
            return _currentColor;
        }

        public Point GetCurrentPosition() {
            return _currentPosition;
        }

        public Point[] GetCurrentShape() {
            return _currentShape;
        }

        public void MoveLeft() {
            --_currentPosition.X;
        }

        public void MoveRight() {
            ++_currentPosition.X;
        }

        public void MoveDown() {
            ++_currentPosition.Y;
        }

        public void MoveRotate() {
            if (_rotate) {
                for (int i = 0; i < _currentShape.Length; ++i) {
                    double x = _currentShape[i].X;
                    _currentShape[i].X = _currentShape[i].Y * -1;
                    _currentShape[i].Y = x;
                }
            }
        }

        private Point[] GetRandomShape() {
            Random rand = new Random();

            _rotate = true;

            switch (rand.Next() % 7) {
                case 0:
                    _currentColor = Brushes.Cyan;
                    return new Point[] {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(1,0),
                        new Point(2,0)
                    };
                case 1:
                    _currentColor = Brushes.Blue;
                    return new Point[] {
                        new Point(-1,1),
                        new Point(0,1),
                        new Point(1,1),
                        new Point(-1,0)
                    };
                case 2:
                    _currentColor = Brushes.Orange;
                    return new Point[] {
                       new Point(-1,1),
                        new Point(0,1),
                        new Point(1,1),
                        new Point(1,0)
                    };
                case 3:
                    _rotate = false;
                    _currentColor = Brushes.Yellow;
                    return new Point[] {
                        new Point(0,0),
                        new Point(0,1),
                        new Point(1,0),
                        new Point(1,1)
                    };
                case 4:
                    _currentColor = Brushes.Green;
                    return new Point[] {
                        new Point(0,1),
                        new Point(1,1),
                        new Point(1,0),
                        new Point(2,0)
                    };
                case 5:
                    _currentColor = Brushes.Purple;
                    return new Point[] {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,1),
                        new Point(1,1)
                    };
                case 6:
                    _currentColor = Brushes.Red;
                    return new Point[] {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,1),
                        new Point(1,1)
                    };
                default:
                    return null;
            }
        }
    }
}
