using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SudokuSolver;

namespace SudokuSolverUI
{
    public partial class SudokuUI : Form
    {
        private string formtext;

        private LogViewer Logger;

        public SudokuUI()
        {
            InitializeComponent();
            ShowControls();
            updateUI(TestInput.EasyInput[0]);
            formtext = this.Text;
        }

        private Control[,] controls = new Control[9, 9];

        private void ShowLog(IList<StepInfo> steps)
        {
            foreach (var step in steps)
                Logger.ShowLog(step);
        }

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
                        MaxLength = 1,
                    });
                    (controls[i, j] as TextBox).GotFocus += Form1_GotFocus;
                }
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

            try
            {
                IList<StepInfo> steps = null;
                nums = new SudokuSolver.SudokuSolver(maze).SolveAndGetSteps(out steps);

                if (Logger != null) ShowLog(steps);

                var timeElapsed = DateTime.Now.Subtract(startTime);
                updateUI(nums);
                timeElapsedLbl.Text = timeElapsed.TotalMilliseconds + " ms.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("logic error :( ");
            }
        }

        private void updateUI(int[,] nums)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    controls[i, j].Text = nums[i, j] != 0 ? nums[i, j].ToString() : string.Empty;

            timeElapsedLbl.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timeElapsedLbl.Text = string.Empty;

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    controls[i, j].Text = string.Empty;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("written by Sujith." + Environment.NewLine + Application.ProductVersion);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateUI(TestInput.EvilInput[0]);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            updateUI(TestInput.EasyInput[0]);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            updateUI(TestInput.MediumInput[0]);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            updateUI(TestInput.HardInput[0]);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            updateUI(TestInput.EvilInput[0]);
        }

        private void showLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (Logger = LogViewer.GetInstance()).Show();
            Logger.BringToFront();
        }
    }
}
