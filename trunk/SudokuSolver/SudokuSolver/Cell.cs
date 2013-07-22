using System;

public class CellValueChangingEventArgs : EventArgs
{
    public int Value { get; private set; }

    public CellValueChangingEventArgs(int num)
    {
        Value = num;
    }
}

namespace SudokuSolver
{
    public class Cell
    {
        private int num;

        public event EventHandler<CellValueChangingEventArgs> Changed;

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
                {
                    if (Changed != null)
                        Changed(this, new CellValueChangingEventArgs(value));

                    num = value;
                }
            }
        }
    }
}
