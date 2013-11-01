using System;

namespace SudokuSolver
{
    public class Cell
    {
        private int num;

        public static event EventHandler<EventArgs> ValueChanged;

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
                num = value;
                NotifyValueChanged();
            }
        }

        private void NotifyValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }
    }
}
