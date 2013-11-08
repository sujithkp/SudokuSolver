using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SudokuSolver;

namespace SudokuSolverUI
{
    public partial class LogViewer : Form
    {
        private static LogViewer Instance = null;

        private DataTable dataTable = null;

        private LogViewer()
        {
            InitializeComponent();

            dataTable = new DataTable("Log");
            dataTable.Columns.Add("Action");
            dataTable.Columns.Add("Message");
            dataTable.Columns.Add("Cell");
            dataTable.Columns.Add("Value");

            var dataset = new DataSet();
            dataset.Tables.Add(dataTable);
            dataGridView1.DataSource = dataset;
            dataGridView1.DataMember = "Log";
            dataGridView1.ReadOnly = true;

            dataGridView1.Columns["Message"].Width = 500;
            dataGridView1.Refresh();
        }

        public void Clear()
        {
            dataTable.Rows.Clear();
        }

        public void ShowLog(StepInfo msg)
        {
            var row = dataTable.NewRow();
            row["Action"] = msg.Step;
            row["Message"] = msg.Message;
            row["Cell"] = "[" + msg.Row + "," + msg.Column + "]";
            row["Value"] = msg.Value;

            dataTable.Rows.Add(row);
        }

        public static LogViewer GetInstance()
        {
            if (Instance == null || Instance.IsDisposed)
                return Instance = new LogViewer();

            return Instance;
        }
    }
}
