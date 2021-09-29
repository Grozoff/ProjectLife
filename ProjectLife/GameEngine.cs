using System;

namespace ConsoleProjectLife
{
    internal class GameEngine
    {
        public uint CurrentGeneration { get; private set; }
        private bool[,] _field;
        private readonly int _rows;
        private readonly int _columns;

        public GameEngine(int rows, int columns, int density)
        {
            _rows = rows;
            _columns = columns;
            _field = new bool[columns, rows];
            Random _random = new Random();
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    _field[x, y] = _random.Next(density) == 0;
                }
            }
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var column = (x + i + _columns) % _columns;
                    var row = (y + j + _rows) % _rows;

                    var isSelfChecking = column == x && row == y;
                    var hasLife = _field[column, row];
                    if (hasLife && !isSelfChecking)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void NextGeneration()
        {
            var tempField = new bool[_columns, _rows];

            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var hasLife = _field[x, y];
                    if (!hasLife && neighboursCount == 3)
                    {
                        tempField[x, y] = true;
                    }
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        tempField[x, y] = false;
                    }
                    else
                    {
                        tempField[x, y] = _field[x, y];
                    }
                }
            }
            _field = tempField;
            CurrentGeneration++;
        }

        public bool[,] GetCurrentGeneration()
        {
            var result = _field.Clone() as bool[,];
            return result;
        }

        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _columns && y < _rows;
        }

        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
            {
                _field[x, y] = state;
            }
        }

        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, true);
        }

        public void DeleteCell(int x, int y)
        {
            UpdateCell(x, y, false);
        }
    }
}
