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
using System.Media;

namespace quizemesterAwol
{
    public partial class FormQuizAwol : Form
    {
        // ===== Config =====
        // Verbindt met je lokale SQL Server database
        private readonly string connectionString =
            @"Server=localhost\SQLEXPRESS;Database=quizemesterAwol;Trusted_Connection=True;";
        // Random generator voor willekeurige volgordes en keuzes
        private readonly Random _rng = new Random();

        // ===== State =====
        // hier benden staan alle game data wat er in de game gebeurd
        private List<QuizQuestion> _questions = new List<QuizQuestion>();
        private int _currentIndex = -1;
        private int _score = 0;
        private int _timeRemaining = 60;
        private int? _specialIndex = null;     // index in _questions
        private int _qTimeRemaining = QUESTION_TIME_LIMIT;
        private int _specialQuizCorrect = 0;
        private int _penaltySeconds = 0;

        private bool _quizRunning = false;
        private bool _skipUsed = false;
        private bool _adminOverride = false;  // sessie-toggle
        private bool _joker5050Used = false;
        private bool _isSpecialActive = false;
        private bool _isSpecialQuizMode = false;

        private List<int> _selectedCategoryIds = new List<int>(); // leeg = General (alles)

        private const int QUESTION_TIME_LIMIT = 10;
        private const int SPECIAL_BONUS = 3;   // extra punten bij goed

        private readonly Color _defaultPlayBg = SystemColors.Control; // originele bg van groupBox3
        private readonly System.Diagnostics.Stopwatch _totalStopwatch = new System.Diagnostics.Stopwatch();

        // Wordt door het login-form gezet (wie speelt er)
        public string CurrentUsername { get; set; } = "Unknown";   // Zet dit vanuit Form1

        public FormQuizAwol()
        {
            InitializeComponent();
        }

        // ===== Lifecycle =====
        // Form load: initialiseer UI, laad categorieën, zet rollenlabel
        private void FormQuizAwol_Load(object sender, EventArgs e)
        {
            try
            {
                ResetUi(); // zet beginstatus van knoppen/labels/timers
                SetAnswerButtonsEnabled(false); // antwoorden pas aanzetten nadat je starts
                LoadCategoriesIntoCheckedListBox(); // vul categorie-keuze in de UI
            }
            catch (Exception ex)
            {
                MessageBox.Show("Startup error:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _adminOverride = false; // admin-toggle standaard uit
            if (chkAdminModeAwol != null) chkAdminModeAwol.Checked = false;
            UpdateRoleLabel(); // UI updaten voor rolweergave
        }

        // Herstelt UI naar ‘niet aan het spelen’
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

            btnStartAwol.Enabled = true; // je mag weer starten
            btnSkipAwol.Enabled = true;   // skip mag 1x; na gebruik wordt ’ie disabled
            btn5050Awol.Enabled = false;
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

                // --- reset joker voor dit NIEUWE potje ---
                _joker5050Used = false;
                btn5050Awol.Enabled = true;

                // random volgorde vragen
                _questions = _questions.OrderBy(_ => _rng.Next()).ToList();

                // ---- SPECIAL QUESTION kiezen ----
                // Special mag alleen gekozen worden uit de eerste 20 (of minder) vragen
                int span = Math.Min(20, _questions.Count);
                _specialIndex = (span > 0) ? _rng.Next(0, span) : (int?)null;

                // Reset basisstatus voor nieuwe run
                _score = 0;
                _timeRemaining = 60;
                _currentIndex = 0;
                _quizRunning = true;
                _isSpecialQuizMode = false; // start normale quiz

                // UI labels updaten
                lblScoreValueAwol.Text = "0";
                lblTimeValueAwol.Text = _timeRemaining.ToString();
                lblTimeValueAwol.ForeColor = Color.Black;

                // Startknop uit, antwoorden aan
                btnStartAwol.Enabled = false;
                SetAnswerButtonsEnabled(true);

                // Toon de eerste vraag en start de timer
                ShowCurrentQuestion();
                gameTimerAwol.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting quiz:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tick-handler van de timer: regelt aftellen en weergave
        private void gameTimerAwol_Tick(object sender, EventArgs e)
        {
            // In special-modus loopt de timer sneller (250ms), en tonen we totale tijd + penalty
            if (_isSpecialQuizMode)
            {
                if (gameTimerAwol.Interval != 250) gameTimerAwol.Interval = 250; // snellere weergave
                if (!_quizRunning) return;

                // Totale verstreken tijd + strafseconden tonen
                var elapsed = _totalStopwatch.Elapsed + TimeSpan.FromSeconds(_penaltySeconds);
                lblTotalTimeAwol.Text = $"{(int)elapsed.TotalMinutes:00}:{elapsed.Seconds:00}";
                return; // geen normale timers in special
            }
            else
            {
                // In normale modus tikt de timer elke seconde
                if (gameTimerAwol.Interval != 1000) gameTimerAwol.Interval = 1000; // langzamer: 1s per tick
            }

            if (!_quizRunning) return;

            // Totaal-timer (normale quiz)
            _timeRemaining--;
            lblTimeValueAwol.Text = _timeRemaining.ToString();

            // Visuele/audio waarschuwingen bij weinig tijd
            if (_timeRemaining <= 10) lblTimeValueAwol.ForeColor = Color.OrangeRed;
            if (_timeRemaining == 10 || _timeRemaining <= 5)
                System.Media.SystemSounds.Beep.Play();

            // Als de totale tijd op is → einde quiz
            if (_timeRemaining <= 0)
            {
                EndQuiz("Time's up!");
                return;
            }

            // Vraag-timer (normale quiz)
            _qTimeRemaining--;
            lblQtimeValueAwol.Text = _qTimeRemaining.ToString();
            if (_qTimeRemaining <= 3) lblQtimeValueAwol.ForeColor = Color.OrangeRed;

            // Als de vraag-tijd op is → automatisch naar volgende vraag of afronden
            if (_qTimeRemaining <= 0)
            {
                NextQuestionOrFinish(); // ShowCurrentQuestion() zal de vraag-timer weer resetten
            }

        }

        // Maakt de quiz netjes af: stoppen, score opslaan, rank tonen, en UI resetten
        private void EndQuiz(string reason)
        {
            _quizRunning = false;
            gameTimerAwol.Stop();
            SetAnswerButtonsEnabled(false);
            btnStartAwol.Enabled = true;

            // Bepaal of we een enkele categorie hadden (handig voor rankings per categorie)
            int? categoryIdToSave = _selectedCategoryIds.Count == 1 ? _selectedCategoryIds[0] : (int?)null;

            // Score opslaan in DB + huidige positie in ranking terugkrijgen (+ of je Top10 haalt)
            var (rank, top10) = SaveScoreAndGetRank(CurrentUsername, _score, _questions.Count, categoryIdToSave);

            // Extra melding als Top 10 gehaald is
            string extra = top10
                ? $"\nYou made the Top 10! Current rank: {rank}"
                : "";

            // Einde-melding met reden + scoreoverzicht
            MessageBox.Show($"{reason}\nYour score: {_score}/{_questions.Count}{extra}",
                "Quiz finished", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // UI terug naar beginstaat zodat je weer een nieuw potje kunt starten
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

            // Nieuwe vraag = knoppen weer actief + neutrale kleur
            SetAnswerButtonsEnabled(true);
            btnAawol.BackColor = SystemColors.Control;
            btnBawol.BackColor = SystemColors.Control;
            btnCawol.BackColor = SystemColors.Control;
            btnDawol.BackColor = SystemColors.Control;

            var q = _questions[_currentIndex];
            lblQuestionsAwol.Text = q.QuestionText;

            // === Special Question: kleur en geluid ===
            if (_specialIndex.HasValue && _currentIndex == _specialIndex.Value)
            {
                _isSpecialActive = true;
                groupBox3.BackColor = Color.Gold;
                lblQuestionsAwol.Font = new Font(lblQuestionsAwol.Font, FontStyle.Bold);
                TryPlaySound("Sounds/special.wav");
            }
            else
            {
                _isSpecialActive = false;
                groupBox3.BackColor = _defaultPlayBg;
                lblQuestionsAwol.Font = new Font(lblQuestionsAwol.Font, FontStyle.Regular);
            }

            // Antwoorden random + juiste taggen
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

            if (_isSpecialQuizMode)
            {
                // geen per-vraag timer in special quiz
                lblQtimeValueAwol.Text = "-";
                lblQtimeValueAwol.ForeColor = Color.Black;
            }
            else
            {
                _qTimeRemaining = QUESTION_TIME_LIMIT;
                lblQtimeValueAwol.Text = _qTimeRemaining.ToString();
                lblQtimeValueAwol.ForeColor = Color.Black;
            }
        }

        private void AnswerSelected(Button btn)
        {
            if (!_quizRunning) return;

            // verdere input blokkeren
            SetAnswerButtonsEnabled(false);

            bool isCorrect = (btn.Tag is bool b) && b;

            // --- SPECIAL QUIZ MODUS ---
            if (_isSpecialQuizMode)
            {
                if (isCorrect)
                {
                    _score++;
                    _specialQuizCorrect++;
                    lblScoreValueAwol.Text = _score.ToString();
                    btn.BackColor = Color.LightGreen;
                }
                else
                {
                    _penaltySeconds += 5; // +5s straf
                    btn.BackColor = Color.LightCoral;
                }

                // korte visuele feedback en dan door
                var tSQ = new Timer { Interval = 500 };
                tSQ.Tick += (s, e) =>
                {
                    tSQ.Stop();
                    btn.BackColor = SystemColors.Control;

                    // klaar bij 10 correcte antwoorden
                    if (_specialQuizCorrect >= 10)
                    {
                        _quizRunning = false;
                        _totalStopwatch.Stop();
                        gameTimerAwol.Stop();

                        var total = _totalStopwatch.Elapsed + TimeSpan.FromSeconds(_penaltySeconds);
                        MessageBox.Show($"Special Quiz finished!\nTime: {(int)total.TotalMinutes:00}:{total.Seconds:00}");

                        ResetUi();
                        _isSpecialQuizMode = false;
                    }
                    else
                    {
                        NextQuestionOrFinish();
                    }
                };
                tSQ.Start();
                return; // normale flow overslaan in special-modus
            }

            // --- NORMALE QUIZ ---
            if (isCorrect)
            {
                _score++;
                lblScoreValueAwol.Text = _score.ToString();
                btn.BackColor = Color.LightGreen;

                // special question bonus
                if (_isSpecialActive)
                {
                    _score += SPECIAL_BONUS;
                    lblScoreValueAwol.Text = _score.ToString();
                    _isSpecialActive = false;
                }

                TryPlaySound("Sounds/correct.wav");
            }
            else
            {
                btn.BackColor = Color.LightCoral;
                TryPlaySound("Sounds/wrong.wav");
            }

            // korte visuele feedback en dan door
            var t = new Timer { Interval = 500 };
            t.Tick += (s, e) =>
            {
                t.Stop();
                btn.BackColor = SystemColors.Control;
                NextQuestionOrFinish(); // ShowCurrentQuestion() zet knoppen weer goed
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
                // speel win geluid
                TryPlaySound("Sounds/win.wav");

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

        private void btn5050Awol_Click(object sender, EventArgs e)
        {
            if (!_quizRunning) return; // Alleen werken als de quiz actief is
            if (_joker5050Used) return; // Joker al gebruikt? Dan niets doen

            // Pak referenties naar de vier antwoordknoppen
            var buttons = new[] { btnAawol, btnBawol, btnCawol, btnDawol };
            // Vind de correcte knop: Tag moet een bool zijn die true is
            var correctBtn = buttons.First(b => (b.Tag is bool ok) && ok);
            // Verzamel alle 'verkeerde' knoppen: Tag ontbreekt of bool == false
            var wrongBtns = buttons.Where(b => !(b.Tag is bool ok) || !ok).ToList();

            // Laat één willekeurige verkeerde knop over (de andere twee gaan uit)
            var keepWrong = wrongBtns[_rng.Next(wrongBtns.Count)];

            // Schakel alle knoppen behalve de correcte + de bewaarde verkeerde uit
            foreach (var b in buttons)
                b.Enabled = (b == correctBtn) || (b == keepWrong);

            // Markeer dat de 50/50-joker gebruikt is en disable de joker-knop zelf
            _joker5050Used = true;
            btn5050Awol.Enabled = false;
        }

        private void btnSpecialQuizAwol_Click(object sender, EventArgs e)
        {
            try
            {
                // Zet de app in 'special quiz mode' (snelle modus)
                _isSpecialQuizMode = true;
                _specialQuizCorrect = 0; // reset teller goede antwoorden in special
                _penaltySeconds = 0; // reset eventuele strafseconden

                // Laad vragen voor de gekozen categorieën en schud de volgorde
                _selectedCategoryIds = GetSelectedCategoryIds();
                _questions = LoadQuestionsFromDb(_selectedCategoryIds)
                    .OrderBy(_ => _rng.Next()).ToList();

                // Geen vragen gevonden? Meld het en stop
                if (_questions.Count == 0)
                {
                    MessageBox.Show("No questions found.", "Quiz", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Reset basisstate voor deze special-run (normale timers staan 'uit' in deze modus)
                _score = 0;
                _currentIndex = 0;
                _quizRunning = true;
                lblScoreValueAwol.Text = "0";

                // Laat meteen de eerste vraag zien en zet de antwoorden aan
                SetAnswerButtonsEnabled(true);
                ShowCurrentQuestion();

                // Start/Reset de stopwatch voor totale tijd (i.p.v. de normale quiz-timer)
                _totalStopwatch.Reset();
                _totalStopwatch.Start();

                // Gebruik de bestaande WinForms-timer alleen als 'UI refresher' (snellere ticks)
                gameTimerAwol.Interval = 250; // 4x per seconde updaten
                gameTimerAwol.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting special quiz:\n" + ex.Message);
            }
        }

        // ===== DB =====
        private List<QuizQuestion> LoadQuestionsFromDb(List<int> categoryIds)
        {
            var result = new List<QuizQuestion>();

            // Als er geen categorie gekozen is → pak "General": alle actieve vragen (max 40)
            string sqlAll = @"
SELECT TOP 40 QuestionID, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption
FROM dbo.QuizQuestions
WHERE IsActive = 1
ORDER BY QuestionID;";

            // Geen categorieën geselecteerd? → voer de algemene query uit.
            if (categoryIds == null || categoryIds.Count == 0)
            {
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sqlAll, con))
                {
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) result.Add(ReadQuestion(r)); // elke rij → QuizQuestion object
                }
                return result;
            }
            else
            {
                // Wel categorieën: bouw een dynamische IN (...) met parameters (@p0, @p1, ...)
                var paramNames = categoryIds.Select((id, idx) => "@p" + idx).ToList();
                string sql = $@"
SELECT TOP 40 QuestionID, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption
FROM dbo.QuizQuestions
WHERE IsActive = 1 AND CategoryID IN ({string.Join(",", paramNames)})
ORDER BY QuestionID;";

                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, con))
                {
                    // Koppel elke gekozen categorie aan de juiste parameter
                    for (int i = 0; i < categoryIds.Count; i++)
                        cmd.Parameters.AddWithValue(paramNames[i], categoryIds[i]);

                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) result.Add(ReadQuestion(r)); // map elke datarij naar object
                }
                return result;
            }
        }

        // Helper: vertaalt één database-rij naar een QuizQuestion object
        private QuizQuestion ReadQuestion(SqlDataReader r) => new QuizQuestion
        {
            // Kolomindexen komen overeen met de SELECT volgorde
            QuestionID = r.GetInt32(0),
            QuestionText = r.GetString(1),
            OptionA = r.GetString(2),
            OptionB = r.GetString(3),
            OptionC = r.GetString(4),
            OptionD = r.GetString(5),
            // CorrectOption staat als string in DB; eerste/alleen karakter omzetten naar char
            CorrectOption = Convert.ToChar(r.GetString(6))
        };

        // Leest de aangevinkte categorieën uit de CheckedListBox en geeft hun IDs terug
        private List<int> GetSelectedCategoryIds()
        {
            var list = new List<int>();
            foreach (var item in clbCategoriesAwol.CheckedItems)
            {
                if (item is CategoryItem c) list.Add(c.CategoryID); // pak de ID uit jouw wrapper type
            }
            return list;
        }

        private void LoadCategoriesIntoCheckedListBox()
        {
            clbCategoriesAwol.Items.Clear(); // eerst leegmaken
            const string sql = "SELECT CategoryID, Name FROM dbo.Categories ORDER BY Name;";
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                con.Open(); // DB-verbinding openen
                using (var r = cmd.ExecuteReader())
                {
                    // Elke rij uit Categories omzetten naar een CategoryItem en in de lijst zetten
                    while (r.Read())
                    {
                        clbCategoriesAwol.Items.Add(new CategoryItem
                        {
                            CategoryID = r.GetInt32(0), // eerste kolom
                            Name = r.GetString(1) // tweede kolom
                        });
                    }
                }
            }
        }

        private (int rank, bool top10) SaveScoreAndGetRank(string username, int score, int totalQuestions, int? categoryId)
        {
            // Bepaal de UserID op basis van username
            int userId = GetUserIdByUsername(username);
            if (userId == 0) return (int.MaxValue, false);

            // --- Score opslaan ---
            int scoreId;
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
INSERT INTO dbo.Scores(UserID, Score, TotalQuestions, CategoryID)
OUTPUT INSERTED.ScoreID
VALUES(@uid, @s, @tq, @cid);", con))
            {
                // Parameters binden (voorkomt SQL-injectie)
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@s", score);
                cmd.Parameters.AddWithValue("@tq", totalQuestions);
                object cid = categoryId.HasValue ? (object)categoryId.Value : DBNull.Value; // NULL toestaan
                cmd.Parameters.AddWithValue("@cid", cid);

                con.Open();
                scoreId = (int)cmd.ExecuteScalar(); // haalt de nieuwe ScoreID op (via OUTPUT)
            }

            // --- Rank bepalen ---
            // Rank = 1 + aantal scores die HOGER zijn dan de huidige score
            int rank;
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT 1 + COUNT(*) FROM dbo.Scores WHERE Score > @s;", con))
            {
                cmd.Parameters.AddWithValue("@s", score);
                con.Open();
                rank = (int)cmd.ExecuteScalar();
            }

            // top10 = true als rank ≤ 10
            return (rank, rank <= 10);
        }

        private int GetUserIdByUsername(string username)
        {
            // Maakt DB-verbinding en zoekt de UserID behorend bij de opgegeven username
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT UserID FROM dbo.Users WHERE Username=@u;", con))
            {
                cmd.Parameters.AddWithValue("@u", username); // geparameteriseerd → voorkomt SQL-injectie
                con.Open(); 
                var obj = cmd.ExecuteScalar(); // pakt 1e kolom van 1e rij (of null als niets)
                return obj == null ? 0 : Convert.ToInt32(obj); // 0 teruggeven wanneer user niet bestaat
            }
        }

        private List<(string Username, int Score)> GetTop10Scores()
        {
            // Haalt de Top 10 scores op met bijbehorende gebruikersnaam
            var list = new List<(string, int)>();
            const string sql = @"
SELECT TOP 10 u.Username, s.Score
FROM dbo.Scores s
JOIN dbo.Users u ON u.UserID = s.UserID
ORDER BY s.Score DESC, s.CreatedAt ASC;"; // bij gelijke score: oudste eerst
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    // Voeg elke rij toe als tuple (Username, Score)
                    while (r.Read())
                        list.Add((r.GetString(0), r.GetInt32(1)));
                }
            }
            return list;
        }

        // ===== Models =====
        private class QuizQuestion
        {
            // Eén vraag met 4 opties en een juiste letter ('A'..'D')
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
            // Item voor in de CheckedListBox (toon Name, bewaar CategoryID)
            public int CategoryID { get; set; }
            public string Name { get; set; } = "";
            public override string ToString() => Name; // zorgt dat de lijst de naam toont
        }

        private void btnAdminAwol_Click(object sender, EventArgs e)
        {
            // Controleer of de huidige sessie adminrechten heeft
            // (kijkt zowel naar de DB als naar de override)
            if (!IsAdminSession())   // <-- gebruik override + DB
            {
                MessageBox.Show("Admins only."); // niet-admins mogen dit niet openen
                return;
            }

            // Open het Admin-scherm als dialoog (modal venster)
            using (var f = new FormAdminAwol(connectionString))
                f.ShowDialog(this);
        }

        private bool IsCurrentUserAdmin()
        {
            // Vraagt in de database op of de huidige gebruiker adminrechten heeft
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT IsAdmin FROM dbo.Users WHERE Username=@u;", con))
            {
                cmd.Parameters.AddWithValue("@u", CurrentUsername);
                con.Open();
                var o = cmd.ExecuteScalar(); // haalt de IsAdmin-waarde (True/False) op
                return o != null && Convert.ToBoolean(o); // geef true terug als IsAdmin = 1 / true
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
                    ? (_adminOverride ? "Role: Admin" : "Role: Admin") // in beide gevallen “Admin”
                    : "Role: Player";
        }

        // Event-handler voor de checkbox waarmee je tijdelijk adminrechten kunt aanzetten
        private void chkAdminModeAwol_CheckedChanged(object sender, EventArgs e)
        {
            _adminOverride = chkAdminModeAwol.Checked; // zet override aan/uit
            UpdateRoleLabel(); // pas de roltekst direct aan
        } 

        private void TryPlaySound(string path)
        {
            try
            {
                // Controleer of het opgegeven geluidsbestand bestaat 
                // op een of ander manier werkt de geluidsbestand niet terwijl het het wel eerst wel deet
                if (System.IO.File.Exists(path))
                {
                    // Maak een tijdelijke SoundPlayer aan en speel het geluid af
                    using (SoundPlayer player = new SoundPlayer(path))
                        player.Play(); // speelt asynchroon af (programma loopt gewoon verder)
                }
            }
            catch
            {
                // geen foutmelding tonen als geluid ontbreekt
            }
        }
    }
}
