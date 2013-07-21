using System;

namespace SudokuSolver
{
    public class Cell
    {
        private int num;

        public Cell(int row, int column, int value)
        {
            this.Column = column;
            this.Row = row;
            this.Value = value;
        }

        public int Column { get; private set; }

        public int Row { get; private set; }

        public int Value
        {
            get { return num; }
            set
            {
                if (num != 0 && value != 0)
                    throw new Exception();

                if (num == 0 && value != 0)
                    num = value;
            }
        }
    }
}
