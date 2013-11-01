using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public partial class SudokuSolver
    {
        public bool PerformSecondLevel(List<CellState> cellStates)
        {
            foreach (var state in cellStates.ToList())
            {
                var sq = maze.GetSquare(state.Cell.Row, state.Cell.Column).Where(x => x.Value == 0).ToList();
                var ps = GetEffectivePossibilities(cellStates, state, sq);

                if (ps.Count == 1)
                {
                    state.Cell.Value = ps.First();
                    return true;
                }

                sq = maze.GetRow(state.Cell.Row).Where(x => x.Value == 0).ToList();
                ps = GetEffectivePossibilities(cellStates, state, sq);

                if (ps.Count == 1)
                {
                    state.Cell.Value = ps.First();
                    return true;
                }

                sq = maze.GetColumn(state.Cell.Column).Where(x => x.Value == 0).ToList();
                ps = GetEffectivePossibilities(cellStates, state, sq);

                if (ps.Count == 1)
                {
                    state.Cell.Value = ps.First();
                    return true;
                }
            }

            return false;
        }

        private List<int> GetEffectivePossibilities(List<CellState> cellStates, CellState cstate, List<Cell> cell)
        {
            cell.RemoveAll(x => x.Value != 0);
            var possibilities_within_group = new List<int>();

            foreach (var item in cell)
            {
                if (item.Column == cstate.Cell.Column && item.Row == cstate.Cell.Row)
                    continue;

                var itemstate = cellStates
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
    }
}
