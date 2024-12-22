using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cainta_MIS_Ticket
{
    public partial class ReportTicketVolume : UserControl
    {
        //#0C9070
        //#D4070F
        //#FFFFFF
        // Define default colors for buttons
        Color defaultBackColor1 = ColorTranslator.FromHtml("#0C9070");
        Color defaultForeColor1 = Color.White;

        Color defaultBackColor2 = ColorTranslator.FromHtml("#D4070F");
        Color defaultForeColor2 = Color.White;

        Color defaultBackColor3 = ColorTranslator.FromHtml("#FFFFFF");
        Color defaultForeColor3 = Color.Black;

        public ReportTicketVolume()
        {
            InitializeComponent();

            //dgvRecentActivity
            dgvReportDay.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 12, FontStyle.Bold);
            dgvReportDay.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 10.5F, FontStyle.Regular);

            btnCheck.Text = "\u2714"; // Displays ✔
            btnCross.Text = "\u2718"; // Displays ✘
        }

        private void pnTop_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                // Create a LinearGradientBrush for the background
                using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0),
                    new Point(pnTop.Width, 0), // Horizontal gradient
                    Color.FromArgb(255, 255, 255),
                    Color.FromArgb(0, 77, 128)))
                {
                    // Set gradient stops
                    ColorBlend colorBlend = new ColorBlend();
                    colorBlend.Colors = new Color[]
                    {
                Color.FromArgb(255, 255, 255), // Start color
                Color.FromArgb(255, 255, 255), // 25% stop color
                Color.FromArgb(0, 77, 128)   // 100% stop color
                    };
                    colorBlend.Positions = new float[] { 0.0f, 0.25f, 1.0f }; // Positions for the stops

                    brush.InterpolationColors = colorBlend;

                    // Paint the panel with the gradient background
                    e.Graphics.FillRectangle(brush, new Rectangle(0, 0, pnTop.Width, pnTop.Height));
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, show a message to the user, etc.)
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void ReportTicketVolume_Load(object sender, EventArgs e)
        {
            // Set default colors for buttons
            btnCheck.BackColor = defaultBackColor3;
            btnCheck.ForeColor = defaultForeColor3;

            btnCross.BackColor = defaultBackColor3;
            btnCross.ForeColor = defaultForeColor3;

            lblDate.Text = DateTime.Now.ToString("dd MMMM yyyy");
            lblDay.Text = DateTime.Now.ToString("dddd").ToUpper();
            lblDayDigit.Text = DateTime.Now.Day.ToString();

            pnTicketEncoder.BringToFront();

            FillTotalTicket();

            FillDataGridViewTicketReport();
        }

        public void FillTotalTicket()
        {
            try
            {
                int Totaltickets = 0;

                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT SUM(total_count) AS TOTAL FROM ( SELECT COUNT(*) AS total_count FROM ticket_technician UNION ALL SELECT COUNT(*) AS total_count FROM ticket_encoder ) AS combined_counts;";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Totaltickets = Convert.ToInt32(reader["TOTAL"]);
                        lblTotalTicket.Text = Totaltickets.ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void FillDataGridViewTicketReport(string filterCondition = "")
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string query = ReportQueryUtility.GetReportQuery(filterCondition);

                    MySqlDataAdapter sda = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();

                    sda.Fill(dt);

                    // Sort DataTable by 'DATE' column
                    dt.DefaultView.Sort = "DATE ASC";

                    dgvReportDay.DataSource = dt;
                }
                cmbSelect.Text = "Select";
                cmbSelect.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = cmbSelect.SelectedItem.ToString();
            switch (selectedItem)
            {
                case "Encoder":
                    pnTicketEncoder.BringToFront();
                    break;
                case "Technician":
                    pnTicketTech.BringToFront();
                    break;
                default:
                    pnTicketEncoder.BringToFront();
                    break;
            }
        }

        private void btnPrevDate_Click(object sender, EventArgs e)
        {
            // Decrement the date by one day
            DateTime currentDate = DateTime.ParseExact(lblDate.Text, "dd MMMM yyyy", null);
            currentDate = currentDate.AddDays(-1);
            lblDate.Text = currentDate.ToString("dd MMMM yyyy");

            // Call method to refresh the DataGridView with the updated date
            FillDataGridViewTicketReport($"DATE = '{currentDate.ToString("MM/dd/yyyy")}'");
        }

        private void btnNextDate_Click(object sender, EventArgs e)
        {
            // Increment the date by one day
            DateTime currentDate = DateTime.ParseExact(lblDate.Text, "dd MMMM yyyy", null);
            currentDate = currentDate.AddDays(1);
            lblDate.Text = currentDate.ToString("dd MMMM yyyy");

            // Call method to refresh the DataGridView with the updated date
            FillDataGridViewTicketReport($"DATE = '{currentDate.ToString("MM/dd/yyyy")}'");
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            // Set the label text to today's date
            lblDate.Text = DateTime.Now.ToString("dd MMMM yyyy");

            // Refresh the DataGridView with today's date filter
            FillDataGridViewTicketReport($"DATE = '{DateTime.Now.ToString("MM/dd/yyyy")}'");
        }

        private void ResetButtonColors()
        {
            // Reset all buttons to default colors
            btnCheck.BackColor = defaultBackColor3;
            btnCheck.ForeColor = defaultForeColor3;

            btnCross.BackColor = defaultBackColor3;
            btnCross.ForeColor = defaultForeColor3;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            // Reset all buttons to default
            ResetButtonColors();

            // Change button 1 colors
            btnCheck.BackColor = defaultBackColor1;
            btnCheck.ForeColor = defaultForeColor1;

            FillDataGridViewTicketReport("resolved = 'yes'");
        }

        private void btnCross_Click(object sender, EventArgs e)
        {
            // Reset all buttons to default
            ResetButtonColors();

            // Change button 2 colors
            btnCross.BackColor = defaultBackColor2;
            btnCross.ForeColor = defaultForeColor2;

            FillDataGridViewTicketReport("resolved = 'no'");
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            // Reset all buttons to default
            ResetButtonColors();

            // Change button 2 colors
            btnCross.BackColor = defaultBackColor3;
            btnCross.ForeColor = defaultForeColor3;
            btnCheck.BackColor = defaultBackColor3;
            btnCheck.ForeColor = defaultForeColor3;

            FillDataGridViewTicketReport();
        }
    }
}
