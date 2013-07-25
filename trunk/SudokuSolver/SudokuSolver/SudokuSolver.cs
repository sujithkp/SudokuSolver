using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public class SudokuSolver
    {
        class CellState
        {
            public CellState()
            {
                Possibilities = new List<int>();
            }

            public Cell Cell;

            public List<int> Possibilities;

            public bool Deleted { get; set; }
        }

        private List<int> posblties;

        private List<CellState> CellStates;

        private readonly Maze maze;

        private bool BruteForceHasEnabled { get; set; }

        public SudokuSolver(Maze maze)
        {
            this.posblties = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            this.CellStates = new List<CellState>();
            this.maze = maze;
            BruteForceHasEnabled = false;
        }

        public int[,] Solve()
        {
            var cells = this.maze.GetEmptyCells();

            foreach (var cell in cells)
            {
                CellStates.Add(new CellState()
                {
                    Cell = cell,
                    Possibilities = new List<int>(this.posblties)
                });

                cell.Changed += new EventHandler<CellValueChangingEventArgs>(cell_Changed);
            }

            privateSolve(CellStates);

            return maze.ToArray();
        }

        void cell_Changed(object sender, CellValueChangingEventArgs e)
        {
            if (!BruteForceHasEnabled)
                return;

            var cell = sender as Cell;

            if (maze.IsInValid(e.Value, cell.Row, cell.Column))
                throw new Exception();
        }

        private List<int> GetEffectivePossibilities(CellState cstate, List<Cell> cell)
        {
            cell.RemoveAll(x => x.Value != 0);
            var possibilities_within_group = new List<int>();

            foreach (var item in cell)
            {
                if (item.Column == cstate.Cell.Column && item.Row == cstate.Cell.Row)
                    continue;

                var itemstate = this.CellStates
                    .Where(c => c.Cell.Row == item.Row && c.Cell.Column == item.Column)
                    .Single();

                possibilities_within_group.AddRange(itemstate.Possibilities);
            }

            var effectivepossibilities = new List<int>();

            foreach (var item in cstate.Possibilities)
            {
                if (!possibilities_within_group.Contains(item))
                {
                    effectivepossibilities.Add(item);
                }
            }
            return effectivepossibilities.Count == 1 ?
                effectivepossibilities : cstate.Possibilities;
        }

        /// <summary>
        /// Updates the possibilities of each cell.
        /// </summary>
        /// <param name="cellStates">Cellstates </param>
        private void update(List<CellState> cellStates)
        {
            cellStates.RemoveAll(x => x.Cell.Value != 0);
            var startlength = 0;

            do
            {
                startlength = cellStates.Count;

                var cellssolved = new List<CellState>();
                foreach (var cellState in cellStates)
                {
                    var cell = cellState.Cell;
                    var p = cellState.Possibilities;

                    maze.GetRow(cell.Row)
                        .ForEach(x => { if (p.Contains(x.Value)) { p.Remove(x.Value); } });

                    maze.GetColumn(cell.Column)
                        .ForEach(x => { if (p.Contains(x.Value)) { p.Remove(x.Value); } });

                    maze.GetSquare(cell.Row, cell.Column)
                        .ForEach(x => { if (p.Contains(x.Value)) { p.Remove(x.Value); } });

                    if (p.Count == 1)
                    {
                        cell.Value = p.First();
                        cellssolved.Add(cellState);
                        continue;
                    }
                }

                foreach (var solvedcell in cellssolved)
                {
                    cellStates.Remove(solvedcell);
                    solvedcell.Deleted = true;
                }

            } while (cellStates.Count != startlength);
        }

        private void solvewithchecks(List<CellState> state, bool checkon)
        {
            int startlength = state.Count;
            update(state);

            foreach (var cstate in new List<CellState>(state))
            {
                if (cstate.Deleted)
                    continue;

                var cellsing = maze.GetSquare(cstate.Cell.Row, cstate.Cell.Column).Where(x => x.Value == 0).ToList();
                var p = GetEffectivePossibilities(cstate, cellsing);

                if (p.Count == 0)
                    continue; //should not come here.

                if (p.Count == 1)
                {
                    cstate.Cell.Value = p.First();
                    update(state);
                    continue;
                }

                cellsing = maze.GetRow(cstate.Cell.Row).Where(x => x.Value == 0).ToList();
                var q = GetEffectivePossibilities(cstate, cellsing);
                p = q;

                if (p.Count == 1)
                {
                    cstate.Cell.Value = p.First();
                    update(state);
                    continue;
                }

                cellsing = maze.GetColumn(cstate.Cell.Column).Where(x => x.Value == 0).ToList();
                var r = GetEffectivePossibilities(cstate, cellsing);
                p = r;

                if (p.Count == 1)
                {
                    cstate.Cell.Value = p.First();
                    update(state);
                    continue;
                }
            }

            if (state.Count != 0 && state.Count == startlength)
                if (!TryWithBruttForce(state))
                   throw new TooHardToSolveException() { PresentState = maze.ToArray(), };

            if (state.Count > 0 && state.Count != startlength)
                solvewithchecks(state, checkon);
        }

        /// <summary>
        /// not implemented.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool TryWithBruttForce(List<CellState> state)
        {
            BruteForceHasEnabled = true;

            //var st = state.OrderBy(x => x.Possibilities.Count).ToList();



            BruteForceHasEnabled = false;
            return false;
        }

        /// <summary>
        /// This is a reursive function Recursive function. 
        /// computes possibilities of each cell.
        /// computes possibilities of each cell based on its row, column and group.
        /// Throws TooHardToSolveException if the sudoku is too complex.
        /// </summary>
        /// <param name="state"></param>
        private void privateSolve(List<CellState> state)
        {
            solvewithchecks(state, false);
        }
    }
}
