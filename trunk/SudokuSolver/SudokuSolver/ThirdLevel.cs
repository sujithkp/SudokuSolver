using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    class SpecInfo
    {
        public Cell Cell { get; set; }

        public List<int> Possibilities { get; set; }

        public List<int> TriedValues { get; set; }
    }

    public partial class SudokuSolver
    {
        private Stack<SpecInfo> Speculations = new Stack<SpecInfo>();

        void CellValueChangedWhileSpeculating(object sender, System.EventArgs e)
        {
            if (!SpeculationOn) return;

            var cell = sender as Cell;

            if (cell.Value == 0) return;

            if (Speculations.Any(x => x.Cell.Column == cell.Column && x.Cell.Row == cell.Row && x.Possibilities != null))
                return; // This column already exist

            if (Speculations.Any(x => x.Cell.Column == cell.Column && x.Cell.Row == cell.Row))
                throw new Exception("Logic error.");

            Speculations.Push(new SpecInfo()
            {
                Cell = cell,
                Possibilities = null,
                TriedValues = null,
            });
        }

        private void PerformSpeculation(List<CellState> cellState)
        {
            SpeculationOn = true;

            var smallestSpec = cellState.OrderBy(x => x.Possibilities.Count).First();

            var newSpeculation = new SpecInfo()
            {
                Cell = smallestSpec.Cell,
                Possibilities = smallestSpec.Possibilities,
                TriedValues = new List<int>(new int[] { smallestSpec.Possibilities.First() }),
            };

            Speculations.Push(newSpeculation);
            smallestSpec.Cell.Value = newSpeculation.TriedValues.First();

            if (needSteps)
            {
                Steps.Add(new StepInfo(
                    Step.SPECULATE,
                    smallestSpec.Cell.Row,
                    smallestSpec.Cell.Column,
                    smallestSpec.Cell.Value,
                    string.Format("The cell ({0},{1}) has these possibilities " + string.Join(",", smallestSpec.Possibilities),
                    smallestSpec.Cell.Row, smallestSpec.Cell.Column) + string.Format(" Lets speculate with {0}.", smallestSpec.Cell.Value)));

            }
        }

        private void HandleSpeculationError()
        {
            SpecInfo lastspeculatedCell = null;

            while ((lastspeculatedCell = Speculations.Pop()).Possibilities == null)
            {
                lastspeculatedCell.Cell.Value = 0;

                if (needSteps)
                {
                    Steps.Add(new StepInfo(
                        Step.RESET,
                        lastspeculatedCell.Cell.Row,
                        lastspeculatedCell.Cell.Column,
                        lastspeculatedCell.Cell.Value,
                        string.Format("Clearing the cell ({0},{1}) because of previous speculation error.",
                        lastspeculatedCell.Cell.Row, lastspeculatedCell.Cell.Column)));
                }
            }

            var triedValues = lastspeculatedCell.TriedValues;
            var nextvalue = lastspeculatedCell.Possibilities.FirstOrDefault(x => !triedValues.Contains(x));

            if (nextvalue == 0)
            {
                if (Speculations.Count == 0)
                    throw new Exception("Invalid sudoku !");

                HandleSpeculationError();
                return;
            }

            lastspeculatedCell.TriedValues.Add(nextvalue);
            Speculations.Push(lastspeculatedCell);

            lastspeculatedCell.Cell.Value = nextvalue;

            if (needSteps)
            {
                Steps.Add(new StepInfo(
                    Step.SPECULATE,
                    lastspeculatedCell.Cell.Row,
                    lastspeculatedCell.Cell.Column,
                    lastspeculatedCell.Cell.Value,
                    string.Format("Respeculating the cell ({0},{1}) with next value {2}.",
                    lastspeculatedCell.Cell.Row, lastspeculatedCell.Cell.Column, lastspeculatedCell.Cell.Value)));
            }

        }

    }
}
