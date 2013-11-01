using System;

namespace SudokuSolver
{
    public class TooHardToSolveException : Exception
    {
        public int[,] PresentState { get; set; }

        public int CellsSolved { get; set; }

        public TooHardToSolveException()
            : base("Too hard to solve for now.") { }
    }
}
