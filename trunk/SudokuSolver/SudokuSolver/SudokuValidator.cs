using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public class SudokuValidator
    {
        public static bool validate(Maze maze)
        {
            foreach (var cell in maze.GetCells())
            {
                if (cell.Value == 0)
                    return false;

                if (maze.GetColumn(cell.Column).Count(x => x.Value == cell.Value) != 1)
                    return false;

                if (maze.GetRow(cell.Row).Count(x => x.Value == cell.Value) != 1)
                    return false;

                if (maze.GetSquare(cell.Row, cell.Column).Count(x => x.Value == cell.Value) != 1)
                    return false;
            }

            return true;
        }
    }
}
