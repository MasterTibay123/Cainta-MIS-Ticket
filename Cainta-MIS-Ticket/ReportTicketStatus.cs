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
    public partial class ReportTicketStatus : UserControl
    {
        public ReportTicketStatus()
        {
            InitializeComponent();
            FillTotalTicketResolved();
            FillTotalTicketNotResolved();
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

        private void ReportTicketStatus_Load(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToString("dd MMMM yyyy");
            lblDay.Text = DateTime.Now.ToString("dddd").ToUpper();
            lblDayDigit.Text = DateTime.Now.Day.ToString();
        }

        public void FillTotalTicketResolved()
        {
            try
            {
                int Totaltickets = 0;

                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT 
                        SUM(resolved_count) AS `TOTAL_RESOLVED`
                    FROM (
                        SELECT 
                            SUM(CASE WHEN resolved = 'yes' THEN 1 ELSE 0 END) AS resolved_count
                        FROM ticket_technician t

                        UNION ALL

                        SELECT 
                            SUM(CASE WHEN resolved = 'yes' THEN 1 ELSE 0 END) AS resolved_count
                        FROM ticket_encoder te
                    ) AS combined_counts;
                ";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Access the result using the column name 'TOTAL_RESOLVED'
                        if (reader["TOTAL_RESOLVED"] != DBNull.Value)
                        {
                            Totaltickets = Convert.ToInt32(reader["TOTAL_RESOLVED"]);
                        }
                        else
                        {
                            Totaltickets = 0; // Set to 0 if the value is DBNull
                        }
                        lblTicketResolved.Text = Totaltickets.ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void FillTotalTicketNotResolved()
        {
            try
            {
                int Totaltickets = 0;

                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT 
                        SUM(not_resolved_count) AS `NOT_RESOLVED`
                    FROM (
                        SELECT 
                            SUM(CASE WHEN resolved = 'no' THEN 1 ELSE 0 END) AS not_resolved_count
                        FROM ticket_technician t

                        UNION ALL

                        SELECT 
                            SUM(CASE WHEN resolved = 'no' THEN 1 ELSE 0 END) AS not_resolved_count
                        FROM ticket_encoder te
                    ) AS combined_counts;
                        ";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (reader["NOT_RESOLVED"] != DBNull.Value)
                        {
                            Totaltickets = Convert.ToInt32(reader["NOT_RESOLVED"]);
                        }
                        else
                        {
                            Totaltickets = 0; // Set to 0 if the value is DBNull
                        }
                        lblTicketNotResolved.Text = Totaltickets.ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
