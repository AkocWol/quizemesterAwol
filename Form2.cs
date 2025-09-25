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
    public partial class Form2 : Form
    {
        // --- Config ---
        private readonly string connectionString =
            @"Server=localhost\SQLEXPRESS;Database=quizemesterAwol;Trusted_Connection=True;";
        private readonly Random _rng = new Random();

        // --- State ---
        private List<QuizQuestion> _questions = new List<QuizQuestion>();
        private int _currentIndex = -1;
        private int _score = 0;
        private int _timeRemaining = 60;
        private bool _quizRunning = false;

        public Form2()
        {
            InitializeComponent();
        }

        // ===== Form events =====
        private void Form2_Load(object sender, EventArgs e)
        {
            ResetUi();
            SetAnswerButtonsEnabled(false);
        }

        private void ResetUi()
        {
            _score = 0;
            _timeRemaining = 60;
            _currentIndex = -1;
            _quizRunning = false;

            lblScoreValueAwol.Text = "0";
            lblTimeValueAwol.Text = _timeRemaining.ToString();
            lblQuestionsAwol.Text = "";
            btnStartAwol.Enabled = true;
            gameTimerAwol.Stop();
        }

        // ===== Start / Timer / Einde =====
        private void btnStartAwol_Click(object sender, EventArgs e)
        {
            try
            {
                _questions = LoadQuestionsFromDb();
                if (_questions.Count == 0)
                {
                    MessageBox.Show("No questions found in the database.", "Quiz",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _questions = _questions.OrderBy(_ => _rng.Next()).ToList();

                _score = 0;
                _timeRemaining = 60;
                _currentIndex = 0;
                _quizRunning = true;

                lblScoreValueAwol.Text = "0";
                lblTimeValueAwol.Text = _timeRemaining.ToString();
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

            if (_timeRemaining <= 0)
            {
                EndQuiz("Time's up!");
            }
        }

        private void EndQuiz(string reason)
        {
            _quizRunning = false;
            gameTimerAwol.Stop();
            SetAnswerButtonsEnabled(false);
            btnStartAwol.Enabled = true;

            MessageBox.Show($"{reason}\nYour score: {_score}/{_questions.Count}",
                "Quiz finished", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ResetUi();
        }

        // ===== Antwoorden =====
        private void ShowCurrentQuestion()
        {
            if (_currentIndex < 0 || _currentIndex >= _questions.Count)
            {
                EndQuiz("No more questions.");
                return;
            }

            var q = _questions[_currentIndex];
            lblQuestionsAwol.Text = q.QuestionText;

            btnAawol.Text = "A. " + q.OptionA;
            btnBawol.Text = "B. " + q.OptionB;
            btnCawol.Text = "C. " + q.OptionC;
            btnDawol.Text = "D. " + q.OptionD;
        }

        private void AnswerSelected(char choice)
        {
            if (!_quizRunning) return;

            var q = _questions[_currentIndex];
            if (char.ToUpperInvariant(choice) == q.CorrectOption)
            {
                _score++;
                lblScoreValueAwol.Text = _score.ToString();
            }

            _currentIndex++;
            if (_currentIndex >= _questions.Count)
                EndQuiz("You have completed all questions!");
            else
                ShowCurrentQuestion();
        }

        // Designer-events van de knoppen:
        private void btnAawol_Click(object sender, EventArgs e) => AnswerSelected('A');
        private void btnBawol_Click(object sender, EventArgs e) => AnswerSelected('B');
        private void btnCawol_Click(object sender, EventArgs e) => AnswerSelected('C');
        private void btnDawol_Click(object sender, EventArgs e) => AnswerSelected('D');

        private void SetAnswerButtonsEnabled(bool enabled)
        {
            btnAawol.Enabled = enabled;
            btnBawol.Enabled = enabled;
            btnCawol.Enabled = enabled;
            btnDawol.Enabled = enabled;
        }

        // ===== DB =====
        private List<QuizQuestion> LoadQuestionsFromDb()
        {
            var result = new List<QuizQuestion>();

            const string sql = @"
SELECT TOP 40 QuestionID, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption
FROM dbo.QuizQuestions
WHERE IsActive = 1
ORDER BY QuestionID;";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        result.Add(new QuizQuestion
                        {
                            QuestionID = r.GetInt32(0),
                            QuestionText = r.GetString(1),
                            OptionA = r.GetString(2),
                            OptionB = r.GetString(3),
                            OptionC = r.GetString(4),
                            OptionD = r.GetString(5),
                            CorrectOption = Convert.ToChar(r.GetString(6))
                        });
                    }
                }
            }
            return result;
        }

        // ===== Model =====
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

    }
}
