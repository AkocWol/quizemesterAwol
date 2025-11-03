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
    public partial class FormAdminAwol : Form
    {
        // Connection string (via constructor meegegeven)
        private readonly string _cs;

        // Adapters voor vragen (QuizQuestions) en users (Users)
        private SqlDataAdapter _qa, _ua;

        // In-memory tabellen die aan de DataGridViews worden gebonden
        private DataTable _qdt = new DataTable();
        private DataTable _udt = new DataTable();

        public FormAdminAwol(string connectionString)
        {
            InitializeComponent(); // Initialiseert alle UI-componenten
            _cs = connectionString; // Bewaar de connection string voor DB-toegang
        }

        private void FormAdminAwol_Load(object sender, EventArgs e)
        {
            LoadQuestions(); // Laad admin-overzicht met quizvragen
            LoadUsers(); // Laad admin-overzicht met gebruikers
        }

        private void LoadQuestions()
        {
            // Haal alle quizvragen op en toon ze in de grid
            const string sql = @"SELECT QuestionID, QuestionText, OptionA, OptionB, OptionC, OptionD,
                                CorrectOption, IsActive, CategoryID
                         FROM dbo.QuizQuestions
                         ORDER BY QuestionID";

            _qa = new SqlDataAdapter(sql, _cs); // Data ophalen en straks ook wegschrijven
            new SqlCommandBuilder(_qa); // auto INSERT/UPDATE/DELETE

            _qdt.Clear(); // Leeg de huidige in-memory tabel
            _qa.Fill(_qdt); // Vul met data uit de database

            dgvQuestionsAwol.DataSource = _qdt; // Koppel aan de vragen-DataGridView

            // Grid-instellingen
            dgvQuestionsAwol.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQuestionsAwol.AllowUserToAddRows = true; // Nieuwe rijen mogen toegevoegd
            dgvQuestionsAwol.AllowUserToDeleteRows = true; // Rijen mogen verwijderds
        }

        private void LoadUsers()
        {
            // Haal users op (incl. IsAdmin) en toon in users-grid
            const string sql = @"SELECT UserID, Username, IsAdmin FROM dbo.Users ORDER BY Username";

            _ua = new SqlDataAdapter(sql, _cs); // Adapter voor users
            new SqlCommandBuilder(_ua); // Auto-commando’s voor INSERT/UPDATE/DELETE

            _udt.Clear(); // Reset in-memory tabel
            _ua.Fill(_udt); // Vul met data

            dataGridView1.DataSource = _udt; // Koppel aan de users-DataGridView

            // Grid-instellingen
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
        }

        private void btnSaveQuestionsAwol_Click(object sender, EventArgs e)
        {
            try
            {
                dgvQuestionsAwol.EndEdit(); // Sluit bewerking in de grid af zodat wijzigingen in _qdt staan

                // Simpele validatie per rij (behalve Deleted)
                foreach (DataRow r in _qdt.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;

                    // Elke vraag en alle 4 opties moeten gevuld zijn
                    if (string.IsNullOrWhiteSpace(Convert.ToString(r["QuestionText"])) ||
                        string.IsNullOrWhiteSpace(Convert.ToString(r["OptionA"])) ||
                        string.IsNullOrWhiteSpace(Convert.ToString(r["OptionB"])) ||
                        string.IsNullOrWhiteSpace(Convert.ToString(r["OptionC"])) ||
                        string.IsNullOrWhiteSpace(Convert.ToString(r["OptionD"])))
                        throw new Exception("Each question and all 4 options must be filled.");

                    // CorrectOption moet A/B/C/D zijn
                    var co = Convert.ToString(r["CorrectOption"]);
                    if (co != "A" && co != "B" && co != "C" && co != "D")
                        throw new Exception("CorrectOption must be A, B, C or D.");
                }

                _qa.Update(_qdt); // Schrijf alle changes (insert/update/delete) naar de database
                MessageBox.Show("Questions saved.");
                LoadQuestions(); // Herladen zodat IDs en status up-to-date zijn
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save questions failed:\n" + ex.Message); // Toon foutmelding
            }
        }

        private void btnAddQuestionsAwol_Click(object sender, EventArgs e)
        {
            // Voeg een nieuwe default-rij toe in de in-memory vragen-tabel
            var row = _qdt.NewRow();
            row["QuestionText"] = "New question...";
            row["OptionA"] = "A";
            row["OptionB"] = "B";
            row["OptionC"] = "C";
            row["OptionD"] = "D";
            row["CorrectOption"] = "A";     // Keuze A/B/C/D
            row["IsActive"] = true; // Direct actief
            row["CategoryID"] = DBNull.Value; // of een bestaande CategoryID
            _qdt.Rows.Add(row); // In memory toegevoegd (nog niet in DB)

            // Zet focus direct op de QuestionText-cel van de nieuwe rij en start edit
            dgvQuestionsAwol.CurrentCell = dgvQuestionsAwol
                .Rows[dgvQuestionsAwol.Rows.Count - 1]
                .Cells["QuestionText"];
            dgvQuestionsAwol.BeginEdit(true);
        }

        private void btnDeleteQuestionAwol_Click(object sender, EventArgs e)
        {
            // Geen selectie of nieuwe lege rij? Dan niet verwijderen
            if (dgvQuestionsAwol.CurrentRow == null || dgvQuestionsAwol.CurrentRow.IsNewRow) return;

            // Bevestiging vragen
            if (MessageBox.Show("Delete selected question?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            // Verwijder de geselecteerde rij (wordt pas echt uit DB gehaald bij Save)
            dgvQuestionsAwol.Rows.RemoveAt(dgvQuestionsAwol.CurrentRow.Index);
        }

        private void btnDeleteUserAwol_Click(object sender, EventArgs e)
        {
            // Geen geldige geselecteerde rij? Stop
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow) return;

            // Pak UserID uit de huidige rij (vereist dat kolom "UserID" in de grid aanwezig is)
            object idObj = dataGridView1.CurrentRow.Cells["UserID"].Value;
            if (idObj == null || idObj == DBNull.Value)
            {
                MessageBox.Show("Select a valid user row first.");
                return;
            }
            int userId = Convert.ToInt32(idObj);

            // Dubbele confirm: user + ALLE scores worden verwijderd
            if (MessageBox.Show($"Delete user {userId} and ALL their scores?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                // Handmatige delete met transactie:
                // 1) Scores weg, 2) User weg, 3) Commit
                using (var con = new SqlConnection(_cs))
                {
                    con.Open();
                    using (var tx = con.BeginTransaction())
                    {
                        // Verwijder scores van deze user
                        using (var cmd = new SqlCommand("DELETE FROM dbo.Scores WHERE UserID=@id;", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", userId);
                            cmd.ExecuteNonQuery();
                        }
                        // Verwijder user zelf
                        using (var cmd = new SqlCommand("DELETE FROM dbo.Users WHERE UserID=@id;", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", userId);
                            cmd.ExecuteNonQuery();
                        }
                        tx.Commit(); // Alles gelukt → commit de wijzigingen
                    }
                }

                // UI herladen zodat de verwijderde user niet meer zichtbaar is
                LoadUsers();
                MessageBox.Show("User (and scores) deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed:\n" + ex.Message); // Toon foutmelding
            }
        }

        private void btnSaveUsersAwol_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.EndEdit(); // Zorg dat alle grid-edits in _udt staan (commit in memory)
                _ua.Update(_udt); // Schrijf alle wijzigingen naar de database
                MessageBox.Show("Users saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save users failed:\n" + ex.Message); // Toon foutmelding
            }
        }
    }
}
