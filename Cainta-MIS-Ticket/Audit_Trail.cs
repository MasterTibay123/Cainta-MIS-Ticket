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
    public partial class Audit_Trail : UserControl
    {
        public Audit_Trail()
        {
            InitializeComponent();
            //dgvAudit
            dgvAudit.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 13.5F, FontStyle.Bold);
            dgvAudit.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 12, FontStyle.Regular);
            FillGridViewAudit();
        }

        public void FillGridViewAudit(DateTime? selectedDateAudit = null)
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string query = AuditQueryUtility.GetAuditQuery();

                    if (selectedDateAudit.HasValue)
                    {
                        query += " WHERE DATE(a.log_date) = @selectedDate";
                    }

                    query += ";"; // Ensure query ends with a semicolon

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (selectedDateAudit.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@selectedDate", selectedDateAudit.Value.ToString("yyyy-MM-dd"));
                    }

                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    sda.Fill(dt);

                    dgvAudit.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
