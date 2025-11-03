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
        // Connection string die je via de constructor meegeeft
        private readonly string _cs;

        public FormScoresAwol(string connectionString)
        {
            InitializeComponent(); // Initialiseert de UI-componenten van het formulier
            _cs = connectionString; // Slaat de connection string lokaal op voor later gebruik
        }

        private void FormScoresAwol_Load(object sender, EventArgs e)
        {
            LoadTop10(); // Bij openen van het formulier meteen de Top 10 scores laden
        }

        private void btnRefreshAwol_Click(object sender, EventArgs e)
        {
            LoadTop10(); // Op de Refresh-knop: opnieuw de Top 10 ophalen en tonen
        }

        private void LoadTop10()
        {
            // SQL-query:
            // - pakt de 10 hoogste scores
            // - toont Username, Score en datum/tijd (CreatedAt) wanneer de score is gemaakt
            // - bij gelijke score komt de oudste (vroegste CreatedAt) eerst
            const string sql = @"
    SELECT TOP 10 u.Username AS [Username], s.Score AS [Score], s.CreatedAt AS [When]
    FROM dbo.Scores s
    JOIN dbo.Users u ON u.UserID = s.UserID
    ORDER BY s.Score DESC, s.CreatedAt ASC;";

            // using-blokken zorgen dat de connectie en adapter altijd netjes worden opgeruimd
            using (var con = new SqlConnection(_cs))
            using (var da = new SqlDataAdapter(sql, con))
            {
                var dt = new DataTable(); // Tabel in geheugen om de resultaten in te stoppen
                da.Fill(dt); // Voert de query uit en vult dt met de rijen

                // Bind de DataTable aan je DataGridView zodat de data zichtbaar wordt
                dataGridView1.DataSource = dt; // <-- was dgvScoresAwol

                // UI-instellingen voor de grid
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
            }
        }

    }
}
