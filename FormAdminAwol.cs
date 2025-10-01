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
        private readonly string _cs;
        private SqlDataAdapter _qa, _ua;
        private DataTable _qdt = new DataTable();
        private DataTable _udt = new DataTable();

        public FormAdminAwol(string connectionString)
        {
            InitializeComponent();
            _cs = connectionString;
        }

        private void FormAdminAwol_Load(object sender, EventArgs e)
        {
            LoadQuestions();
            LoadUsers();
        }

        private void LoadQuestions()
        {
            const string sql = @"SELECT QuestionID, QuestionText, OptionA, OptionB, OptionC, OptionD,
                                CorrectOption, IsActive, CategoryID
                         FROM dbo.QuizQuestions
                         ORDER BY QuestionID";

            _qa = new SqlDataAdapter(sql, _cs);
            new SqlCommandBuilder(_qa);              // auto INSERT/UPDATE/DELETE
            _qdt.Clear();
            _qa.Fill(_qdt);
            dgvQuestionsAwol.DataSource = _qdt;
            dgvQuestionsAwol.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQuestionsAwol.AllowUserToAddRows = true;
            dgvQuestionsAwol.AllowUserToDeleteRows = true;
        }

        private void LoadUsers()
        {
            const string sql = @"SELECT UserID, Username, IsAdmin FROM dbo.Users ORDER BY Username";
            _ua = new SqlDataAdapter(sql, _cs);
            new SqlCommandBuilder(_ua);
            _udt.Clear();
            _ua.Fill(_udt);
            dataGridView1.DataSource = _udt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
        }

        private void btnSaveQuestionsAwol_Click(object sender, EventArgs e)
        {
            try
            {
                dgvQuestionsAwol.EndEdit();

                // simpele validatie
                foreach (DataRow r in _qdt.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    if (string.IsNullOrWhiteSpace(Convert.ToString(r["QuestionText"])) ||
                        string.IsNullOrWhiteSpace(Convert.ToString(r["OptionA"])) ||
                        string.IsNullOrWhiteSpace(Convert.ToString(r["OptionB"])) ||
                        string.IsNullOrWhiteSpace(Convert.ToString(r["OptionC"])) ||
                        string.IsNullOrWhiteSpace(Convert.ToString(r["OptionD"])))
                        throw new Exception("Each question and all 4 options must be filled.");

                    var co = Convert.ToString(r["CorrectOption"]);
                    if (co != "A" && co != "B" && co != "C" && co != "D")
                        throw new Exception("CorrectOption must be A, B, C or D.");
                }

                _qa.Update(_qdt);
                MessageBox.Show("Questions saved.");
                LoadQuestions(); // refresh IDs etc.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save questions failed:\n" + ex.Message);
            }
        }

        private void btnAddQuestionsAwol_Click(object sender, EventArgs e)
        {
            var row = _qdt.NewRow();
            row["QuestionText"] = "New question...";
            row["OptionA"] = "A";
            row["OptionB"] = "B";
            row["OptionC"] = "C";
            row["OptionD"] = "D";
            row["CorrectOption"] = "A";     // A/B/C/D
            row["IsActive"] = true;
            row["CategoryID"] = DBNull.Value; // of een bestaande CategoryID
            _qdt.Rows.Add(row);
            dgvQuestionsAwol.CurrentCell = dgvQuestionsAwol.Rows[dgvQuestionsAwol.Rows.Count - 1].Cells["QuestionText"];
            dgvQuestionsAwol.BeginEdit(true);
        }

        private void btnDeleteQuestionAwol_Click(object sender, EventArgs e)
        {
            if (dgvQuestionsAwol.CurrentRow == null || dgvQuestionsAwol.CurrentRow.IsNewRow) return;
            if (MessageBox.Show("Delete selected question?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            dgvQuestionsAwol.Rows.RemoveAt(dgvQuestionsAwol.CurrentRow.Index);
        }

        private void btnDeleteUserAwol_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow) return;

            // pak de UserID uit de huidige rij
            object idObj = dataGridView1.CurrentRow.Cells["UserID"].Value;
            if (idObj == null || idObj == DBNull.Value)
            {
                MessageBox.Show("Select a valid user row first.");
                return;
            }
            int userId = Convert.ToInt32(idObj);

            if (MessageBox.Show($"Delete user {userId} and ALL their scores?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using (var con = new SqlConnection(_cs))
                {
                    con.Open();
                    using (var tx = con.BeginTransaction())
                    {
                        // 1) scores weg
                        using (var cmd = new SqlCommand("DELETE FROM dbo.Scores WHERE UserID=@id;", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", userId);
                            cmd.ExecuteNonQuery();
                        }
                        // 2) user weg
                        using (var cmd = new SqlCommand("DELETE FROM dbo.Users WHERE UserID=@id;", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", userId);
                            cmd.ExecuteNonQuery();
                        }
                        tx.Commit();
                    }
                }

                // UI updaten
                LoadUsers();
                MessageBox.Show("User (and scores) deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed:\n" + ex.Message);
            }
        }

        private void btnSaveUsersAwol_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.EndEdit();   // <-- was dgvUsersAwol.EndEdit();
                _ua.Update(_udt);
                MessageBox.Show("Users saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save users failed:\n" + ex.Message);
            }
        }
    }
}
