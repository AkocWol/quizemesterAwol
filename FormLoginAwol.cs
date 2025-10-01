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
    public partial class FormLoginAwol : Form
    {
        // Connection string naar jouw database
        string connectionString = @"Server=localhost\SQLEXPRESS;Database=quizemesterAwol;Trusted_Connection=True;";

        public FormLoginAwol()
        {
            InitializeComponent();
        }

        private void FormLoginAwol_Load(object sender, EventArgs e)
        {

        }

        // ===========================
        // Register knop
        // ===========================
        private void btnRegisterAwol_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsernameAwol.Text) || string.IsNullOrWhiteSpace(txtPasswordAwol.Text))
            {
                lblMessageAwol.Text = "Please fill in both fields.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@u, @p)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@u", txtUsernameAwol.Text);
                cmd.Parameters.AddWithValue("@p", txtPasswordAwol.Text); // ⚠ wachtwoord plain text (simpel houden)

                try
                {
                    cmd.ExecuteNonQuery();
                    lblMessageAwol.Text = "User registered successfully!";
                }
                catch
                {
                    lblMessageAwol.Text = "Error: Username might already exist.";
                }
            }
        }

        // ===========================
        // Login knop
        // ===========================
        private void btnLoginAwol_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsernameAwol.Text) || string.IsNullOrWhiteSpace(txtPasswordAwol.Text))
            {
                lblMessageAwol.Text = "Please enter username and password.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username=@u AND PasswordHash=@p";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@u", txtUsernameAwol.Text);
                cmd.Parameters.AddWithValue("@p", txtPasswordAwol.Text);

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    lblMessageAwol.Text = "Login success!";

                    var dashboard = new FormQuizAwol
                    {
                        CurrentUsername = txtUsernameAwol.Text  // <-- geef username mee
                    };

                    this.Hide();                                // verberg Form1
                    dashboard.FormClosed += (s, args) => this.Close(); // sluit app als Form2 sluit
                    dashboard.Show();
                }
                else
                {
                    lblMessageAwol.Text = "Invalid username or password.";
                }
            }
        }
    }
}
