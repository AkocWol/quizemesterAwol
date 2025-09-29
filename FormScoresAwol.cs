using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quizemesterAwol
{
    public partial class FormScoresAwol : Form
    {
        private readonly string _cs;

        public FormScoresAwol(string connectionString)
        {
            InitializeComponent();
            _cs = connectionString;
        }

        private void FormScoresAwol_Load(object sender, EventArgs e)
        {
            LoadTop10();
        }

        private void btnRefreshAwol_Click(object sender, EventArgs e)
        {
            LoadTop10();
        }

        private void LoadTop10()
        {
            const string sql = @"
    SELECT TOP 10 u.Username AS [Username], s.Score AS [Score], s.CreatedAt AS [When]
    FROM dbo.Scores s
    JOIN dbo.Users u ON u.UserID = s.UserID
    ORDER BY s.Score DESC, s.CreatedAt ASC;";

            using (var con = new SqlConnection(_cs))
            using (var da = new SqlDataAdapter(sql, con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt; // <-- was dgvScoresAwol
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
            }
        }

    }
}
