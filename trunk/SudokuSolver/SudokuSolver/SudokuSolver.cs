using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;

namespace SudokuSolver
{
    public partial class SudokuSolver
    {
        private List<int> InitialPossibilities = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

        private bool SpeculationOn;

        private readonly Maze maze;

        public SudokuSolver(Maze maze)
        {
            this.maze = maze;
        }

        public int[,] Solve()
        {
            Cell.ValueChanged += new EventHandler<EventArgs>(CellValueChangedWhileSpeculating);
            Cell.ValueChanged += new EventHandler<EventArgs>(Cell_ValueChanged);

            Process();

            Cell.ValueChanged -= new EventHandler<EventArgs>(CellValueChangedWhileSpeculating);
            Cell.ValueChanged -= new EventHandler<EventArgs>(Cell_ValueChanged);


            SudokuValidator.validate(maze);
            return maze.ToArray();
        }

        void Cell_ValueChanged(object sender, EventArgs e)
        {
            var cell = sender as Cell;

            Debug.WriteLine("cell at " + cell.Row + "," + cell.Column + " : " + cell.Value);
        }

        private List<int> GetPosibilities(Cell cell)
        {
            var initialps = this.InitialPossibilities.ToList();

            maze.GetRow(cell.Row).ForEach(x => initialps.Remove(x.Value));
            maze.GetColumn(cell.Column).ForEach(x => initialps.Remove(x.Value));
            maze.GetSquare(cell.Row, cell.Column).ForEach(x => initialps.Remove(x.Value));

            return initialps;
        }

        private List<CellState> GetCellState()
        {
            var cells = this.maze.GetEmptyCells();
            var needtoUpdateCellState = cells.Count > 0;

            List<CellState> cellStates = new List<CellState>();

            while (needtoUpdateCellState)
            {
                needtoUpdateCellState = false;
                var cstate = new List<CellState>();

                foreach (var cell in (cells = cells.Where(x => x.Value == 0).ToList()))
                {
                    var cs = new CellState()
                    {
                        Cell = cell,
                        Possibilities = GetPosibilities(cell),
                    };

                    if (needtoUpdateCellState = (cs.Possibilities.Count == 1))
                    {
                        cell.Value = cs.Possibilities.First();
                        break;
                    }

                    if (cs.Possibilities.Count == 0)
                    {
                        HandleCellStateError(cs);
                        cells = this.maze.GetEmptyCells();
                        needtoUpdateCellState = true;
                        break;
                    }

                    cstate.Add(cs);
                }

                cellStates = cstate;
            }

            return cellStates;
        }

        private void HandleCellStateError(CellState cs)
        {
            if (SpeculationOn)
                HandleSpeculationError();
        }

        private void Process()
        {
            List<CellState> cellStates = null;
            int InitialNumberOfEmptyCells = this.maze.GetEmptyCells().Count();

            while ((cellStates = GetCellState()).Count > 0)
            {
                var numberofEmptyCells = cellStates.Count;
                numberofEmptyCells -= PerformSecondLevel(cellStates) ? 1 : 0;


                if (numberofEmptyCells == InitialNumberOfEmptyCells)
                {
                    PerformSpeculation(cellStates);
                    numberofEmptyCells--;
                }

                if (numberofEmptyCells == InitialNumberOfEmptyCells)
                    break;

                InitialNumberOfEmptyCells = numberofEmptyCells;
            }
        }

    }
}
