using Guna.Charts.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cainta_MIS_Ticket
{
    public partial class Login : Form
    {
        private const int MaxFailedAttempts = 5;
        private readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(1);

        private bool isPasswordHidden = true; // State to track password visibility
        private bool isPasswordVisible = false;

        public Login()
        {
            InitializeComponent();
        }

        private void txtUsename_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevents the "ding" sound
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevents the "ding" sound
                btnLogin.PerformClick();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsename.Text;
                string password = txtPass.Text;

                DatabaseConnection dbcon = new DatabaseConnection();
                using (MySqlConnection conn = dbcon.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id, password, salt, failed_attempts, last_failed_attempt, usertype FROM users WHERE username = @Username";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32("id");
                                string storedHash = reader["password"].ToString();
                                string storedSalt = reader["salt"].ToString();
                                int failedAttempts = reader.GetInt32("failed_attempts");
                                DateTime? lastFailedAttempt = reader.IsDBNull(reader.GetOrdinal("last_failed_attempt"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime("last_failed_attempt");
                                string usertype = reader["usertype"].ToString();

                                // Check if user is locked out
                                if (failedAttempts >= MaxFailedAttempts && lastFailedAttempt.HasValue &&
                                    DateTime.Now < lastFailedAttempt.Value.Add(LockoutDuration))
                                {
                                    MessageBox.Show($"Account locked. Try again in {LockoutDuration.TotalMinutes} minutes.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                // Verify the password
                                if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                                {
                                    // Reset failed attempts on successful login
                                    ResetFailedAttempts(userId);
                                    //MessageBox.Show("Login successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Set session information
                                    UserSession.UserId = userId;
                                    UserSession.Username = username;
                                    UserSession.Usertype = usertype;

                                    // Close the first reader before executing the second query
                                    reader.Close();

                                    // Fetch picture from userimages table
                                    string pictureQuery = "SELECT picture FROM userimages WHERE user_id = @UserId";
                                    using (MySqlCommand pictureCmd = new MySqlCommand(pictureQuery, conn))
                                    {
                                        pictureCmd.Parameters.AddWithValue("@UserId", userId);
                                        using (MySqlDataReader pictureReader = pictureCmd.ExecuteReader())
                                        {
                                            if (pictureReader.Read())
                                            {
                                                byte[] pictureData = (byte[])pictureReader["picture"];
                                                UserSession.PictureData = pictureData;
                                            }
                                        }
                                    }

                                    // Log successful login
                                    MIS.LogAuditTrail(username, "User logged in", "Login");

                                    // Redirect based on user type
                                    if (usertype.Equals("Encoder", StringComparison.OrdinalIgnoreCase))
                                    {
                                        // Open Admin Home
                                        MIS mis = new MIS();
                                        mis.Show();
                                    }
                                    else
                                    {
                                        MIS mis = new MIS();
                                        mis.Show();
                                        // Open Encoder Home
                                        //EncoderHome encoderHome = new EncoderHome();
                                        //encoderHome.Show();
                                    }

                                    this.Hide(); // Hide the login form
                                }
                                else
                                {
                                    // Increment failed attempts on unsuccessful login
                                    IncrementFailedAttempts(userId, failedAttempts);
                                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetFailedAttempts(int userId)
        {
            DatabaseConnection dbcon = new DatabaseConnection();
            using (MySqlConnection conn = dbcon.GetConnection())
            {
                conn.Open();
                string query = "UPDATE users SET failed_attempts = 0, last_failed_attempt = NULL WHERE id = @UserId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void IncrementFailedAttempts(int userId, int failedAttempts)
        {
            DatabaseConnection dbcon = new DatabaseConnection();
            using (MySqlConnection conn = dbcon.GetConnection())
            {
                conn.Open();
                string query = "UPDATE users SET failed_attempts = @FailedAttempts, last_failed_attempt = @LastFailedAttempt WHERE id = @UserId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FailedAttempts", failedAttempts + 1);
                    cmd.Parameters.AddWithValue("@LastFailedAttempt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void TogglePasswordVisibility(Guna.UI2.WinForms.Guna2TextBox txtPass)
        {
            if (txtPass.UseSystemPasswordChar)
            {
                // Show the password
                txtPass.UseSystemPasswordChar = false;  // Disable masking
                txtPass.PasswordChar = '\0';  // Ensure no masking character is used
                txtPass.IconRight = Properties.Resources.hidden;  // Change icon to "view"
            }
            else
            {
                // Hide the password
                txtPass.UseSystemPasswordChar = true;   // Enable masking
                txtPass.PasswordChar = '●';  // You can set this to '*' or any preferred masking character
                txtPass.IconRight = Properties.Resources.view;  // Change icon to "hidden"
            }
        }

        private void txtPass_IconRightClick(object sender, EventArgs e)
        {
            TogglePasswordVisibility((Guna.UI2.WinForms.Guna2TextBox)sender);
        }
    }
}
