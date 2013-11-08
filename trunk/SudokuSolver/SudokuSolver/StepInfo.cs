
namespace SudokuSolver
{
    public enum Step { SET, SPECULATE, RESPECULATE, RESET };

    public class StepInfo
    {
        private static int Count = 1;

        public StepInfo(Step step, int row, int column, int value, string message)
        {
            ID = Count++;
            this.Step = step;
            this.Row = row;
            this.Column = column;
            this.Value = value;
            this.Message = message;
        }

        public static void ResetCounter()
        {
            Count = 1;
        }

        public Step Step { get; private set; }

        public int Row { get; private set; }

        public int Column { get; private set; }

        public int Value { get; private set; }

        public int ID { get; private set; }

        public string Message { get; private set; }
    }
}
