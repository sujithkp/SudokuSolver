using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public class Maze
    {
        private Cell[,] maze;

        public Maze(int[,] maze)
        {
            int rows = maze.GetUpperBound(0) + 1;
            int cols = maze.GetUpperBound(1) + 1;

            if (rows != cols)
                throw new Exception("invalid maze!");

            this.maze = new Cell[rows, cols];
            fillMaze(maze);
        }

        public int[,] ToArray()
        {
            var nums = new int[maze.GetUpperBound(0) + 1, maze.GetUpperBound(1) + 1];
            for (int i = 0; i <= maze.GetUpperBound(0); i++)
                for (int j = 0; j <= maze.GetUpperBound(1); j++)
                    nums[i, j] = maze[i, j].Value;

            return nums;
        }

        private void fillMaze(int[,] maze)
        {
            for (int i = 0; i <= maze.GetUpperBound(0); i++)
                for (int j = 0; j <= maze.GetUpperBound(1); j++)
                    this.maze[i, j] = new Cell(i, j, maze[i, j]);
        }

        public List<Cell> GetRow(int row)
        {
            var list = new List<Cell>();

            for (int col = 0; col <= this.maze.GetUpperBound(0); col++)
                list.Add(this.maze[row, col]);

            return list;
        }

        private List<int> GetMissingValues(IEnumerable<int> values)
        {
            var result = new List<int>();

            for (int i = 1; i <= 9; i++)
                if (!values.Contains(i))
                    result.Add(i);

            return result;
        }

        public List<int> GetMissingValuesInRow(int row)
        {
            return GetMissingValues(GetRow(row).Select(x => x.Value));
        }

        public List<int> GetMissingValuesInCol(int col)
        {
            return GetMissingValues(GetColumn(col).Select(x => x.Value));
        }

        public List<int> GetMissingValuesInSquare(int row, int col)
        {
            return GetMissingValues(GetSquare(row, col).Select(x => x.Value));
        }

        public List<Cell> GetColumn(int col)
        {
            var list = new List<Cell>();

            for (int row = 0; row <= this.maze.GetUpperBound(0); row++)
                list.Add(this.maze[row, col]);

            return list;
        }

        public List<Cell> GetSquare(int row, int col)
        {
            int startx = col / 3 * 3;
            int starty = row / 3 * 3;

            var list = new List<Cell>();

            for (int i = startx; i < startx + 3; i++)
                for (int j = starty; j < starty + 3; j++)
                    list.Add(this.maze[j, i]);

            return list;
        }

        public void print()
        {
            /*
            for (int i = 0; i < 9; Console.WriteLine(""), i++)
                GetRow(i).ForEach(x => Console.Write(x.Value + "\t"));
            */
        }

        public bool IsInValid(int val, int row, int col)
        {
            return !(GetMissingValuesInRow(row).Contains(val)
                || GetMissingValuesInCol(col).Contains(val)
                || GetMissingValuesInSquare(row, col).Contains(val));
        }

        public List<Cell> GetEmptyCells()
        {
            var list = new List<Cell>();

            for (int i = 0; i <= this.maze.GetUpperBound(0); i++)
                for (int j = 0; j <= this.maze.GetUpperBound(1); j++)
                    if (maze[i, j].Value == 0)
                        list.Add(this.maze[i, j]);

            return list;
        }
    }
}
