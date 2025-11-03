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
            // Controleer of de velden zijn ingevuld velden mogen niet leeg zijn !!
            if (string.IsNullOrWhiteSpace(txtUsernameAwol.Text) || string.IsNullOrWhiteSpace(txtPasswordAwol.Text))
            {
                lblMessageAwol.Text = "Please fill in both fields.";
                return;
            }

            // Maak een verbinding met de database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // SQL query om een nieuwe gebruiker toe te voegen
                string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@u, @p)";
                SqlCommand cmd = new SqlCommand(query, con);

                // Voeg de waarden uit de tekstvakken toe aan de query
                cmd.Parameters.AddWithValue("@u", txtUsernameAwol.Text);
                cmd.Parameters.AddWithValue("@p", txtPasswordAwol.Text); // ⚠ wachtwoord plain text (simpel houden)

                try
                {
                    // Voer de query uit
                    cmd.ExecuteNonQuery();
                    lblMessageAwol.Text = "User registered successfully!";
                }
                catch
                {
                    // Als het misgaat (bijv. username al bestaat)
                    lblMessageAwol.Text = "Error: Username might already exist.";
                }
            }
        }

        // ===========================
        // Login knop
        // ===========================
        private void btnLoginAwol_Click(object sender, EventArgs e)
        {
            // Controleer of beide velden ingevuld zijn
            if (string.IsNullOrWhiteSpace(txtUsernameAwol.Text) || string.IsNullOrWhiteSpace(txtPasswordAwol.Text))
            {
                lblMessageAwol.Text = "Please enter username and password.";
                return; // stop hier als iets leeg is
            }

            // Maak een verbinding met de database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open(); // open de verbinding

                // Controleer of er een gebruiker bestaat met deze gegevens
                string query = "SELECT COUNT(*) FROM Users WHERE Username=@u AND PasswordHash=@p";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@u", txtUsernameAwol.Text);
                cmd.Parameters.AddWithValue("@p", txtPasswordAwol.Text);

                // Voer de query uit en krijg het aantal overeenkomende gebruikers
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    // Als er minstens 1 gebruiker gevonden is → succes
                    lblMessageAwol.Text = "Login success!";

                    // Maak het volgende formulier (het quiz dashboard)
                    var dashboard = new FormQuizAwol
                    {
                        CurrentUsername = txtUsernameAwol.Text  // <-- geef username mee
                    };

                    this.Hide();                                // verberg het loginformulier
                    // als het dashboard sluit, sluit ook dit formulier (hele app)
                    dashboard.FormClosed += (s, args) => this.Close(); // sluit app als Form2 sluit
                    dashboard.Show(); // toon het dashboard
                }
                else
                {
                    // Geen match → foutmelding
                    lblMessageAwol.Text = "Invalid username or password.";
                }
            }
        }
    }
}
