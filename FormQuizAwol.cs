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
    public partial class FormQuizAwol : Form
    {
        // ===== Config =====
        private readonly string connectionString =
            @"Server=localhost\SQLEXPRESS;Database=quizemesterAwol;Trusted_Connection=True;";
        private readonly Random _rng = new Random();

        // ===== State =====
        private List<QuizQuestion> _questions = new List<QuizQuestion>();
        private int _currentIndex = -1;
        private int _score = 0;
        private int _timeRemaining = 60;
        private bool _quizRunning = false;
        private bool _skipUsed = false;
        private bool _adminOverride = false;  // sessie-toggle
        private List<int> _selectedCategoryIds = new List<int>(); // leeg = General (alles)
        private const int QUESTION_TIME_LIMIT = 10;
        private int _qTimeRemaining = QUESTION_TIME_LIMIT;

        public string CurrentUsername { get; set; } = "Unknown";   // Zet dit vanuit Form1

        public FormQuizAwol()
        {
            InitializeComponent();
        }

        // ===== Lifecycle =====
        private void FormQuizAwol_Load(object sender, EventArgs e)
        {
            try
            {
                ResetUi();
                SetAnswerButtonsEnabled(false);
                LoadCategoriesIntoCheckedListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Startup error:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _adminOverride = false;
            if (chkAdminModeAwol != null) chkAdminModeAwol.Checked = false;
            UpdateRoleLabel();
        }


        private void ResetUi()
        {
            _score = 0;
            _timeRemaining = 60;
            _currentIndex = -1;
            _quizRunning = false;
            _skipUsed = false;

            lblScoreValueAwol.Text = "0";
            lblTimeValueAwol.Text = _timeRemaining.ToString();
            lblTimeValueAwol.ForeColor = Color.Black;
            lblQuestionsAwol.Text = "";

            btnStartAwol.Enabled = true;
            btnSkipAwol.Enabled = true;   // mag één keer; wordt disabled na gebruik
            gameTimerAwol.Stop();
        }

        // ===== Start / Timer / Einde =====
        private void btnStartAwol_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedCategoryIds = GetSelectedCategoryIds();
                _questions = LoadQuestionsFromDb(_selectedCategoryIds);

                if (_questions.Count == 0)
                {
                    MessageBox.Show("No questions found for the selected categories.", "Quiz",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // random volgorde vragen
                _questions = _questions.OrderBy(_ => _rng.Next()).ToList();

                _score = 0;
                _timeRemaining = 60;
                _currentIndex = 0;
                _quizRunning = true;

                lblScoreValueAwol.Text = "0";
                lblTimeValueAwol.Text = _timeRemaining.ToString();
                lblTimeValueAwol.ForeColor = Color.Black;

                btnStartAwol.Enabled = false;
                SetAnswerButtonsEnabled(true);

                ShowCurrentQuestion();
                gameTimerAwol.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting quiz:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gameTimerAwol_Tick(object sender, EventArgs e)
        {
            if (!_quizRunning) return;

            _timeRemaining--;
            lblTimeValueAwol.Text = _timeRemaining.ToString();

            // Alert near expiry
            if (_timeRemaining <= 10) lblTimeValueAwol.ForeColor = Color.OrangeRed;
            if (_timeRemaining == 10 || _timeRemaining <= 5)
                System.Media.SystemSounds.Beep.Play();

            if (_timeRemaining <= 0)
            {
                EndQuiz("Time's up!");
            }

            _qTimeRemaining--;
            lblQtimeValueAwol.Text = _qTimeRemaining.ToString();
            if (_qTimeRemaining <= 3) lblQtimeValueAwol.ForeColor = Color.OrangeRed;
            if (_qTimeRemaining <= 0)
            {
                NextQuestionOrFinish();
                // ShowCurrentQuestion() reset de vraag-timer weer
            }

        }

        private void EndQuiz(string reason)
        {
            _quizRunning = false;
            gameTimerAwol.Stop();
            SetAnswerButtonsEnabled(false);
            btnStartAwol.Enabled = true;

            // Score opslaan + rank bepalen
            int? categoryIdToSave = _selectedCategoryIds.Count == 1 ? _selectedCategoryIds[0] : (int?)null;
            var (rank, top10) = SaveScoreAndGetRank(CurrentUsername, _score, _questions.Count, categoryIdToSave);

            string extra = top10
                ? $"\nYou made the Top 10! Current rank: {rank}"
                : "";
            MessageBox.Show($"{reason}\nYour score: {_score}/{_questions.Count}{extra}",
                "Quiz finished", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ResetUi();
        }

        // ===== Vragen tonen & antwoorden =====
        private void ShowCurrentQuestion()
        {
            if (_currentIndex < 0 || _currentIndex >= _questions.Count)
            {
                EndQuiz("No more questions.");
                return;
            }

            var q = _questions[_currentIndex];

            lblQuestionsAwol.Text = q.QuestionText;

            // Antwoorden per vraag randomiseren, maar weten welke correct is
            var options = new List<(string Text, bool IsCorrect)>
            {
                (q.OptionA, q.CorrectOption == 'A'),
                (q.OptionB, q.CorrectOption == 'B'),
                (q.OptionC, q.CorrectOption == 'C'),
                (q.OptionD, q.CorrectOption == 'D')
            }.OrderBy(_ => _rng.Next()).ToList();

            var buttons = new[] { btnAawol, btnBawol, btnCawol, btnDawol };
            for (int i = 0; i < 4; i++)
            {
                buttons[i].Text = $"{(char)('A' + i)}. " + options[i].Text;
                buttons[i].Tag = options[i].IsCorrect; // bool
            }

            _qTimeRemaining = QUESTION_TIME_LIMIT;
            lblQtimeValueAwol.Text = _qTimeRemaining.ToString();
            lblQtimeValueAwol.ForeColor = Color.Black;
        }

        private void AnswerSelected(Button btn)
        {
            if (!_quizRunning) return;

            bool isCorrect = (btn.Tag is bool b) && b;
            if (isCorrect)
            {
                _score++;
                lblScoreValueAwol.Text = _score.ToString();
                btn.BackColor = Color.LightGreen;
            }
            else
            {
                btn.BackColor = Color.LightCoral;
            }

            // korte visuele feedback
            var t = new Timer { Interval = 300 };
            t.Tick += (s, e) =>
            {
                t.Stop();
                btn.BackColor = SystemColors.Control;
                NextQuestionOrFinish();
            };
            t.Start();
        }

        private void NextQuestionOrFinish()
        {
            _currentIndex++;
            if (_currentIndex >= _questions.Count)
                EndQuiz("You have completed all questions!");
            else
                ShowCurrentQuestion();
        }

        private void SetAnswerButtonsEnabled(bool enabled)
        {
            btnAawol.Enabled = enabled;
            btnBawol.Enabled = enabled;
            btnCawol.Enabled = enabled;
            btnDawol.Enabled = enabled;
        }

        // Designer-event handlers van je knoppen (GEEN HookUpEvents):
        private void btnAawol_Click(object sender, EventArgs e) => AnswerSelected(btnAawol);
        private void btnBawol_Click(object sender, EventArgs e) => AnswerSelected(btnBawol);
        private void btnCawol_Click(object sender, EventArgs e) => AnswerSelected(btnCawol);
        private void btnDawol_Click(object sender, EventArgs e) => AnswerSelected(btnDawol);

        private void btnSkipAwol_Click(object sender, EventArgs e)
        {
            if (!_quizRunning) return;
            if (_skipUsed)
            {
                MessageBox.Show("Skip already used.");
                return;
            }
            _skipUsed = true;
            btnSkipAwol.Enabled = false;
            NextQuestionOrFinish();
        }

        private void btnScoresAwol_Click(object sender, EventArgs e)
        {
            using (var f = new FormScoresAwol(connectionString))
                f.ShowDialog(this);
        }

        // ===== DB =====
        private List<QuizQuestion> LoadQuestionsFromDb(List<int> categoryIds)
        {
            var result = new List<QuizQuestion>();

            // Als er geen categorie gekozen is → General (alle vragen)
            string sqlAll = @"
SELECT TOP 40 QuestionID, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption
FROM dbo.QuizQuestions
WHERE IsActive = 1
ORDER BY QuestionID;";

            // Anders filter op gekozen categorieën
            if (categoryIds == null || categoryIds.Count == 0)
            {
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sqlAll, con))
                {
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) result.Add(ReadQuestion(r));
                }
                return result;
            }
            else
            {
                // Dynamische IN (...) met parameters
                var paramNames = categoryIds.Select((id, idx) => "@p" + idx).ToList();
                string sql = $@"
SELECT TOP 40 QuestionID, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption
FROM dbo.QuizQuestions
WHERE IsActive = 1 AND CategoryID IN ({string.Join(",", paramNames)})
ORDER BY QuestionID;";

                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, con))
                {
                    for (int i = 0; i < categoryIds.Count; i++)
                        cmd.Parameters.AddWithValue(paramNames[i], categoryIds[i]);

                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) result.Add(ReadQuestion(r));
                }
                return result;
            }
        }

        private QuizQuestion ReadQuestion(SqlDataReader r) => new QuizQuestion
        {
            QuestionID = r.GetInt32(0),
            QuestionText = r.GetString(1),
            OptionA = r.GetString(2),
            OptionB = r.GetString(3),
            OptionC = r.GetString(4),
            OptionD = r.GetString(5),
            CorrectOption = Convert.ToChar(r.GetString(6))
        };

        private List<int> GetSelectedCategoryIds()
        {
            var list = new List<int>();
            foreach (var item in clbCategoriesAwol.CheckedItems)
            {
                if (item is CategoryItem c) list.Add(c.CategoryID);
            }
            return list;
        }

        private void LoadCategoriesIntoCheckedListBox()
        {
            clbCategoriesAwol.Items.Clear();
            const string sql = "SELECT CategoryID, Name FROM dbo.Categories ORDER BY Name;";
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        clbCategoriesAwol.Items.Add(new CategoryItem
                        {
                            CategoryID = r.GetInt32(0),
                            Name = r.GetString(1)
                        });
                    }
                }
            }
        }

        private (int rank, bool top10) SaveScoreAndGetRank(string username, int score, int totalQuestions, int? categoryId)
        {
            // Pak UserID
            int userId = GetUserIdByUsername(username);
            if (userId == 0) return (int.MaxValue, false);

            // Insert score
            int scoreId;
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
INSERT INTO dbo.Scores(UserID, Score, TotalQuestions, CategoryID)
OUTPUT INSERTED.ScoreID
VALUES(@uid, @s, @tq, @cid);", con))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@s", score);
                cmd.Parameters.AddWithValue("@tq", totalQuestions);
                object cid = categoryId.HasValue ? (object)categoryId.Value : DBNull.Value;
                cmd.Parameters.AddWithValue("@cid", cid);
                con.Open();
                scoreId = (int)cmd.ExecuteScalar();
            }

            // Rank = 1 + aantal scores die hoger zijn
            int rank;
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT 1 + COUNT(*) FROM dbo.Scores WHERE Score > @s;", con))
            {
                cmd.Parameters.AddWithValue("@s", score);
                con.Open();
                rank = (int)cmd.ExecuteScalar();
            }

            return (rank, rank <= 10);
        }

        private int GetUserIdByUsername(string username)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT UserID FROM dbo.Users WHERE Username=@u;", con))
            {
                cmd.Parameters.AddWithValue("@u", username);
                con.Open();
                var obj = cmd.ExecuteScalar();
                return obj == null ? 0 : Convert.ToInt32(obj);
            }
        }

        private List<(string Username, int Score)> GetTop10Scores()
        {
            var list = new List<(string, int)>();
            const string sql = @"
SELECT TOP 10 u.Username, s.Score
FROM dbo.Scores s
JOIN dbo.Users u ON u.UserID = s.UserID
ORDER BY s.Score DESC, s.CreatedAt ASC;";
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                        list.Add((r.GetString(0), r.GetInt32(1)));
                }
            }
            return list;
        }

        // ===== Models =====
        private class QuizQuestion
        {
            public int QuestionID { get; set; }
            public string QuestionText { get; set; } = "";
            public string OptionA { get; set; } = "";
            public string OptionB { get; set; } = "";
            public string OptionC { get; set; } = "";
            public string OptionD { get; set; } = "";
            public char CorrectOption { get; set; } // 'A','B','C','D'
        }
        private class CategoryItem
        {
            public int CategoryID { get; set; }
            public string Name { get; set; } = "";
            public override string ToString() => Name;
        }

        private void btnAdminAwol_Click(object sender, EventArgs e)
        {
            if (!IsAdminSession())   // <-- gebruik override + DB
            {
                MessageBox.Show("Admins only.");
                return;
            }
            using (var f = new FormAdminAwol(connectionString))
                f.ShowDialog(this);
        }

        private bool IsCurrentUserAdmin()
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT IsAdmin FROM dbo.Users WHERE Username=@u;", con))
            {
                cmd.Parameters.AddWithValue("@u", CurrentUsername);
                con.Open();
                var o = cmd.ExecuteScalar();
                return o != null && Convert.ToBoolean(o);
            }
        }

        private bool IsAdminSession()
        {
            return _adminOverride || IsCurrentUserAdmin(); // override wint van DB
        }

        // dit overwrite de sessie zodat gebruiker admin kan worden
        private void UpdateRoleLabel()
        {
            if (lblRoleAwol != null)
                lblRoleAwol.Text = IsAdminSession()
                    ? (_adminOverride ? "Role: Admin" : "Role: Admin")
                    : "Role: Player";
        }

        private void chkAdminModeAwol_CheckedChanged(object sender, EventArgs e)
        {
            _adminOverride = chkAdminModeAwol.Checked;
            UpdateRoleLabel();
        }

    }
}
