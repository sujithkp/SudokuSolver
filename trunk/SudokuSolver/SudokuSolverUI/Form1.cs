using System;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolverUI
{
    public partial class Form1 : Form
    {
        private string formtext;

        public Form1()
        {
            InitializeComponent();
            ShowControls();
            updateUI(TestInput.EvilInput[0]);
            formtext = this.Text;
        }

        private Control[,] controls = new Control[9, 9];

        private void ShowControls()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    this.Controls.Add(controls[i, j] = new TextBox()
                    {
                        Width = 20,
                        Left = (j == 0) ? 40 : controls[i, j - 1].Left + controls[i, j - 1].Width + 5,
                        Top = (i == 0) ? 30 : controls[i - 1, j].Top + controls[i - 1, j].Height + 5,
                        Text = "",
                        Font = new Font(FontFamily.GenericSerif, 15),
                    });
                    (controls[i, j] as TextBox).GotFocus += Form1_GotFocus;
                    (controls[i, j] as TextBox).Leave += Form1_Leave;
                }
        }

        void Form1_Leave(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl.Text == string.Empty)
                ctrl.Text = "0";
        }

        void Form1_GotFocus(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl.Text == "0")
                ctrl.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[,] nums = new int[9, 9];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    nums[i, j] = int.Parse(controls[i, j].Text == string.Empty ? "0" : controls[i, j].Text);

            var startTime = DateTime.Now;
            var maze = new SudokuSolver.Maze(nums);
            var timeElapsed = DateTime.Now.Subtract(startTime);

            try
            {
                nums = new SudokuSolver.SudokuSolver(maze).Solve();
                updateUI(nums);
            }
            catch (SudokuSolver.TooHardToSolveException ex)
            {
                updateUI(ex.PresentState);
                MessageBox.Show("too hard to solve.");
            }

            this.Text = formtext + "  -  " + timeElapsed.TotalMilliseconds.ToString() + " ms";
        }

        private void updateUI(int[,] nums)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    controls[i, j].Text = nums[i, j] != 0 ? nums[i, j].ToString() : string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    controls[i, j].Text = string.Empty;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sudoku Solver \n\t by Sujith." + Environment.NewLine + Environment.NewLine + "(this is not final version)");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateUI(TestInput.HardInput[0]);
        }
    }
}
