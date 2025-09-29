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
            new SqlCommandBuilder(_qa); // auto-generate insert/update/delete

            _qdt.Clear();
            _qa.Fill(_qdt);
            dgvQuestionsAwol.DataSource = _qdt;
            dgvQuestionsAwol.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadUsers()
        {
            // LoadUsers()
            const string sql = @"SELECT UserID, Username, IsAdmin FROM dbo.Users ORDER BY Username";
            _ua = new SqlDataAdapter(sql, _cs);
            new SqlCommandBuilder(_ua);

            _udt.Clear();
            _ua.Fill(_udt);
            dataGridView1.DataSource = _udt; // <-- was dgvUsersAwol
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // <-- idem

        }

        private void btnSaveQuestionsAwol_Click(object sender, EventArgs e)
        {
            try
            {
                dgvQuestionsAwol.EndEdit();
                _qa.Update(_qdt);
                MessageBox.Show("Questions saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save questions failed:\n" + ex.Message);
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
