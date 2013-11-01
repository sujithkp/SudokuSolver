using System.Collections.Generic;

namespace SudokuSolver
{
    public class CellState
    {
        public CellState()
        {
            Possibilities = new List<int>();
        }

        public Cell Cell;

        public List<int> Possibilities;

        public bool Deleted { get; set; }
    }
}
