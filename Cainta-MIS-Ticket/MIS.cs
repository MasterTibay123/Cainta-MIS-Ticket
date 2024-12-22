using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Microsoft.Office.Interop.Excel;
//using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms.DataVisualization.Charting;
using PdfSharp.Pdf;

namespace Cainta_MIS_Ticket
{
    public partial class MIS : Form
    {
        #region Button Colors ticket
        // Define default colors for buttons
        Color defaultBackColor1 = ColorTranslator.FromHtml("#0C9070"); //Sea Green
        Color defaultForeColor1 = Color.White;

        Color defaultBackColor2 = ColorTranslator.FromHtml("#D4070F"); //Bright Red
        Color defaultForeColor2 = Color.White;

        Color defaultBackColor3 = Color.White;
        Color defaultForeColor3 = ColorTranslator.FromHtml("#4D4D4D"); //Charcoal Gray
        #endregion

        #region Button Colors report
        // Define default colors for buttons
        Color defaultBackColor4 = ColorTranslator.FromHtml("#487FA4");
        Color defaultForeColor4 = Color.White;

        Color defaultBackColor5 = Color.White;
        Color defaultForeColor5 = ColorTranslator.FromHtml("#4D4D4D");

        Color defaultBackColor6 = Color.White;
        Color defaultForeColor6 = ColorTranslator.FromHtml("#4D4D4D");
        #endregion

        #region Default and Hover Colors
        // Define the default and hover colors
        Color defaultColor = Color.FromArgb(0, 77, 128);
        Color hoverColor = Color.FromArgb(145, 178, 203);
        #endregion

        #region Additional Colors
        // Additional color
        Color hexColor = ColorTranslator.FromHtml("#D3D3D3");
        #endregion

        #region Default Texts
        // Default texts
        private const string DefaultTextUser = "Enter your User Name";
        private const string DefaultTextEmail = "Enter your Email";
        private const string DefaultTextPass = "Enter your Password";
        private const string DefaultTextRetypePass = "Retype your Password";
        #endregion
        /*
        #region Auto Increment
        private int countEncoder = 0; // Initialize the countEncoder
        private int countTech = 0; // Initialize the countTech
        #endregion
        */
        // Declare ticketsCountLabel as a field in your form class
        private System.Windows.Forms.Label ticketsTechCountLabel;
        private System.Windows.Forms.Label ticketsEncoderCountLabel;

        private ToolStripMenuItem _selectedMenuItem;

        public MIS()
        {
            InitializeComponent();
            //Chart
            LoadTechnicianPerformanceChart();
            LoadEncoderPerformanceChart();

            //dgvRecentActivity
            dgvRecentActivity.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 13.5F, FontStyle.Bold);
            dgvRecentActivity.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 12, FontStyle.Regular);
            //dgvTicketEncoder
            dgvTicketEncoder.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 13.5F, FontStyle.Bold);
            dgvTicketEncoder.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 10.5F, FontStyle.Regular);
            dgvTicketEncoder.AlternatingRowsDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 10.5F, FontStyle.Regular);
            //dgvTicketTech
            dgvTicketTech.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 12, FontStyle.Bold);
            dgvTicketTech.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 10.5F, FontStyle.Regular);
            dgvTicketTech.AlternatingRowsDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 10.5F, FontStyle.Regular);
            //dgvFilesIn
            dgvFilesIn.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 13.5F, FontStyle.Bold);
            dgvFilesIn.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 12, FontStyle.Regular);
            //dgvFilesOut
            dgvFilesOut.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 13.5F, FontStyle.Bold);
            dgvFilesOut.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 12, FontStyle.Regular);
            //dgvAccount
            dgvAccount.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins", 13.5F, FontStyle.Bold);
            dgvAccount.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 12, FontStyle.Regular);

            btnCheck.Text = "\u2714"; // Displays ✔
            btnCross.Text = "\u2718"; // Displays ✘
            /*
            #region
            // dgvAccount
            dgvAccount.Paint += new PaintEventHandler(dgvAccount_Paint);
            #endregion
            */
            #region Custom Renderer
            // Assign the custom renderer to the ToolStrip
            menuStrip1.Renderer = new CustomToolStripRenderer();
            #endregion

            #region User Information
            lblUsername.Text = UserSession.Username;
            this.Text = UserSession.Usertype;

            if (UserSession.PictureData != null)
            {
                using (var ms = new System.IO.MemoryStream(UserSession.PictureData))
                {
                    pictureBoxUser.Image = Image.FromStream(ms);
                }
            }
            #endregion

            #region Default Panel
            pnDash.BringToFront();
            #endregion
        }

        private void LoadTechnicianPerformanceChart()
        {
            DatabaseConnection dbConnection = new DatabaseConnection();
            using (MySqlConnection connection = dbConnection.GetConnection())
            {
                string query = @"SELECT tech.name AS TechnicianName, COUNT(t.id) AS ResolvedTickets
                         FROM ticket_technician t
                         LEFT JOIN technician tech ON t.technician_id = tech.technician_id
                         WHERE t.resolved = 'Yes'
                         GROUP BY tech.name
                         ORDER BY ResolvedTickets DESC";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Set up the chart
                    chartTech.Series.Clear();
                    chartTech.ChartAreas.Clear();
                    chartTech.Titles.Clear();  // Clear any existing titles

                    chartTech.ChartAreas.Add(new ChartArea("TechnicianPerformance"));

                    Series series = new Series
                    {
                        Name = "Technician Performance",
                        ChartType = SeriesChartType.Column,
                        Color = System.Drawing.Color.FromArgb(0, 77, 128), // Custom color
                        IsVisibleInLegend = false // Hide the series name in the legend
                    };

                    chartTech.Series.Add(series);

                    while (reader.Read())
                    {
                        series.Points.AddXY(reader["TechnicianName"].ToString(), Convert.ToInt32(reader["ResolvedTickets"]));
                    }

                    // Add title to the chart
                    Title chartTitle = new Title
                    {
                        Text = "Technician Performance",
                        Docking = Docking.Top,
                        Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                        ForeColor = System.Drawing.Color.FromArgb(0, 77, 128)
                    };
                    chartTech.Titles.Add(chartTitle);

                    var chartArea = chartTech.ChartAreas["TechnicianPerformance"];
                    chartArea.AxisX.Title = ""; // Hide X-axis title
                    chartArea.AxisY.Title = ""; // Hide Y-axis title
                    chartArea.AxisX.Interval = 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void LoadEncoderPerformanceChart()
        {
            DatabaseConnection dbConnection = new DatabaseConnection();
            using (MySqlConnection connection = dbConnection.GetConnection())
            {
                string query = @"SELECT enc.name AS EncoderName, COUNT(t.id) AS ResolvedTickets
                         FROM ticket_encoder t
                         LEFT JOIN encoder enc ON t.encoder_id = enc.encoder_id
                         WHERE t.resolved = 'Yes'
                         GROUP BY enc.name
                         ORDER BY ResolvedTickets DESC";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Set up the chart
                    chartEncoder.Series.Clear();
                    chartEncoder.ChartAreas.Clear();
                    chartEncoder.Titles.Clear();  // Clear any existing titles

                    chartEncoder.ChartAreas.Add(new ChartArea("EncoderPerformance"));

                    Series series = new Series
                    {
                        Name = "Encoder Performance",
                        ChartType = SeriesChartType.Column,
                        Color = System.Drawing.Color.FromArgb(0, 77, 128), // Custom color
                        IsVisibleInLegend = false // Hide the series name in the legend
                    };

                    chartEncoder.Series.Add(series);

                    while (reader.Read())
                    {
                        series.Points.AddXY(reader["EncoderName"].ToString(), Convert.ToInt32(reader["ResolvedTickets"]));
                    }

                    // Add title to the chart
                    Title chartTitle = new Title
                    {
                        Text = "Encoder Performance",
                        Docking = Docking.Top,
                        Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                        ForeColor = System.Drawing.Color.FromArgb(0, 77, 128)
                    };
                    chartEncoder.Titles.Add(chartTitle);

                    var chartArea = chartEncoder.ChartAreas["EncoderPerformance"];
                    chartArea.AxisX.Title = ""; // Hide X-axis title
                    chartArea.AxisY.Title = ""; // Hide Y-axis title
                    chartArea.AxisX.Interval = 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public enum ImageFormatType
        {
            Png,
            Jpeg,
            Bmp,
            Gif,
            Tiff
        }

        private void MIS_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Show a message box with an icon before logging out
            DialogResult result = MessageBox.Show("Are you sure you want to Logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // If user clicks Yes, proceed with logout
            if (result == DialogResult.Yes)
            {
                // Get the username from lblUsername
                string username = lblUsername.Text;

                // Log the activity to the audit trail
                LogAuditTrail(username, "User logged out", "logout");
                // Close any open database connections or dispose of any resources if needed

                // Clear user session
                UserSession.ClearSession();

                // Redirect to login form
                Login login = new Login();
                login.Show();

                // Dispose of the current form
                this.Dispose();
            }
            else
            {
                // If user clicks No, allow the form to close
                e.Cancel = true;
            }
        }

        #region Helper Methods

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        #endregion

        private List<Technician> RetrieveTechniciansFromDatabase()
        {
            List<Technician> technicians = new List<Technician>();

            try
            {
                DatabaseConnection db = new DatabaseConnection();

                using (MySqlConnection connection = db.GetConnection())
                {
                    string query = "SELECT name, position, image FROM technician";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader.GetString("name");
                                string position = reader.GetString("position");
                                byte[] imageBytes = (byte[])reader["image"];
                                Image image = ByteArrayToImage(imageBytes);

                                technicians.Add(new Technician
                                {
                                    Name = name,
                                    Position = position,
                                    Image = image
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving technicians: {ex.Message}");
            }

            return technicians;
        }
        
        private List<Encoder> RetrieveEncodersFromDatabase()
        {
            List<Encoder> encoders = new List<Encoder>();

            try
            {
                DatabaseConnection db = new DatabaseConnection();

                using (MySqlConnection connection = db.GetConnection())
                {
                    string query = "SELECT name, position, image FROM encoder";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader.GetString("name");
                                string position = reader.GetString("position");
                                byte[] imageBytes = (byte[])reader["image"];
                                Image image = ByteArrayToImage(imageBytes);

                                encoders.Add(new Encoder
                                {
                                    Name = name,
                                    Position = position,
                                    Image = image
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving encoders: {ex.Message}");
            }

            return encoders;
        }

        private void btnGotoTotalTicket_MouseEnter(object sender, EventArgs e)
        {
            btnGotoTotalTicket.BackColor = ColorTranslator.FromHtml("#91B2CB");
        }

        private void btnGotoTotalTicket_MouseLeave(object sender, EventArgs e)
        {
            btnGotoTotalTicket.BackColor = ColorTranslator.FromHtml("#004D80");
        }
        
        private void btnGotoResolvedTicket_MouseEnter(object sender, EventArgs e)
        {
            btnGotoResolvedTicket.BackColor = ColorTranslator.FromHtml("#91B2CB");
        }

        private void btnGotoResolvedTicket_MouseLeave(object sender, EventArgs e)
        {
            btnGotoResolvedTicket.BackColor = ColorTranslator.FromHtml("#004D80");
        }

        private void MIS_Load(object sender, EventArgs e)
        {

            btnExpoPdfFilesIn.Visible = true;
            btnExpoPdfFilesOut.Visible = false;

            // Set the default image and Tag when initializing
            pictureBox.Image = Properties.Resources.add_image;
            pictureBox.Tag = "add_image";

            pictureBoxEmpTech.Image = Properties.Resources.add_image;
            pictureBoxEmpTech.Tag = "add_image";

            pictureBoxEmp.Image = Properties.Resources.add_image;
            pictureBoxEmp.Tag = "add_image";

            dtpTicketTechnician.Padding = new Padding(20);

            ToolStripMenuItem1_Click(tsmiDash, EventArgs.Empty);

            // Set default colors for buttons 
            btnCheck.BackColor = defaultBackColor3;
            btnCheck.ForeColor = defaultForeColor3;

            btnCross.BackColor = defaultBackColor3;
            btnCross.ForeColor = defaultForeColor3;

            // Set default colors for buttons Repo
            btnTicketVolume.BackColor = defaultBackColor4;
            btnTicketVolume.ForeColor = defaultForeColor4;
            //reportTicketVolume1.Visible = true;

            btnTicketStatus.BackColor = defaultBackColor5;
            btnTicketStatus.ForeColor = defaultForeColor5;

            btnAuditTrail.BackColor = defaultBackColor6;
            btnAuditTrail.ForeColor = defaultForeColor6;

            // Retrieve technicians' data from database
            List<Technician> technicians = RetrieveTechniciansFromDatabase();

            // Add technician panels to the flow layout panel
            foreach (Technician technician in technicians)
            {
                AddTechnicianPanel(technician.Name, technician.Position, technician.Image);
            }
            
            // Retrieve encoders' data from database
            List<Encoder> encoders = RetrieveEncodersFromDatabase();

            // Add encoder panels to the flow layout panel
            foreach (Encoder encoder in encoders)
            {
                AddEncoderPanel(encoder.Name, encoder.Position, encoder.Image);
            }
            
            // Set default colors for buttons report
            btnTicketVolume.BackColor = defaultBackColor4;
            btnTicketVolume.ForeColor = defaultForeColor4;
            reportTicketVolume1.Visible = true;

            btnTicketStatus.BackColor = defaultBackColor5;
            btnTicketStatus.ForeColor = defaultForeColor5;

            btnAuditTrail.BackColor = defaultBackColor6;
            btnAuditTrail.ForeColor = defaultForeColor6;
            
            // Display Datagrid
            FillGridViewTicketTech();
            FillGridViewTicketEncoder();
            FillDataGridViewAccount();

            FillComboBoxTicketTech();
            FillComboBoxTicketEncoder();

            FillComboBoxTicketList();
            FillComboBoxEncoderList();
            
            FillDataGridViewRecentActivity();
            FillGridViewFilesIn();
            FillGridViewFilesOut();

            FillTotalTicketResolved();
            FillTotalTicket();
        }

        #region ToolStripMenuItem1
        private void ToolStripMenuItem1_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            if (sender is ToolStripMenuItem menuItem)
            {
                // Change color for hover
                if (menuItem != _selectedMenuItem) // Only change if it's not the selected item
                {
                    menuItem.ForeColor = ColorTranslator.FromHtml("#4A90E2");
                    menuItem.Owner.Invalidate();
                }
            }
        }

        private void ToolStripMenuItem1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            if (sender is ToolStripMenuItem menuItem)
            {
                // Revert color if it's not the selected item
                if (menuItem != _selectedMenuItem)
                {
                    menuItem.ForeColor = SystemColors.ControlText;
                    menuItem.Owner.Invalidate();
                }
            }
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                // Reset previous selected item's color
                if (_selectedMenuItem != null && _selectedMenuItem != menuItem)
                {
                    _selectedMenuItem.ForeColor = SystemColors.ControlText;
                }

                // Set the new selected item
                _selectedMenuItem = menuItem;
                menuItem.ForeColor = ColorTranslator.FromHtml("#4A90E2");
                menuItem.Owner.Invalidate();

                // Show the appropriate panel based on the clicked ToolStripMenuItem
                if (menuItem == tsmiDash)
                {
                    pnDash.BringToFront();
                    FillDataGridViewRecentActivity();
                    //Chart
                    LoadTechnicianPerformanceChart();
                }
                else if (menuItem == tsmiTicketDash)
                {
                    pnTicketsDash.BringToFront();
                    pnTicketTechSub.Visible = true;

                    pnAddTicketTech.Visible = false;
                    pnTicketListTech.Visible = true;

                    lblTicketTitle.Text = "Tickets";

                    controlsTicketShow();

                    pnTicketEncoderSub.Visible = false;
                    pnAddTicketEncoder.Visible = false;
                    pnTicketListEncoder.Visible = false;
                    txtTicketEncoderSearch.Visible = false;

                    btnExpoPdfTicketTech.Visible = true;
                    btnExpoPdfTicket.Visible = false;

                    //btnAddTicketEncoder.Visible = false;
                    btnAddTicket.Visible = true;

                    //dgvTicket.Columns.Clear();
                    pnTicketsDash.Refresh();
                    FillGridViewTicketTech();
                    FillGridViewTicketEncoder();

                    // Reset all buttons to default
                    ResetButtonColors();

                    cmbTechnicianList.Text = "Technician";
                    cmbTechnicianList.SelectedIndex = 0;
                }
                else if (menuItem == tsmiEmp)
                {
                    pnEmpDash.BringToFront();
                }

                else if (menuItem == tsmiFiles)
                {
                    pmFilesDash.Visible = true;
                    //pnAccounts.Visible = false;
                    pmFilesDash.BringToFront();
                    
                    FillGridViewFilesIn();
                    FillGridViewFilesOut();

                    txtSearchFilesIn.BringToFront();

                    //pnFilesOut.Visible = true;
                    //pnFilesIn.Visible = true;
                    /*
                    pnFilesInAdd.Visible = false;
                    pnFilesOutAdd.Visible = false;*/
                }

                else if (menuItem == tsmiAccounts)
                {
                    pnAccountsDash.BringToFront();
                    pnAccountList.Visible = true;
                    pnAccCotainer.Visible = false;
                    pictureBox.Image = Properties.Resources.add_image;
                }

                else if (menuItem == tsmiReport)
                {
                    pnReportsDash.BringToFront();
                    btnTicketVolume.PerformClick();
                }
            }
        }
        #endregion

        #region controls
        private void controlsTicketShow()
        {
            pictureBox3.Visible = true;
            cmbSortTicket.Visible = true;
            pictureBox5.Visible = true;
            cmbTechnicianList.Visible = true;
            cmbEncoderList.Visible = true;
            pbTicketCalendar.Visible = true;
            btnCheck.Visible = true;
            btnCross.Visible = true;
            btnNormal.Visible = true;
            btnAddTicket.Visible = true;
            btnExpoExcelTicket.Visible = true;
            txtTicketTechSearch.Visible = true;
            txtTicketEncoderSearch.Visible = true;
        }

        private void controlsTicketHide()
        {
            pictureBox3.Visible = false;
            cmbSortTicket.Visible = false;
            pictureBox5.Visible = false;
            cmbTechnicianList.Visible = false;
            cmbEncoderList.Visible = false;
            pbTicketCalendar.Visible = false;
            btnCheck.Visible = false;
            btnCross.Visible = false;
            btnNormal.Visible = false;
            btnAddTicket.Visible = false;
            btnExpoExcelTicket.Visible = false;
            txtTicketTechSearch.Visible = false;
            txtTicketEncoderSearch.Visible = false;

        }
        #endregion
        /*
        #region btnExcelImport_Click
        private void btnExcelImport_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            Microsoft.Office.Interop.Excel.Range xlRange;

            int xlRow;
            string strFilename;

            openFD.Filter = "Excel Office |*.xls; *xlsx";
            openFD.ShowDialog();
            strFilename = openFD.FileName;

            if (strFilename != "")
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(strFilename);
                xlWorksheet = xlWorkbook.Worksheets["Sheet1"];
                xlRange = xlWorksheet.UsedRange;

                int i = 0;

                for (xlRow = 2; xlRow <= xlRange.Rows.Count; xlRow++)
                {
                    i++;
                    dgvTicketTech.Rows.Add(i, xlRange.Cells[xlRow, 1].Text, xlRange.Cells[xlRow, 2].Text, xlRange.Cells[xlRow, 3].Text, xlRange.Cells[xlRow, 4].Text, xlRange.Cells[xlRow, 5].Text, xlRange.Cells[xlRow, 6].Text, xlRange.Cells[xlRow, 7].Text, xlRange.Cells[xlRow, 8].Text, xlRange.Cells[xlRow, 9].Text, xlRange.Cells[xlRow, 10].Text);
                }

                xlWorkbook.Close();
                xlApp.Quit();
            }
        }
        #endregion
        */
        private void btnAddTicket_Click(object sender, EventArgs e)
        {
            if (pnTicketTechSub.Visible == true)
            {
                pnAddTicketTech.Visible = true;
                pnTicketListTech.Visible = false;

                lblTicketTitle.Text = "Tickets   -  Add";

                controlsTicketHide();

                btnTicketTechSave.BringToFront();

                dtpTicketTechnician.Value = DateTime.Now;

                cmbOfficeTicket.SelectedIndex = -1;
                cmbOfficeTicket.Text = "Select";

                txtTicketCaller.Text = "";

                cmbProblemTicket.SelectedIndex = -1;
                cmbProblemTicket.Text = "Select";
                txtTicketProblem.Text = "";

                txtTicketSolution.Text = "";

                cmbTicketTechnician.SelectedIndex = -1;
                cmbTicketTechnician.Text = "Select";

                txtTicketRecom.Text = "";

                cbYesTech.Checked = false;
                cbNoTech.Checked = false;
            }
            else if (pnTicketEncoderSub.Visible == true)
            {
                pnTicketEncoderSub.Visible = true;
                pnAddTicketEncoder.Visible = true;
                pnTicketListEncoder.Visible = false;

                lblTicketTitle.Text = "Tickets   -  Add";

                controlsTicketHide();

                btnTicketEncoderSave.BringToFront();

                dtpTicketEncoder.Value = DateTime.Now;

                cmbOfficeWorkTicket.SelectedIndex = -1;
                cmbOfficeWorkTicket.Text = "Select";

                cmbTicketEncoder.SelectedIndex = -1;
                cmbTicketEncoder.Text = "Select";

                cbYesEncoder.Checked = false;
                cbNoEncoder.Checked = false;
            }

        }

        private void TxtTicketTechSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Cast the DataGridView's DataSource to a DataTable
                System.Data.DataTable dt = (System.Data.DataTable)dgvTicketTech.DataSource;

                if (dt != null)
                {
                    // Initialize a list to store filter conditions
                    List<string> filterConditions = new List<string>();

                    // Iterate through each column in the DataTable
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.DataType == typeof(string))
                        {
                            // Add a filter condition for string columns
                            filterConditions.Add($"[{column.ColumnName}] LIKE '%{txtTicketTechSearch.Text}%'");
                        }
                        else if (column.DataType == typeof(int) || column.DataType == typeof(double) || column.DataType == typeof(decimal))
                        {
                            // Try parsing the search text to a number and add a filter condition for numeric columns
                            if (decimal.TryParse(txtTicketTechSearch.Text, out decimal searchNumber))
                            {
                                filterConditions.Add($"CONVERT([{column.ColumnName}], 'System.String') LIKE '%{txtTicketTechSearch.Text}%'");
                            }
                        }
                    }

                    // Combine filter conditions into a single expression
                    string filterExpression = string.Join(" OR ", filterConditions);

                    // Apply the filter to the DataTable's DefaultView
                    dt.DefaultView.RowFilter = filterExpression;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while filtering: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTicketEncoderSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Cast the DataGridView's DataSource to a DataTable
                System.Data.DataTable dt = (System.Data.DataTable)dgvTicketEncoder.DataSource;

                if (dt != null)
                {
                    // Initialize a list to store filter conditions
                    List<string> filterConditions = new List<string>();

                    // Iterate through each column in the DataTable
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.DataType == typeof(string))
                        {
                            // Add a filter condition for string columns
                            filterConditions.Add($"[{column.ColumnName}] LIKE '%{txtTicketEncoderSearch.Text}%'");
                        }
                        else if (column.DataType == typeof(int) || column.DataType == typeof(double) || column.DataType == typeof(decimal))
                        {
                            // Try parsing the search text to a number and add a filter condition for numeric columns
                            if (decimal.TryParse(txtTicketEncoderSearch.Text, out decimal searchNumber))
                            {
                                filterConditions.Add($"CONVERT([{column.ColumnName}], 'System.String') LIKE '%{txtTicketEncoderSearch.Text}%'");
                            }
                        }
                    }

                    // Combine filter conditions into a single expression
                    string filterExpression = string.Join(" OR ", filterConditions);

                    // Apply the filter to the DataTable's DefaultView
                    dt.DefaultView.RowFilter = filterExpression;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while filtering: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTicketTechnician_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTicketTechnician.SelectedItem != null)
            {
                string selectedTechnician = cmbTicketTechnician.SelectedValue.ToString();
                // Optionally, you can retrieve the image if you need it for further processing.
                // byte[] technicianImage = GetTechnicianImage(selectedTechnician);
            }
        }

        private void cmbTicketEncoder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTicketEncoder.SelectedItem != null)
            {
                string selectedEncoder = cmbTicketEncoder.SelectedValue.ToString();
                // Optionally, you can retrieve the image if you need it for further processing.
                // byte[] technicianImage = GetTechnicianImage(selectedTechnician);
            }
        }

        private void cmbProblemTicket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProblemTicket.Text == "Others")
            {
                txtTicketProblem.Visible = true;
            }
            else
            {
                txtTicketProblem.Visible = false;
            }
        }

        private void cbYes_CheckedChanged(object sender, EventArgs e)
        {
            if (cbYesTech.Checked == true)
            {
                cbNoTech.Checked = false;
            }
        }

        private void cbNo_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNoTech.Checked == true)
            {
                cbYesTech.Checked = false;
            }
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

            if (pnTicketTechSub.Visible)
            {
                string selectedTechnician = cmbTechnicianList.SelectedValue.ToString();
                FillGridViewTicketTech(selectedTechnician, "yes");
            }
            else if (pnTicketEncoderSub.Visible)
            {
                string selectedEncoder = cmbEncoderList.SelectedValue.ToString();
                FillGridViewTicketEncoder(selectedEncoder, "yes");
            }
        }

        private void btnCross_Click(object sender, EventArgs e)
        {
            // Reset all buttons to default
            ResetButtonColors();

            // Change button 2 colors
            btnCross.BackColor = defaultBackColor2;
            btnCross.ForeColor = defaultForeColor2;

            if (pnTicketTechSub.Visible)
            {
                string selectedTechnician = cmbTechnicianList.SelectedValue.ToString();
                FillGridViewTicketTech(selectedTechnician, "no");
            }
            else if (pnTicketEncoderSub.Visible)
            {
                string selectedEncoder = cmbEncoderList.SelectedValue.ToString();
                FillGridViewTicketEncoder(selectedEncoder, "no");
            }
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

            if (pnTicketTechSub.Visible)
            {
                FillGridViewTicketTech();
            }
            else if (pnTicketEncoderSub.Visible)
            {
                FillGridViewTicketEncoder();
            }
        }

        private void FillDataGridViewRecentActivity()
        {
            try
            {
                #region Initialize and Open Database Connection
                // Initialize database connection
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    #endregion

                    #region Execute Query and Fill DataTable
                    // Execute query to get recent activity
                    string query = ActivityQueryUtility.GetActivityQueryUtility();

                    MySqlDataAdapter sda = new MySqlDataAdapter(query, conn);
                    System.Data.DataTable dt = new System.Data.DataTable();

                    sda.Fill(dt);

                    // Bind the DataTable to the DataGridView
                    dgvRecentActivity.DataSource = dt;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                // Handle any exceptions and show an error message
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                #endregion
            }
        }

        private void FillTotalTicketResolved()
        {
            try
            {
                #region Initialize Variables
                int Totaltickets = 0;
                #endregion

                #region Initialize and Open Database Connection
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    #endregion

                    #region Execute Query and Read Data
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
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                #endregion
            }
        }


        private void FillTotalTicket()
        {
            try
            {
                #region Initialize Variables
                int Totaltickets = 0;
                #endregion

                #region Initialize and Open Database Connection
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    #endregion

                    #region Execute Query and Read Data
                    string query = "SELECT SUM(total_count) AS TOTAL FROM ( SELECT COUNT(*) AS total_count FROM ticket_technician UNION ALL SELECT COUNT(*) AS total_count FROM ticket_encoder ) AS combined_counts;";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Totaltickets = Convert.ToInt32(reader["TOTAL"]);
                        lblTotalTicket.Text = Totaltickets.ToString();
                    }
                    reader.Close();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                #endregion
            }

        }

        private void tsmiTicketEncoderSub_Click(object sender, EventArgs e)
        {
            pnTicketsDash.BringToFront();
            pnTicketEncoderSub.Visible = true;

            pnAddTicketEncoder.Visible = false;
            pnTicketListEncoder.Visible = true;
            cmbEncoderList.Visible = true;

            pnTicketTechSub.Visible = false;
            pnAddTicketTech.Visible = false;
            pnTicketListTech.Visible = false;
            cmbTechnicianList.Visible = false;

            txtTicketTechSearch.Visible = false;
            txtTicketEncoderSearch.Visible = true;

            btnExpoPdfTicketTech.Visible = false;
            btnExpoPdfTicket.Visible = true;

            //btnAddTicketEncoder.Visible = true;
            btnAddTicket.Visible = true;

            //dgvTicket.Columns.Clear();
            pnTicketsDash.Refresh();
            FillGridViewTicketEncoder();
        }

        private void tsmiTicketTechSub_Click(object sender, EventArgs e)
        {
            pnTicketsDash.BringToFront();
            pnTicketTechSub.Visible = true;

            pnAddTicketTech.Visible = false;
            pnTicketListTech.Visible = true;
            lblTicketTitle.Text = "Tickets";
            cmbTechnicianList.Visible = true;

            pnTicketEncoderSub.Visible = false;
            pnAddTicketEncoder.Visible = false;
            pnTicketListEncoder.Visible = false;
            cmbEncoderList.Visible = false;

            btnExpoPdfTicketTech.Visible = true;
            btnExpoPdfTicket.Visible = false;

            //btnAddTicketEncoder.Visible = false;
            btnAddTicket.Visible = true;

            //dgvTicket.Columns.Clear();
            pnTicketsDash.Refresh();
            FillGridViewTicketTech();
        }

        private void cmbSortTicket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnTicketTechSub.Visible)
            {
                string selectedTechnician = cmbTechnicianList.SelectedValue?.ToString();
                string sortOrder = cmbSortTicket.SelectedItem?.ToString();
                FillGridViewTicketTech(selectedTechnician, sortOrderTech: sortOrder);
            }
            else if (pnTicketEncoderSub.Visible)
            {
                string selectedEncoder = cmbEncoderList.SelectedValue?.ToString();
                string sortOrder = cmbSortTicket.SelectedItem?.ToString();
                FillGridViewTicketEncoder(selectedEncoder, sortOrder: sortOrder);
            }
        }

        private void cmbTechnicianList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTechnician = cmbTechnicianList.SelectedValue.ToString();
            FillGridViewTicketTech(selectedTechnician);
        }

        private void cmbEncoderList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEncoder = cmbEncoderList.SelectedValue.ToString();
            FillGridViewTicketEncoder(selectedEncoder);
        }

        private void pbTicketCalendar_Click(object sender, EventArgs e)
        {
            if (pnTicketTechSub.Visible)
            {
                using (MonthCalendar mcTech = new MonthCalendar())
                {
                    Form calendarFormTech = new Form
                    {
                        Text = "Select Date",
                        FormBorderStyle = FormBorderStyle.FixedToolWindow,
                        StartPosition = FormStartPosition.CenterParent,
                        AutoSize = false,
                        Size = new Size(250, 220) // Adjust size as needed
                    };

                    mcTech.MaxSelectionCount = 1;
                    mcTech.Dock = DockStyle.Fill;
                    mcTech.DateSelected += (s, ev) => calendarFormTech.Close();

                    calendarFormTech.Controls.Add(mcTech);
                    calendarFormTech.ShowDialog();

                    DateTime selectedDateTech = mcTech.SelectionStart;
                    string selectedTechnician = cmbTechnicianList.SelectedValue.ToString();
                    FillGridViewTicketTech(selectedTechnician, selectedDateTech: selectedDateTech);
                }
            }
            else if (pnTicketEncoderSub.Visible)
            {
                using (MonthCalendar mcEncoder = new MonthCalendar())
                {
                    Form calendarFormEncoder = new Form
                    {
                        Text = "Select Date",
                        FormBorderStyle = FormBorderStyle.FixedToolWindow,
                        StartPosition = FormStartPosition.CenterParent,
                        AutoSize = false,
                        Size = new Size(250, 220) // Adjust size as needed
                    };

                    mcEncoder.MaxSelectionCount = 1;
                    mcEncoder.Dock = DockStyle.Fill;
                    mcEncoder.DateSelected += (s, ev) => calendarFormEncoder.Close();

                    calendarFormEncoder.Controls.Add(mcEncoder);
                    calendarFormEncoder.ShowDialog();

                    DateTime selectedDateEncoder = mcEncoder.SelectionStart;
                    string selectedEncoder = cmbEncoderList.SelectedValue.ToString();
                    FillGridViewTicketEncoder(selectedEncoder, selectedDateEncoder: selectedDateEncoder);
                }
            }
        }

        public void FillGridViewTicketTech(string technicianName = null, string resolvedStatus = null, DateTime? selectedDateTech = null, string sortOrderTech = null)
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    #region QueryBuilding
                    string query = TicketQueryUtility.GetTicketTechQuery();
                    List<string> conditions = new List<string>();

                    if (!string.IsNullOrEmpty(technicianName) && technicianName != "Technician")
                    {
                        conditions.Add("tech.name = @technicianName");
                    }

                    if (!string.IsNullOrEmpty(resolvedStatus))
                    {
                        conditions.Add("t.resolved = @resolvedStatus");
                    }

                    if (selectedDateTech.HasValue)
                    {
                        conditions.Add("DATE(t.date) = @selectedDate");
                    }

                    if (conditions.Count > 0)
                    {
                        query += " WHERE " + string.Join(" AND ", conditions);
                    }

                    if (!string.IsNullOrEmpty(sortOrderTech))
                    {
                        switch (sortOrderTech)
                        {
                            case "A-Z":
                                query += " ORDER BY tech.name ASC";
                                break;
                            case "Z-A":
                                query += " ORDER BY tech.name DESC";
                                break;
                        }
                    }
                    #endregion

                    #region CommandSetup
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (!string.IsNullOrEmpty(technicianName) && technicianName != "Technician")
                    {
                        cmd.Parameters.AddWithValue("@technicianName", technicianName);
                    }

                    if (!string.IsNullOrEmpty(resolvedStatus))
                    {
                        cmd.Parameters.AddWithValue("@resolvedStatus", resolvedStatus);
                    }

                    if (selectedDateTech.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@selectedDate", selectedDateTech.Value.ToString("yyyy-MM-dd"));
                    }
                    #endregion

                    #region DataBinding
                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    sda.Fill(dt);

                    dgvTicketTech.DataSource = dt;

                    foreach (DataGridViewColumn column in dgvTicketTech.Columns)
                    {
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    #endregion

                    #region EventHandling
                    dgvTicketTech.CellMouseEnter += (sender, e) =>
                    {
                        if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && (dgvTicketTech.Columns[e.ColumnIndex] is DataGridViewImageColumn))
                        {
                            dgvTicketTech.Cursor = Cursors.Hand;
                        }
                    };

                    dgvTicketTech.CellMouseLeave += (sender, e) =>
                    {
                        dgvTicketTech.Cursor = Cursors.Default;
                    };

                    dgvTicketTech.CellFormatting += (sender, e) =>
                    {
                        if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dgvTicketTech.Columns[e.ColumnIndex].Name == "dgvcTechnician")
                        {
                            object cellValue = e.Value;

                            if (cellValue != DBNull.Value && cellValue != null)
                            {
                                if (cellValue.GetType() == typeof(byte[]))
                                {
                                    byte[] imgBytes = (byte[])cellValue;
                                    Image img = Image.FromStream(new MemoryStream(imgBytes));
                                    e.Value = img.GetThumbnailImage(30, 30, null, IntPtr.Zero);
                                }
                                else if (cellValue.GetType() == typeof(Bitmap))
                                {
                                    Bitmap bitmap = (Bitmap)cellValue;
                                    e.Value = bitmap.GetThumbnailImage(30, 30, null, IntPtr.Zero);
                                }
                            }
                        }
                    };
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTicketTech_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is the "Edit" button
            if (e.ColumnIndex == dgvTicketTech.Columns["EditTech"].Index && e.RowIndex >= 0)
            {
                // Get the registry_no from the selected row
                string idNo = dgvTicketTech.Rows[e.RowIndex].Cells["dgvcIDTech"].Value.ToString();

                // Retrieve data from the database based on registry_no
                PopulateEditFieldsTech(idNo);
                lblTicketTitle.Text = "Tickets   -  Edit";
                lblTitleLTicketInfo.Text = "Edit Ticket";
                pnAddTicketTech.Visible = true;
                pnTicketListTech.Visible = false;

                controlsTicketHide();

                btnTicketTechUpdateSave.BringToFront();
            }

        }

        private void PopulateEditFieldsTech(string idNo)
        {
            #region Database Connection and Query

            using (MySqlConnection connection = new DatabaseConnection().GetConnection())
            {
                // Prepare SQL command
                string query = TicketQueryUtility.GetTicketTechDetailsQuery();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@idNo", idNo);

                try
                {
                    // Open the connection
                    connection.Open();
                    // Execute the command
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Populate edit fields with data from the reader
                            txtTicketTechID.Text = reader["id"].ToString(); // Use the alias 'TICKET #'
                            dtpTicketTechnician.Value = Convert.ToDateTime(reader["DATE"]);
                            cmbOfficeTicket.Text = reader["office"].ToString();
                            txtTicketCaller.Text = reader["caller"].ToString();
                            cmbProblemTicket.Text = reader["problem"].ToString();
                            txtTicketProblem.Text = reader["problem"].ToString();
                            txtTicketSolution.Text = reader["solution"].ToString();
                            cmbTicketTechnician.Text = reader["name"].ToString();
                            txtTicketRecom.Text = reader["recommendation"].ToString();

                            // Set CheckBoxes based on the resolved column
                            bool resolvedTech = reader["resolved"].ToString().ToUpper() == "YES";
                            cbYesTech.Checked = resolvedTech;
                            cbNoTech.Checked = !resolvedTech;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            #endregion Database Connection and Query
        }

        private void dgvTicketTech_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the cell
                DataGridViewCell cell = dgvTicketTech.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Check if the cell is in the image column
                if (cell.OwningColumn.Name == "dgvcTechnician")
                {
                    // Get the image name from the corresponding row
                    string imageName = dgvTicketTech.Rows[e.RowIndex].Cells["dgvcName"].Value.ToString();

                    // Set the tooltip text
                    dgvTicketTech.ShowCellToolTips = true;
                    cell.ToolTipText = imageName;
                }
            }
        }

        #region ComboBox Methods

        void FillComboBoxTicketTech()
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    MySqlDataAdapter sda = new MySqlDataAdapter("select DISTINCT name from technician", conn);
                    DataSet dt = new DataSet();
                    sda.Fill(dt);

                    // Add default rows
                    DataRow defaultRowTicket = dt.Tables[0].NewRow();
                    defaultRowTicket["name"] = "Select";
                    dt.Tables[0].Rows.InsertAt(defaultRowTicket, 0);

                    // Bind DataTable to ComboBoxes
                    cmbTicketTechnician.DataSource = dt.Tables[0];
                    cmbTicketTechnician.DisplayMember = "name";
                    cmbTicketTechnician.ValueMember = "name";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void FillComboBoxTicketEncoder()
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    MySqlDataAdapter sda = new MySqlDataAdapter("select DISTINCT name from encoder", conn);
                    DataSet dt = new DataSet();
                    sda.Fill(dt);

                    // Add default rows
                    DataRow defaultRowTicket = dt.Tables[0].NewRow();
                    defaultRowTicket["name"] = "Select";
                    dt.Tables[0].Rows.InsertAt(defaultRowTicket, 0);

                    // Bind DataTable to ComboBoxes
                    cmbTicketEncoder.DataSource = dt.Tables[0];
                    cmbTicketEncoder.DisplayMember = "name";
                    cmbTicketEncoder.ValueMember = "name";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void FillComboBoxTicketList()
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    MySqlDataAdapter sda = new MySqlDataAdapter("select DISTINCT name from technician", conn);
                    DataSet dt = new DataSet();
                    sda.Fill(dt);

                    // Add default rows
                    DataRow defaultRowTicket = dt.Tables[0].NewRow();
                    defaultRowTicket["name"] = "Technician";
                    dt.Tables[0].Rows.InsertAt(defaultRowTicket, 0);

                    // Bind DataTable to ComboBoxes
                    cmbTechnicianList.DataSource = dt.Tables[0];
                    cmbTechnicianList.DisplayMember = "name";
                    cmbTechnicianList.ValueMember = "name";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillComboBoxEncoderList()
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    MySqlDataAdapter sda = new MySqlDataAdapter("SELECT DISTINCT name FROM encoder", conn);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    // Add default row
                    DataRow defaultRow = ds.Tables[0].NewRow();
                    defaultRow["name"] = "Encoder";
                    ds.Tables[0].Rows.InsertAt(defaultRow, 0);

                    // Bind DataTable to ComboBox
                    cmbEncoderList.DataSource = ds.Tables[0];
                    cmbEncoderList.DisplayMember = "name";
                    cmbEncoderList.ValueMember = "name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetTechnicianId(string technicianName)
        {
            DatabaseConnection dbConnection = new DatabaseConnection();
            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = "SELECT technician_id FROM technician WHERE name = @Name";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", technicianName);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private int GetEncoderId(string encoderName)
        {
            DatabaseConnection dbConnection = new DatabaseConnection();
            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = "SELECT encoder_id FROM encoder WHERE name = @Name";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", encoderName);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        #endregion

        #region Event Handlers

        private bool ValidateInputsTicketTech()
        {

            if (string.IsNullOrWhiteSpace(cmbOfficeTicket.Text) ||
                string.IsNullOrWhiteSpace(txtTicketCaller.Text) ||
                string.IsNullOrWhiteSpace(cmbProblemTicket.Text) ||
                string.IsNullOrWhiteSpace(txtTicketSolution.Text))
            {
                MessageBox.Show("All fields must be filled out.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnTicketTechSave_Click(object sender, EventArgs e)
        {
            if (cmbTicketTechnician.Text == "Select")
            {
                MessageBox.Show("Please select a technician.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!cbYesTech.Checked && !cbNoTech.Checked)
            {
                MessageBox.Show("Please check either 'Yes' or 'No'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateInputsTicketTech())
            {
                try
                {
                    DatabaseConnection dbConnection = new DatabaseConnection();
                    using (MySqlConnection conn = dbConnection.GetConnection())
                    {
                        conn.Open();

                        string resolvedValue = cbYesTech.Checked ? "yes" : "no"; // Determine 'yes' or 'no' based on checkbox state
                        int technicianId = GetTechnicianId(cmbTicketTechnician.Text.Trim());

                        string query = "INSERT INTO ticket_technician (date, office, caller, problem, solution, technician_id, recommendation, resolved) " +
                                       "VALUES (@Date, @Office, @Caller, @Problem, @Solution, @TechnicianId, @Recommendation, @Resolved)";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            if (cmbProblemTicket.Text == "Others")
                            {
                                cmd.Parameters.AddWithValue("@Date", dtpTicketTechnician.Value);
                                cmd.Parameters.AddWithValue("@Office", cmbOfficeTicket.Text.Trim());
                                cmd.Parameters.AddWithValue("@Caller", txtTicketCaller.Text.Trim());
                                cmd.Parameters.AddWithValue("@Problem", cmbProblemTicket.Text.Trim() + " (" + txtTicketProblem.Text.Trim() + ")");
                                cmd.Parameters.AddWithValue("@Solution", txtTicketSolution.Text.Trim());
                                cmd.Parameters.AddWithValue("@TechnicianId", technicianId);
                                cmd.Parameters.AddWithValue("@Recommendation", txtTicketRecom.Text.Trim());
                                cmd.Parameters.AddWithValue("@Resolved", resolvedValue);

                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Date", dtpTicketTechnician.Value);
                                cmd.Parameters.AddWithValue("@Office", cmbOfficeTicket.Text.Trim());
                                cmd.Parameters.AddWithValue("@Caller", txtTicketCaller.Text.Trim());
                                cmd.Parameters.AddWithValue("@Problem", cmbProblemTicket.Text.Trim());
                                cmd.Parameters.AddWithValue("@Solution", txtTicketSolution.Text.Trim());
                                cmd.Parameters.AddWithValue("@TechnicianId", technicianId);
                                cmd.Parameters.AddWithValue("@Recommendation", txtTicketRecom.Text.Trim());
                                cmd.Parameters.AddWithValue("@Resolved", resolvedValue);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Log the activity to the audit trail
                        string username = lblUsername.Text;
                        LogAuditTrail(username, "Created New Ticket", technicianId.ToString());

                        // Update ticketsCountLabel for the technician
                        int ticketsDone = FetchNumberOfTicketsDoneTech(cmbTicketTechnician.Text.Trim());
                        ticketsTechCountLabel.Text = ticketsDone.ToString();

                        MessageBox.Show("Ticket saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FillGridViewTicketTech();

                        FillComboBoxTicketTech();
                        FillDataGridViewRecentActivity();
                        FillGridViewFilesIn();
                        FillGridViewFilesOut();

                        FillTotalTicketResolved();
                        FillTotalTicket();

                        //UserControl reportTicketStatus1
                        reportTicketStatus1.FillTotalTicketResolved();
                        reportTicketStatus1.FillTotalTicketNotResolved();

                        //UserControl reportTicketVolume1
                        reportTicketVolume1.FillDataGridViewTicketReport();
                        reportTicketVolume1.FillTotalTicket();

                        // Optional: Re-fetch and re-fill other data if needed
                        FillComboBoxTicketTech();
                        pnAddTicketTech.Visible = false;
                        pnTicketListTech.Visible = true;
                        lblTicketTitle.Text = "Tickets";

                        controlsTicketShow();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        private void btnTicketTechUpdateSave_Click(object sender, EventArgs e)
        {
            if (cmbTicketTechnician.Text == "Select")
            {
                MessageBox.Show("Please select a technician.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!cbYesTech.Checked && !cbNoTech.Checked)
            {
                MessageBox.Show("Please check either 'Yes' or 'No'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateInputsTicketTech())
            {
                try
                {
                    DatabaseConnection dbConnection = new DatabaseConnection();
                    using (MySqlConnection conn = dbConnection.GetConnection())
                    {
                        conn.Open();

                        string resolvedValue = cbYesTech.Checked ? "yes" : "no"; // Determine 'yes' or 'no' based on checkbox state
                        int technicianId = GetTechnicianId(cmbTicketTechnician.Text.Trim());

                        string query = "UPDATE ticket_technician SET date = @Date, office = @Office, caller = @Caller, problem = @Problem, " +
                                       "solution = @Solution, technician_id = @TechnicianId, recommendation = @Recommendation, " +
                                       "resolved = @Resolved WHERE id = @TicketId"; // Assuming 'ticket_id' is the primary key

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Date", dtpTicketTechnician.Value);
                            cmd.Parameters.AddWithValue("@Office", cmbOfficeTicket.Text.Trim());
                            cmd.Parameters.AddWithValue("@Caller", txtTicketCaller.Text.Trim());
                            if (cmbProblemTicket.Text == "Others")
                            {
                                cmd.Parameters.AddWithValue("@Problem", cmbProblemTicket.Text.Trim() + " (" + txtTicketProblem.Text.Trim() + ")");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Problem", cmbProblemTicket.Text.Trim());
                            }
                            cmd.Parameters.AddWithValue("@Solution", txtTicketSolution.Text.Trim());
                            cmd.Parameters.AddWithValue("@TechnicianId", technicianId);
                            cmd.Parameters.AddWithValue("@Recommendation", txtTicketRecom.Text.Trim());
                            cmd.Parameters.AddWithValue("@Resolved", resolvedValue); // Use resolvedValue determined above
                            cmd.Parameters.AddWithValue("@TicketId", txtTicketTechID.Text.Trim()); // Add the ID of the ticket being updated

                            cmd.ExecuteNonQuery();
                        }

                        #region
                        // Get the username from lblUsername
                        string username = lblUsername.Text;

                        // Log the activity to the audit trail
                        LogAuditTrail(username, "Updated Record Ticket", technicianId.ToString());
                        #endregion

                        // After inserting ticket, update ticketsCountLabel
                        string technicianName = cmbTicketTechnician.Text.Trim(); // Assuming this gets the technician's name

                        // Update ticketsCountLabel for the technician
                        int ticketsDone = FetchNumberOfTicketsDoneTech(technicianName);
                        ticketsTechCountLabel.Text = ticketsDone.ToString();

                        MessageBox.Show("Ticket updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FillGridViewTicketTech();

                        FillComboBoxTicketTech();
                        FillDataGridViewRecentActivity();
                        FillGridViewFilesIn();
                        FillGridViewFilesOut();

                        FillTotalTicketResolved();
                        FillTotalTicket();

                        //UserControl reportTicketStatus1
                        reportTicketStatus1.FillTotalTicketResolved();
                        reportTicketStatus1.FillTotalTicketNotResolved();

                        //UserControl reportTicketVolume1
                        reportTicketVolume1.FillDataGridViewTicketReport();
                        reportTicketVolume1.FillTotalTicket();

                        pnAddTicketTech.Visible = false;
                        pnTicketListTech.Visible = true;
                        lblTicketTitle.Text = "Tickets";

                        controlsTicketShow();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsmiEncoder_Click(object sender, EventArgs e)
        {
            lblEmpTitle.Text = "Encoders Profile";
            pnEmpDash.BringToFront();
            pnEncoderSub.Visible = true;
            pnTechSub.Visible = false;

            pnAddEncoder.Visible = false;
            pnListEncoder.Visible = true;

            cmbSortTechnician.Visible = false;
            cmbSortEncoder.Visible = true;

            cmbSortEncoder.Text = "Sort";
            cmbSortEncoder.SelectedIndex = 0;

            controlsShowEmp();

            //flpnTechList.Refresh();
        }

        private void tsmiTech_Click(object sender, EventArgs e)
        {
            lblEmpTitle.Text = "Technicians Profile";
            pnEmpDash.BringToFront();
            pnTechSub.Visible = true;
            pnEncoderSub.Visible = false;

            pnAddTech.Visible = false;
            pnListTech.Visible = true;

            cmbSortTechnician.Visible = true;
            cmbSortEncoder.Visible = false;

            cmbSortTechnician.Text = "Sort";
            cmbSortTechnician.SelectedIndex = 0;

            controlsShowEmp();
        }

        #region ImageToByteArray Method
        public byte[] ImageToByteArray(Image image, ImageFormatType formatType)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png; // Default format

                switch (formatType)
                {
                    case ImageFormatType.Jpeg:
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case ImageFormatType.Bmp:
                        format = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;
                    case ImageFormatType.Gif:
                        format = System.Drawing.Imaging.ImageFormat.Gif;
                        break;
                    case ImageFormatType.Tiff:
                        format = System.Drawing.Imaging.ImageFormat.Tiff;
                        break;
                }

                // Save the image to the MemoryStream
                image.Save(ms, format);

                // Reset the position to the beginning of the stream
                ms.Seek(0, SeekOrigin.Begin);

                // Convert MemoryStream to byte array and return
                return ms.ToArray();
            }
        }
        #endregion

        #region SaveTechnician Method
        private void SaveTechnician(string name, string position, byte[] image)
        {
            DatabaseConnection db = new DatabaseConnection();

            using (MySqlConnection connection = db.GetConnection())
            {
                connection.Open();

                // Insert the technician record and retrieve the ID
                string query = "INSERT INTO technician (name, position, image) VALUES (@name, @position, @image); SELECT LAST_INSERT_ID();";

                long technicianId;

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@position", MySqlDbType.VarChar).Value = position;
                    command.Parameters.Add("@image", MySqlDbType.Blob).Value = image;

                    // Execute the query and retrieve the ID of the newly inserted technician
                    technicianId = Convert.ToInt64(command.ExecuteScalar());
                }

                // Get the username from lblUsername
                string username = lblUsername.Text;

                // Log the activity to the audit trail
                LogAuditTrail(username, "Added New Technician", technicianId.ToString());
            }
        }
        #endregion

        private void btnAddTechSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBoxEmpTech.Tag.ToString() == "add_image")
                {
                    MessageBox.Show("Please upload a valid image before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Exit the save function to prevent saving the default image
                }

                string name = txtTechname.Text;
                string position = txtTechposition.Text;

                // Get the image from the PictureBox
                Image techImage = pictureBoxEmpTech.Image;

                // Convert image to byte array with specified format
                byte[] imageBytes = ImageToByteArray(techImage, ImageFormatType.Png);

                // Save the technician
                SaveTechnician(name, position, imageBytes);

                // Add technician panel to the flow layout panel
                AddTechnicianPanel(name, position, techImage);

                // Display message box with an information icon
                MessageBox.Show("Technician saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pnAddTech.Visible = false;
                pnListTech.Visible = true;

                FillGridViewTicketTech();
                FillComboBoxTicketTech();
                controlsShowEmp();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optionally, you can log the exception details for troubleshooting
                Console.WriteLine($"Error in btnAddTechSave_Click: {ex.ToString()}");
            }
        }

        #region Technician Panel Handling

        private void AddTechnicianPanel(string technicianName, string position, Image image)
        {
            // Create a new flow layout panel
            Panel technicianPanel = new Panel();
            technicianPanel.Size = new Size(240, 240); // Set panel size to 240x240 pixels
            //technicianPanel.FlowDirection = FlowDirection.TopDown; // Set flow direction to TopDown
            technicianPanel.BorderStyle = BorderStyle.FixedSingle; // Optional: Add border for visual clarity
            technicianPanel.BackColor = ColorTranslator.FromHtml("#FFFFFF"); // Set background color to white (#FFFFFF)
            technicianPanel.ForeColor = ColorTranslator.FromHtml("#004D80"); // Set foreground color to a shade of blue (#004D80)
            technicianPanel.Padding = new Padding(15); // Add 15 pixels of padding around the panel
            technicianPanel.Margin = new Padding(6); // Add 10 pixels of margin between panels

            // Create and add the picture box
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = MakeImageTransparent(image); // Call method to make image transparent
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Size = new Size(70, 70); // Set image size to 70x70 pixels
            pictureBox.Padding = new Padding(10); // Add 10 pixels of padding around the picture box
            pictureBox.Location = new System.Drawing.Point(85, 20);
            technicianPanel.Controls.Add(pictureBox);

            // Create and add the name label
            System.Windows.Forms.Label nameLabel = new System.Windows.Forms.Label();
            nameLabel.Text = technicianName;
            nameLabel.AutoSize = true;
            nameLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally
            nameLabel.Padding = new Padding(5); // Add 5 pixels of padding around the label
            nameLabel.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold); // Set font to Arial, size 15, bold
            nameLabel.Height = 30; // Set height to 30 pixels
            nameLabel.Location = new System.Drawing.Point(85, 100);
            technicianPanel.Controls.Add(nameLabel);

            // Create and add the position label
            System.Windows.Forms.Label positionLabel = new System.Windows.Forms.Label();
            positionLabel.Text = position;
            positionLabel.AutoSize = true;
            positionLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally
            positionLabel.Padding = new Padding(5); // Add 5 pixels of padding around the label
            positionLabel.Font = new System.Drawing.Font("Arial", 10.5f); // Set font to Arial, size 10.5
            positionLabel.Height = 21; // Set height to 21 pixels
            positionLabel.Location = new System.Drawing.Point(85, 140);
            technicianPanel.Controls.Add(positionLabel);

            // Add label for "Number of Tickets Done"
            System.Windows.Forms.Label ticketsDoneLabel = new System.Windows.Forms.Label();
            ticketsDoneLabel.Text = "Number of Tickets Done:";
            ticketsDoneLabel.AutoSize = true;
            ticketsDoneLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally
            ticketsDoneLabel.Padding = new Padding(5); // Add 5 pixels of padding around the label
            ticketsDoneLabel.Font = new System.Drawing.Font("Arial", 10.5f); // Set font to Arial, size 10.5
            ticketsDoneLabel.Height = 21; // Set height to 21 pixels
            ticketsDoneLabel.Location = new System.Drawing.Point(85, 160);
            technicianPanel.Controls.Add(ticketsDoneLabel);

            // Create ticketsCountLabel
            ticketsTechCountLabel = new System.Windows.Forms.Label();
            ticketsTechCountLabel.Text = FetchNumberOfTicketsDoneTech(technicianName).ToString(); // Fetch initial count
            ticketsTechCountLabel.AutoSize = true;
            ticketsTechCountLabel.TextAlign = ContentAlignment.MiddleCenter;
            ticketsTechCountLabel.Padding = new Padding(5);
            ticketsTechCountLabel.Font = new System.Drawing.Font("Arial", 24f); // Set font to Arial, size 24
            ticketsTechCountLabel.Height = 21;
            ticketsTechCountLabel.Location = new System.Drawing.Point(85, 180);
            technicianPanel.Controls.Add(ticketsTechCountLabel);

            // Center all controls within the flow layout panel
            foreach (Control control in technicianPanel.Controls)
            {
                control.Anchor = AnchorStyles.Top;
                control.Margin = new Padding(0); // Set margin to 0 to control padding separately
                control.Location = new System.Drawing.Point((technicianPanel.Width - control.Width) / 2, control.Location.Y);
            }

            // Add the panel to the main flow layout panel (flpnTechList)
            flpnTechList.Controls.Add(technicianPanel);
        }

        private int FetchNumberOfTicketsDoneTech(string technicianName)
        {
            int ticketsDone = 0;

            string query = @"
                            SELECT COUNT(*) AS total_resolved_tickets
                            FROM ticket_technician tk
                            JOIN technician t ON tk.technician_id = t.technician_id
                            WHERE tk.resolved = 'yes' AND t.name = @TechnicianName
                            GROUP BY t.name;
                            ";

            // Assuming your DatabaseConnection class returns MySqlConnection
            using (MySqlConnection connection = new DatabaseConnection().GetConnection())
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@TechnicianName", technicianName);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ticketsDone = Convert.ToInt32(reader["total_resolved_tickets"]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Handle or log any exceptions
                    Console.WriteLine(ex.Message);
                }
            }

            return ticketsDone;
        }

        private Image MakeImageTransparent(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            bmp.MakeTransparent(); // Make image transparent
            return bmp;
        }

        #endregion

        private void controlsHideEmp()
        {
            pictureBox9.Hide();
            cmbSortEncoder.Hide();
            cmbSortTechnician.Hide();
            btnAddTech.Hide();
            button5.Hide();
        }

        private void controlsShowEmp()
        {
            pictureBox9.Show();
            cmbSortEncoder.Show();
            cmbSortTechnician.Show();
            btnAddTech.Show();
            button5.Show();
        }

        private void btnAddTech_Click(object sender, EventArgs e)
        {
            if (pnTechSub.Visible == true)
            {
                lblEmpTitle.Text = "Technicians Profile - Add";
                pnAddTech.Visible = true;
                pnListTech.Visible = false;
                controlsHideEmp();

                pictureBoxEmpTech.Image = Properties.Resources.add_image;
                txtTechname.Text = "";
                txtTechposition.Text = "";
            }
            else
            {
                lblEmpTitle.Text = "Encoders Profile - Add";
                pnEncoderSub.Visible = true;
                pnAddEncoder.Visible = true;
                pnListEncoder.Visible = false;
                controlsHideEmp();

                pictureBoxEmp.Image = Properties.Resources.add_image;
                txtEncodername.Text = "";
                txtEncoderposition.Text = "";
            }
        }

        private void cmbSortTechnician_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                // Check if the selected item is the default text "Sort"
                if (cmbSortTechnician.SelectedItem.ToString() == "Sort")
                {
                    return; // Do not apply any sorting
                }

                string sortBy = cmbSortTechnician.SelectedItem.ToString();
                List<Technician> technicians = RetrieveTechniciansFromDatabase();

                switch (sortBy)
                {
                    case "Name":
                        technicians = technicians.OrderBy(t => t.Name).ToList();
                        break;
                    default:
                        break;
                }

                // Clear existing panels
                flpnTechList.Controls.Clear();

                // Add sorted panels
                foreach (var technician in technicians)
                {
                    AddTechnicianPanel(technician.Name, technician.Position, technician.Image);
                }
            
                
        }

        private void cmbSortEncoder_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the selected item is the default text "Sort"
            if (cmbSortEncoder.SelectedItem.ToString() == "Sort")
            {
                return; // Do not apply any sorting
            }

            string sortBy = cmbSortEncoder.SelectedItem.ToString();
            List<Encoder> encoders = RetrieveEncodersFromDatabase();

            switch (sortBy)
            {
                case "Name":
                    encoders = encoders.OrderBy(t => t.Name).ToList();
                    break;
                default:
                    break;
            }

            // Clear existing panels
            flpnEncoderList.Controls.Clear();

            // Add sorted panels
            foreach (var encoder in encoders)
            {
                AddEncoderPanel(encoder.Name, encoder.Position, encoder.Image);
            }
        }

        private void pictureBoxEmp_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBoxEmp.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void pictureBoxEmpTech_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBoxEmpTech.Image = new Bitmap(openFileDialog.FileName);
                pictureBoxEmpTech.Tag = "user_image";
            }
        }

        #region SaveEncoder Method
        private void SaveEncoder(string name, string position, byte[] image)
        {
            DatabaseConnection db = new DatabaseConnection();

            using (MySqlConnection connection = db.GetConnection())
            {
                connection.Open();

                // Insert the encoder record
                string query = "INSERT INTO encoder (name, position, image) VALUES (@name, @position, @image); SELECT LAST_INSERT_ID();";

                long encoderId;

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@position", MySqlDbType.VarChar).Value = position;
                    command.Parameters.Add("@image", MySqlDbType.Blob).Value = image;

                    // Execute the query and retrieve the ID of the newly inserted encoder
                    encoderId = Convert.ToInt64(command.ExecuteScalar());
                }

                // Get the username from lblUsername
                string username = lblUsername.Text;

                // Log the activity to the audit trail
                LogAuditTrail(username, "Added New Encoder", encoderId.ToString());
            }
        }
        #endregion

        private void btnAddEncoderSave_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtEncodername.Text;
                string position = txtEncoderposition.Text;

                // Get the image from pictureBoxEmp
                Image encoderImage = pictureBoxEmp.Image;

                // Convert image to byte array with specified format
                byte[] imageBytes = ImageToByteArray(encoderImage, ImageFormatType.Png);

                // Save the encoder
                SaveEncoder(name, position, imageBytes);

                // Add encoder panel to the flow layout panel
                AddEncoderPanel(name, position, encoderImage);

                // Display message box with an information icon
                MessageBox.Show("Encoder saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pnAddEncoder.Visible = false;
                pnListEncoder.Visible = true;

                FillGridViewTicketEncoder();
                FillComboBoxTicketEncoder();
                controlsShowEmp();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optionally, you can log the exception details for troubleshooting
                Console.WriteLine($"Error in btnAddEncoderSave_Click: {ex.ToString()}");
            }
        }

        #region Encoder Panel Handling

        private void AddEncoderPanel(string encoderName, string position, Image image)
        {
            // Create a new flow layout panel
            Panel encoderPanel = new Panel();
            encoderPanel.Size = new Size(240, 240); // Set panel size to 240x240 pixels
                                                    //encoderPanel.FlowDirection = FlowDirection.TopDown; // Set flow direction to TopDown
            encoderPanel.BorderStyle = BorderStyle.FixedSingle; // Optional: Add border for visual clarity
            encoderPanel.BackColor = ColorTranslator.FromHtml("#FFFFFF"); // Set background color to white (#FFFFFF)
            encoderPanel.ForeColor = ColorTranslator.FromHtml("#004D80"); // Set foreground color to a shade of blue (#004D80)
            encoderPanel.Padding = new Padding(15); // Add 15 pixels of padding around the panel
            encoderPanel.Margin = new Padding(6); // Add 10 pixels of margin between panels

            // Create and add the picture box
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = MakeImageTransparent(image); // Call method to make image transparent
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Size = new Size(70, 70); // Set image size to 70x70 pixels
            pictureBox.Padding = new Padding(10); // Add 10 pixels of padding around the picture box
            pictureBox.Location = new System.Drawing.Point(85, 20);
            encoderPanel.Controls.Add(pictureBox);

            // Create and add the name label
            System.Windows.Forms.Label nameLabel = new System.Windows.Forms.Label();
            nameLabel.Text = encoderName;
            nameLabel.AutoSize = true;
            nameLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally
            nameLabel.Padding = new Padding(5); // Add 5 pixels of padding around the label
            nameLabel.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold); // Set font to Arial, size 15, bold
            nameLabel.Height = 30; // Set height to 30 pixels
            nameLabel.Location = new System.Drawing.Point(85, 100);
            encoderPanel.Controls.Add(nameLabel);

            // Create and add the position label
            System.Windows.Forms.Label positionLabel = new System.Windows.Forms.Label();
            positionLabel.Text = position;
            positionLabel.AutoSize = true;
            positionLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally
            positionLabel.Padding = new Padding(5); // Add 5 pixels of padding around the label
            positionLabel.Font = new System.Drawing.Font("Arial", 10.5f); // Set font to Arial, size 10.5
            positionLabel.Height = 21; // Set height to 21 pixels
            positionLabel.Location = new System.Drawing.Point(85, 140);
            encoderPanel.Controls.Add(positionLabel);

            // Add label for "Number of Tickets Done"
            System.Windows.Forms.Label ticketsDoneLabel = new System.Windows.Forms.Label();
            ticketsDoneLabel.Text = "Number of Tickets Done:";
            ticketsDoneLabel.AutoSize = true;
            ticketsDoneLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally
            ticketsDoneLabel.Padding = new Padding(5); // Add 5 pixels of padding around the label
            ticketsDoneLabel.Font = new System.Drawing.Font("Arial", 10.5f); // Set font to Arial, size 10.5
            ticketsDoneLabel.Height = 21; // Set height to 21 pixels
            ticketsDoneLabel.Location = new System.Drawing.Point(85, 160);
            encoderPanel.Controls.Add(ticketsDoneLabel);

            // Create ticketsEncoderCountLabel
            ticketsEncoderCountLabel = new System.Windows.Forms.Label();
            ticketsEncoderCountLabel.Text = FetchNumberOfTicketsDoneEncoder(encoderName).ToString(); // Fetch initial count
            ticketsEncoderCountLabel.AutoSize = true;
            ticketsEncoderCountLabel.TextAlign = ContentAlignment.MiddleCenter;
            ticketsEncoderCountLabel.Padding = new Padding(5);
            ticketsEncoderCountLabel.Font = new System.Drawing.Font("Arial", 24f); // Set font to Arial, size 24
            ticketsEncoderCountLabel.Height = 21;
            ticketsEncoderCountLabel.Location = new System.Drawing.Point(85, 180);
            encoderPanel.Controls.Add(ticketsEncoderCountLabel);

            // Center all controls within the flow layout panel
            foreach (Control control in encoderPanel.Controls)
            {
                control.Anchor = AnchorStyles.Top;
                control.Margin = new Padding(0); // Set margin to 0 to control padding separately
                control.Location = new System.Drawing.Point((encoderPanel.Width - control.Width) / 2, control.Location.Y);
            }


            // Add the panel to the main flow layout panel (flpnTechList)
            flpnEncoderList.Controls.Add(encoderPanel);
        }


        private int FetchNumberOfTicketsDoneEncoder(string encoderName)
        {
            int ticketsDone = 0;

            string query = @"
                            SELECT COUNT(*) AS total_resolved_tickets
                            FROM ticket_encoder tk
                            JOIN encoder e ON tk.encoder_id = e.encoder_id
                            WHERE tk.resolved = 'yes' AND e.name = @EncoderName
                            GROUP BY e.name;
                            ";

            // Assuming your DatabaseConnection class returns MySqlConnection
            using (MySqlConnection connection = new DatabaseConnection().GetConnection())
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EncoderName", encoderName);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ticketsDone = Convert.ToInt32(reader["total_resolved_tickets"]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Handle or log any exceptions
                    Console.WriteLine(ex.Message);
                }
            }

            return ticketsDone;
        }


        #endregion

        public void FillGridViewTicketEncoder(string encoderName = null, string resolvedStatus = null, DateTime? selectedDateEncoder = null, string sortOrder = null)
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string query = TicketQueryUtility.GetTicketEncoderQuery();
                    List<string> conditionsEncoder = new List<string>();

                    // Check for valid encoderName
                    if (encoderName != null && encoderName != "Encoder")
                    {
                        conditionsEncoder.Add("enc.name = @encoderName");
                    }

                    // Check for resolved status
                    if (!string.IsNullOrEmpty(resolvedStatus))
                    {
                        conditionsEncoder.Add("te.resolved = @resolvedStatus");
                    }

                    // Check for selected date
                    if (selectedDateEncoder.HasValue)
                    {
                        conditionsEncoder.Add("DATE(te.date) = @selectedDateEncoder");
                    }

                    // Append conditions to the query
                    if (conditionsEncoder.Count > 0)
                    {
                        query += " WHERE " + string.Join(" AND ", conditionsEncoder);
                    }

                    // Append sorting order to the query
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortOrder)
                        {
                            case "A-Z":
                                query += " ORDER BY enc.name ASC";
                                break;
                            case "Z-A":
                                query += " ORDER BY enc.name DESC";
                                break;
                        }
                    }

                    // Log the final query for debugging
                    Console.WriteLine("Final Query: " + query);

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Add parameters to the command
                    if (encoderName != null && encoderName != "Encoder")
                    {
                        cmd.Parameters.AddWithValue("@encoderName", encoderName);
                    }
                    if (!string.IsNullOrEmpty(resolvedStatus))
                    {
                        cmd.Parameters.AddWithValue("@resolvedStatus", resolvedStatus);
                    }
                    if (selectedDateEncoder.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@selectedDateEncoder", selectedDateEncoder.Value.ToString("yyyy-MM-dd"));
                    }

                    // Execute the query and fill the DataTable
                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    sda.Fill(dt);

                    // Set the DataGridView data source
                    dgvTicketEncoder.DataSource = dt;

                    // Center-align column headers
                    foreach (DataGridViewColumn column in dgvTicketEncoder.Columns)
                    {
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    // Handle mouse events and cell formatting
                    dgvTicketEncoder.CellMouseEnter += (sender, e) =>
                    {
                        if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && (dgvTicketEncoder.Columns[e.ColumnIndex] is DataGridViewImageColumn))
                        {
                            dgvTicketEncoder.Cursor = Cursors.Hand;
                        }
                    };

                    dgvTicketEncoder.CellMouseLeave += (sender, e) =>
                    {
                        dgvTicketEncoder.Cursor = Cursors.Default;
                    };

                    dgvTicketEncoder.CellFormatting += (sender, e) =>
                    {
                        if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dgvTicketEncoder.Columns[e.ColumnIndex].Name == "dgvcEncoder")
                        {
                            object cellValue = e.Value;

                            if (cellValue != DBNull.Value && cellValue != null)
                            {
                                if (cellValue.GetType() == typeof(byte[]))
                                {
                                    byte[] imgBytes = (byte[])cellValue;
                                    Image img = Image.FromStream(new MemoryStream(imgBytes));
                                    e.Value = img.GetThumbnailImage(30, 30, null, IntPtr.Zero);
                                }
                                else if (cellValue.GetType() == typeof(Bitmap))
                                {
                                    Bitmap bitmap = (Bitmap)cellValue;
                                    e.Value = bitmap.GetThumbnailImage(30, 30, null, IntPtr.Zero);
                                }
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTicketEncoder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is the "Edit" button
            if (e.ColumnIndex == dgvTicketEncoder.Columns["EditEncoder"].Index && e.RowIndex >= 0)
            {
                // Get the registry_no from the selected row
                string idNo = dgvTicketEncoder.Rows[e.RowIndex].Cells["dgvcIDEncoder"].Value.ToString();

                // Retrieve data from the database based on registry_no
                PopulateEditFieldsEncoder(idNo);
                lblTicketTitle.Text = "Tickets   -  Edit";
                label10.Text = "Edit Ticket";
                pnAddTicketEncoder.Visible = true;
                pnTicketListEncoder.Visible = false;
                btnTicketEncoderUpdateSave.BringToFront();
            }

        }

        private void PopulateEditFieldsEncoder(string idNo)
        {
            #region Database Connection and Query

            using (MySqlConnection connection = new DatabaseConnection().GetConnection())
            {
                // Prepare SQL command
                string query = TicketQueryUtility.GetTicketEncoderDetailsQuery();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@idNo", idNo);

                try
                {
                    // Open the connection
                    connection.Open();
                    // Execute the command
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Populate edit fields with data from the reader
                            txtTicketEncoderID.Text = reader["id"].ToString(); // Use the alias 'TICKET #'
                            dtpTicketEncoder.Value = Convert.ToDateTime(reader["DATE"]);
                            cmbTicketEncoder.Text = reader["name"].ToString();
                            cmbOfficeWorkTicket.Text = reader["office"].ToString();

                            // Set CheckBoxes based on the resolved column
                            bool resolvedEncoder = reader["resolved"].ToString().ToUpper() == "YES";
                            cbYesEncoder.Checked = resolvedEncoder;
                            cbNoEncoder.Checked = !resolvedEncoder;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            #endregion Database Connection and Query
        }

        private void dgvTicketEncoder_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the cell
                DataGridViewCell cell = dgvTicketEncoder.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Check if the cell is in the image column
                if (cell.OwningColumn.Name == "dgvcEncoder")
                {
                    // Get the image name from the corresponding row
                    string imageName = dgvTicketEncoder.Rows[e.RowIndex].Cells["dgvcNameEncoder"].Value.ToString();

                    // Set the tooltip text
                    dgvTicketEncoder.ShowCellToolTips = true;
                    cell.ToolTipText = imageName;
                }
            }
        }

        private void cbYesEncoder_CheckedChanged(object sender, EventArgs e)
        {
            if (cbYesEncoder.Checked == true)
            {
                cbNoEncoder.Checked = false;
            }
        }

        private void cbNoEncoder_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNoEncoder.Checked == true)
            {
                cbYesEncoder.Checked = false;
            }
        }

        private void btnTicketEncoderSave_Click(object sender, EventArgs e)
        {
            if (cmbTicketEncoder.Text == "Select")
            {
                MessageBox.Show("Please select a Encoder.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!cbYesEncoder.Checked && !cbNoEncoder.Checked)
            {
                MessageBox.Show("Please check either 'Yes' or 'No'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string resolvedValue = cbYesEncoder.Checked ? "yes" : "no"; // Determine 'yes' or 'no' based on checkbox state
                    int encoderId = GetEncoderId(cmbTicketEncoder.Text.Trim());

                    string query = "INSERT INTO ticket_encoder (date, encoder_id, office, resolved) " +
                                   "VALUES (@Date, @EncoderId, @office, @Resolved)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", dtpTicketEncoder.Value);
                        cmd.Parameters.AddWithValue("@EncoderId", encoderId);
                        cmd.Parameters.AddWithValue("@office", cmbOfficeWorkTicket.Text.Trim());
                        cmd.Parameters.AddWithValue("@Resolved", resolvedValue); // Use resolvedValue determined above

                        cmd.ExecuteNonQuery();
                    }

                    #region
                    // Get the username from lblUsername
                    string username = lblUsername.Text;

                    // Log the activity to the audit trail
                    LogAuditTrail(username, "Created New Ticket", encoderId.ToString());
                    #endregion

                    // After inserting ticket, update ticketsEncoderCountLabel
                    string encoderName = cmbTicketEncoder.Text.Trim(); // Assuming this gets the technician's name

                    // Update ticketsEncoderCountLabel for the technician
                    int ticketsDone = FetchNumberOfTicketsDoneEncoder(encoderName);
                    ticketsEncoderCountLabel.Text = ticketsDone.ToString();

                    MessageBox.Show("Ticket saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillGridViewTicketEncoder();

                    FillComboBoxTicketEncoder();
                    FillDataGridViewRecentActivity();
                    FillGridViewFilesIn();
                    FillGridViewFilesOut();

                    FillTotalTicketResolved();
                    FillTotalTicket();

                    //UserControl reportTicketStatus1
                    reportTicketStatus1.FillTotalTicketResolved();
                    reportTicketStatus1.FillTotalTicketNotResolved();

                    //UserControl reportTicketVolume1
                    reportTicketVolume1.FillDataGridViewTicketReport();
                    reportTicketVolume1.FillTotalTicket();

                    pnAddTicketEncoder.Visible = false;
                    pnTicketListEncoder.Visible = true;
                    lblTicketTitle.Text = "Tickets";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnTicketEncoderUpdateSave_Click(object sender, EventArgs e)
        {
            if (cmbTicketEncoder.Text == "Select")
            {
                MessageBox.Show("Please select a encoder.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!cbYesEncoder.Checked && !cbNoEncoder.Checked)
            {
                MessageBox.Show("Please check either 'Yes' or 'No'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string resolvedValue = cbYesEncoder.Checked ? "yes" : "no"; // Determine 'yes' or 'no' based on checkbox state
                    int encoderId = GetEncoderId(cmbTicketEncoder.Text.Trim());

                    string query = "UPDATE ticket_encoder SET date = @Date, " +
                                   "encoder_id = @EncoderId, office = @office, " +
                                   "resolved = @Resolved WHERE id = @TicketId"; // Assuming 'ticket_id' is the primary key

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", dtpTicketEncoder.Value);
                        cmd.Parameters.AddWithValue("@EncoderId", encoderId);
                        cmd.Parameters.AddWithValue("@office", cmbOfficeWorkTicket.Text.Trim());
                        cmd.Parameters.AddWithValue("@Resolved", resolvedValue); // Use resolvedValue determined above
                        cmd.Parameters.AddWithValue("@TicketId", txtTicketEncoderID.Text.Trim()); // Add the ID of the ticket being updated

                        cmd.ExecuteNonQuery();
                    }

                    #region
                    // Get the username from lblUsername
                    string username = lblUsername.Text;

                    // Log the activity to the audit trail
                    LogAuditTrail(username, "Updated Record Ticket", encoderId.ToString());
                    #endregion

                    // After inserting ticket, update ticketsEncoderCountLabel
                    string encoderName = cmbTicketEncoder.Text.Trim(); // Assuming this gets the technician's name

                    // Update ticketsEncoderCountLabel for the technician
                    int ticketsDone = FetchNumberOfTicketsDoneEncoder(encoderName);
                    ticketsEncoderCountLabel.Text = ticketsDone.ToString();

                    MessageBox.Show("Ticket updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillGridViewTicketEncoder();

                    FillComboBoxTicketEncoder();
                    FillDataGridViewRecentActivity();
                    FillGridViewFilesIn();
                    FillGridViewFilesOut();

                    FillTotalTicketResolved();
                    FillTotalTicket();

                    //UserControl reportTicketStatus1
                    reportTicketStatus1.FillTotalTicketResolved();
                    reportTicketStatus1.FillTotalTicketNotResolved();

                    //UserControl reportTicketVolume1
                    reportTicketVolume1.FillDataGridViewTicketReport();
                    reportTicketVolume1.FillTotalTicket();

                    pnAddTicketEncoder.Visible = false;
                    pnTicketListEncoder.Visible = true;
                    lblTicketTitle.Text = "Tickets";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtSearchDash_TextChange(object sender, EventArgs e)
        {
            try
            {
                // Cast the DataGridView's DataSource to a DataTable
                System.Data.DataTable dt = (System.Data.DataTable)dgvRecentActivity.DataSource;

                if (dt != null)
                {
                    // Initialize a list to store filter conditions
                    List<string> filterConditions = new List<string>();

                    // Iterate through each column in the DataTable
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.DataType == typeof(string))
                        {
                            // Add a filter condition for string columns
                            filterConditions.Add($"[{column.ColumnName}] LIKE '%{txtSearchDash.Text}%'");
                        }
                        else if (column.DataType == typeof(int) || column.DataType == typeof(double) || column.DataType == typeof(decimal))
                        {
                            // Try parsing the search text to a number and add a filter condition for numeric columns
                            if (decimal.TryParse(txtSearchDash.Text, out decimal searchNumber))
                            {
                                filterConditions.Add($"CONVERT([{column.ColumnName}], 'System.String') LIKE '%{txtSearchDash.Text}%'");
                            }
                        }
                    }

                    // Combine filter conditions into a single expression
                    string filterExpression = string.Join(" OR ", filterConditions);

                    // Apply the filter to the DataTable's DefaultView
                    dt.DefaultView.RowFilter = filterExpression;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while filtering: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Report Dash
        
        private void ResetButtonColorsRepo()
        {
            // Reset all buttons to default colors
            btnTicketVolume.BackColor = defaultBackColor5;
            btnTicketVolume.ForeColor = defaultForeColor5;

            btnTicketStatus.BackColor = defaultBackColor5;
            btnTicketStatus.ForeColor = defaultForeColor5;

            btnAuditTrail.BackColor = defaultBackColor5;
            btnAuditTrail.ForeColor = defaultForeColor5;


        }

        private void ControHideRepo()
        {
            pbCalendarAudit.Visible = false;
            pictureBox8.Visible = false;
            cmbSortAudit.Visible = false;
        }

        private void ControShowRepo()
        {
            pbCalendarAudit.Visible = true;
            pictureBox8.Visible = true;
            cmbSortAudit.Visible = true;
        }

        public ComboBox cmbSelect { get; set; }


        private void btnTicketVolume_Click(object sender, EventArgs e)
        {
            // Reset all buttons to default
            ResetButtonColorsRepo();

            // Hide controls
            ControHideRepo();

            // Change button 1 colors
            btnTicketVolume.BackColor = defaultBackColor4;
            btnTicketVolume.ForeColor = defaultForeColor4;

            reportTicketVolume1.Visible = true;
            reportTicketVolume1.FillDataGridViewTicketReport();
            reportTicketVolume1.btnNormal.PerformClick();
            reportTicketStatus1.Visible = false;
            audit_Trail1.Visible = false;

            btnExpoPdfVol.Visible = true;
            btnExpoPdfAudit.Visible = false;
        }

        private void btnTicketStatus_Click(object sender, EventArgs e)
        {
            // Reset all buttons to default
            ResetButtonColorsRepo();

            // Hide controls
            ControHideRepo();

            // Change button 2 colors
            btnTicketStatus.BackColor = defaultBackColor4;
            btnTicketStatus.ForeColor = defaultForeColor4;

            audit_Trail1.Visible = false;
            reportTicketVolume1.Visible = false;
            reportTicketStatus1.Visible = true;

            btnExpoPdfVol.Visible = false;
            btnExpoPdfAudit.Visible = false;
        }

        private void btnAuditTrail_Click(object sender, EventArgs e)
        {
            // Reset all buttons to default
            ResetButtonColorsRepo();

            // Show Controls
            ControShowRepo();

            // Change button 3 colors
            btnAuditTrail.BackColor = defaultBackColor4;
            btnAuditTrail.ForeColor = defaultForeColor4;

            reportTicketStatus1.Visible = false;
            reportTicketVolume1.Visible = false;
            audit_Trail1.Visible = true;
            cmbSortAudit.Text = "Sort";
            cmbSortAudit.SelectedIndex = 0;
            audit_Trail1.FillGridViewAudit();

            btnExpoPdfVol.Visible = false;
            btnExpoPdfAudit.Visible = true;
        }

        private void pbCalendarAudit_Click(object sender, EventArgs e)
        {
            using (MonthCalendar mcAudit = new MonthCalendar())
            {
                Form calendarFormFilesIn = new Form
                {
                    Text = "Select Date",
                    FormBorderStyle = FormBorderStyle.FixedToolWindow,
                    StartPosition = FormStartPosition.CenterParent,
                    AutoSize = false,
                    Size = new Size(250, 220) // Adjust size as needed
                };

                mcAudit.MaxSelectionCount = 1;
                mcAudit.Dock = DockStyle.Fill;
                mcAudit.DateSelected += (s, ev) => calendarFormFilesIn.Close();

                calendarFormFilesIn.Controls.Add(mcAudit);
                calendarFormFilesIn.ShowDialog();

                DateTime selectedDateAudit = mcAudit.SelectionStart;
                audit_Trail1.FillGridViewAudit(selectedDateAudit);
            }
        }

        //File Dash

        private void tsmiFilesIn_Click(object sender, EventArgs e)
        {
            pnFilesInSub.BringToFront();
            pnFilesIn.Visible = true;
            pnFilesInAdd.Visible = false;

            pbFilesInCalendar.Visible = true;
            cmbFilesInSort.Visible = true;
            pbFilesOutCalendar.Visible = false;
            cmbFilesOutSort.Visible = false;

            btnExpoPdfFilesIn.Visible = true;
            btnExpoPdfFilesOut.Visible = false;

            cmbFilesInSort.SelectedIndex = 0;
            cmbFilesInSort.Text = "Sort";
            txtSearchFilesIn.BringToFront();
            FillGridViewFilesIn();
        }

        private void tsmiFilesOut_Click(object sender, EventArgs e)
        {
            pnFilesOutSub.BringToFront();
            pnFilesOut.Visible = true;
            pnFilesOutAdd.Visible = false;

            pbFilesOutCalendar.Visible = true;
            cmbFilesOutSort.Visible = true;
            pbFilesInCalendar.Visible = false;
            cmbFilesInSort.Visible = false;

            btnExpoPdfFilesIn.Visible = false;
            btnExpoPdfFilesOut.Visible = true;

            cmbFilesOutSort.SelectedIndex = 0;
            cmbFilesOutSort.Text = "Sort";
            txtSearchFilesOut.BringToFront();
            FillGridViewFilesOut();
        }

        private void txtSearchFilesIn_TextChange(object sender, EventArgs e)
        {
            try
            {
                // Cast the DataGridView's DataSource to a DataTable
                System.Data.DataTable dt = (System.Data.DataTable)dgvFilesIn.DataSource;

                if (dt != null)
                {
                    // Initialize a list to store filter conditions
                    List<string> filterConditions = new List<string>();

                    // Iterate through each column in the DataTable
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.DataType == typeof(string))
                        {
                            // Add a filter condition for string columns
                            filterConditions.Add($"[{column.ColumnName}] LIKE '%{txtSearchFilesIn.Text}%'");
                        }
                        else if (column.DataType == typeof(int) || column.DataType == typeof(double) || column.DataType == typeof(decimal))
                        {
                            // Try parsing the search text to a number and add a filter condition for numeric columns
                            if (decimal.TryParse(txtSearchFilesIn.Text, out decimal searchNumber))
                            {
                                filterConditions.Add($"CONVERT([{column.ColumnName}], 'System.String') LIKE '%{txtSearchFilesIn.Text}%'");
                            }
                        }
                    }

                    // Combine filter conditions into a single expression
                    string filterExpression = string.Join(" OR ", filterConditions);

                    // Apply the filter to the DataTable's DefaultView
                    dt.DefaultView.RowFilter = filterExpression;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while filtering: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearchFilesOut_TextChange(object sender, EventArgs e)
        {
            try
            {
                // Cast the DataGridView's DataSource to a DataTable
                System.Data.DataTable dt = (System.Data.DataTable)dgvFilesOut.DataSource;

                if (dt != null)
                {
                    // Initialize a list to store filter conditions
                    List<string> filterConditions = new List<string>();

                    // Iterate through each column in the DataTable
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.DataType == typeof(string))
                        {
                            // Add a filter condition for string columns
                            filterConditions.Add($"[{column.ColumnName}] LIKE '%{txtSearchFilesOut.Text}%'");
                        }
                        else if (column.DataType == typeof(int) || column.DataType == typeof(double) || column.DataType == typeof(decimal))
                        {
                            // Try parsing the search text to a number and add a filter condition for numeric columns
                            if (decimal.TryParse(txtSearchFilesOut.Text, out decimal searchNumber))
                            {
                                filterConditions.Add($"CONVERT([{column.ColumnName}], 'System.String') LIKE '%{txtSearchFilesOut.Text}%'");
                            }
                        }
                    }

                    // Combine filter conditions into a single expression
                    string filterExpression = string.Join(" OR ", filterConditions);

                    // Apply the filter to the DataTable's DefaultView
                    dt.DefaultView.RowFilter = filterExpression;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while filtering: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbFilesInSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortOrder = cmbFilesInSort.SelectedItem?.ToString();
            FillGridViewFilesIn(sortOrderFilesIn: sortOrder);
        }

        private void cmbFilesOutSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortOrder = cmbFilesOutSort.SelectedItem?.ToString(); // Update ComboBox name
            FillGridViewFilesOut(sortOrderFilesOut: sortOrder);
        }

        private void pbFilesInCalendar_Click(object sender, EventArgs e)
        {
            using (MonthCalendar mcFilesIn = new MonthCalendar())
            {
                Form calendarFormFilesIn = new Form
                {
                    Text = "Select Date",
                    FormBorderStyle = FormBorderStyle.FixedToolWindow,
                    StartPosition = FormStartPosition.CenterParent,
                    AutoSize = false,
                    Size = new Size(250, 220) // Adjust size as needed
                };

                mcFilesIn.MaxSelectionCount = 1;
                mcFilesIn.Dock = DockStyle.Fill;
                mcFilesIn.DateSelected += (s, ev) => calendarFormFilesIn.Close();

                calendarFormFilesIn.Controls.Add(mcFilesIn);
                calendarFormFilesIn.ShowDialog();

                DateTime selectedDateFilesIn = mcFilesIn.SelectionStart;
                FillGridViewFilesIn(selectedDateFilesIn);
            }
        }

        private void pbFilesOutCalendar_Click(object sender, EventArgs e)
        {
            using (MonthCalendar mcFilesOut = new MonthCalendar())
            {
                Form calendarFormFilesOut = new Form
                {
                    Text = "Select Date",
                    FormBorderStyle = FormBorderStyle.FixedToolWindow,
                    StartPosition = FormStartPosition.CenterParent,
                    AutoSize = false,
                    Size = new Size(250, 220) // Adjust size as needed
                };

                mcFilesOut.MaxSelectionCount = 1;
                mcFilesOut.Dock = DockStyle.Fill;
                mcFilesOut.DateSelected += (s, ev) => calendarFormFilesOut.Close();

                calendarFormFilesOut.Controls.Add(mcFilesOut);
                calendarFormFilesOut.ShowDialog();

                DateTime selectedDateFilesOut = mcFilesOut.SelectionStart;
                FillGridViewFilesOut(selectedDateFilesOut);
            }
        }

        public void FillGridViewFilesIn(DateTime? selectedDateFilesIn = null, string sortOrderFilesIn = null)
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string query = FilesQueryUtility.GetFilesInQuery();

                    if (selectedDateFilesIn.HasValue)
                    {
                        query += " WHERE DATE(fi.date) = @selectedDate";
                    }

                    // Apply sorting based on the selected order
                    if (!string.IsNullOrEmpty(sortOrderFilesIn))
                    {
                        if (sortOrderFilesIn == "A-Z")
                        {
                            query += " ORDER BY fi.department ASC"; // Replace 'fi.technician' with your column name
                        }
                        else if (sortOrderFilesIn == "Z-A")
                        {
                            query += " ORDER BY fi.department DESC"; // Replace 'fi.technician' with your column name
                        }
                    }

                    query += ";"; // Ensure query ends with a semicolon

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (selectedDateFilesIn.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@selectedDate", selectedDateFilesIn.Value.ToString("yyyy-MM-dd"));
                    }

                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    sda.Fill(dt);

                    dgvFilesIn.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void FillGridViewFilesOut(DateTime? selectedDateFilesOut = null, string sortOrderFilesOut = null)
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string query = FilesQueryUtility.GetFilesOutQuery(); // Update the query method if necessary

                    if (selectedDateFilesOut.HasValue)
                    {
                        query += " WHERE DATE(fo.date) = @selectedDate";
                    }

                    // Apply sorting based on the selected order
                    if (!string.IsNullOrEmpty(sortOrderFilesOut))
                    {
                        if (sortOrderFilesOut == "A-Z")
                        {
                            query += " ORDER BY fo.department ASC"; // Replace 'fi.technician' with your column name
                        }
                        else if (sortOrderFilesOut == "Z-A")
                        {
                            query += " ORDER BY fo.department DESC"; // Replace 'fi.technician' with your column name
                        }
                    }

                    query += ";"; // Ensure query ends with a semicolon

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (selectedDateFilesOut.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@selectedDate", selectedDateFilesOut.Value.ToString("yyyy-MM-dd"));
                    }

                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    sda.Fill(dt);

                    dgvFilesOut.DataSource = dt; // Update the DataGridView name
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            pnFilesInAdd.Visible = true;
            pnFilesOutAdd.Visible = true;

            pnFilesOut.Visible = false;
            pnFilesIn.Visible = false;
        }
        
        private void btnFilesInDone_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO filesin (date, department, items, description, recieved) " +
                        "VALUES (@Date, @Department, @Items, @Description, @Recieved); SELECT LAST_INSERT_ID();";

                    long docs_in_info_id; // variable to store the last inserted ID

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", dtpFilesIn.Value);
                        cmd.Parameters.AddWithValue("@Department", cmbFInDepartment.Text.Trim());
                        cmd.Parameters.AddWithValue("@Items", txtFInItems.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", txtFInDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@Recieved", txtFInReceive.Text.Trim());
                        
                        // Execute the query and get the last inserted ID
                        docs_in_info_id = Convert.ToInt64(cmd.ExecuteScalar());
                    }

                    // Insert filename and image data into lb_document_image table
                    Image image = pbFilesIn.Image;
                    if (image != null)
                    {
                        // You can decide which format to use here. For example, let's assume you want to use JPEG format.
                        ImageFormatType selectedFormat = ImageFormatType.Jpeg;
                        byte[] imageData = ImageToByteArray(image, selectedFormat);

                        string insertQuery = "INSERT INTO filesinimages (`docs_in_info_id`, `filename`, `image`) VALUES (@docs_in_info_id, @filename, @image)";
                        using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@docs_in_info_id", docs_in_info_id);
                            cmd.Parameters.AddWithValue("@filename", txtFileInname.Text.Trim()); // Provide filename
                            cmd.Parameters.AddWithValue("@image", imageData);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Display error message if no image is selected
                        MessageBox.Show("No image selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Get the username from lblUsername
                    string username = lblUsername.Text;

                    // Log the activity to the audit trail
                    LogAuditTrail(username, "Files In Added", docs_in_info_id.ToString());

                    MessageBox.Show("Files saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillGridViewFilesIn();
                    pnFilesIn.Visible = true;
                    pnFilesInAdd.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChooseFileIn_Click(object sender, EventArgs e)
        {
            string selectedFilePath = ""; // Declare selectedFilePath inside btnChooseFile_Click
            byte[] imageByteArray = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\path\to\your\scanned_documents"; // Adjust the path accordingly
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg; *.jpeg; *.png; *.gif";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    selectedFilePath = openFileDialog.FileName;
                    imageByteArray = File.ReadAllBytes(selectedFilePath);

                    // Display image in PictureBox
                    pbFilesIn.ImageLocation = selectedFilePath;

                    // Display filename in TextBox
                    txtFileInname.Text = Path.GetFileName(selectedFilePath);

                    // Ask user if they want to rename the image
                    DialogResult result = MessageBox.Show("Do you want to rename the image?", "Rename Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Implement renaming logic
                        string newFileName = Path.Combine(Path.GetDirectoryName(selectedFilePath), "female.jpg");
                        File.Move(selectedFilePath, newFileName);

                        // Update PictureBox and TextBox with the new filename
                        pbFilesIn.ImageLocation = newFileName;
                        txtFileInname.Text = Path.GetFileName(newFileName);
                    }
                    else
                    {
                        // User chose not to rename the image
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                finally
                {
                    // Clean up if needed
                    if (imageByteArray != null)
                    {
                        Array.Clear(imageByteArray, 0, imageByteArray.Length);
                    }
                }
            }
        }

        
        private void btnChooseFileOut_Click(object sender, EventArgs e)
        {
            string selectedFilePath = ""; // Declare selectedFilePath inside btnChooseFile_Click
            byte[] imageByteArray = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\path\to\your\scanned_documents"; // Adjust the path accordingly
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg; *.jpeg; *.png; *.gif";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    selectedFilePath = openFileDialog.FileName;
                    imageByteArray = File.ReadAllBytes(selectedFilePath);

                    // Display image in PictureBox
                    pbFilesOut.ImageLocation = selectedFilePath;

                    // Display filename in TextBox
                    txtFileOutname.Text = Path.GetFileName(selectedFilePath);

                    // Ask user if they want to rename the image
                    DialogResult result = MessageBox.Show("Do you want to rename the image?", "Rename Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Implement renaming logic
                        string newFileName = Path.Combine(Path.GetDirectoryName(selectedFilePath), "newfilename.jpg");
                        File.Move(selectedFilePath, newFileName);

                        // Update PictureBox and TextBox with the new filename
                        pbFilesOut.ImageLocation = newFileName;
                        txtFileOutname.Text = Path.GetFileName(newFileName);
                    }
                    else
                    {
                        // User chose not to rename the image
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                finally
                {
                    // Clean up if needed
                    if (imageByteArray != null)
                    {
                        Array.Clear(imageByteArray, 0, imageByteArray.Length);
                    }
                }
            }
        }

        // Method to log the audit trail
        public static void LogAuditTrail(string username, string activity, string code)
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO audit_trail (log_date, log_time, username, activity, code) " +
                                   "VALUES (CURDATE(), CURTIME(), @Username, @Activity, @Code)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Activity", activity);
                        cmd.Parameters.AddWithValue("@Code", code);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error logging audit trail: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event handler for the Files Out Done button
        private void btnFilesOutDone_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO filesout (date, department, items, description) " +
                        "VALUES (@Date, @Department, @Items, @Description); SELECT LAST_INSERT_ID();";

                    long docs_out_info_id; // variable to store the last inserted ID

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", dtpFilesOut.Value);
                        cmd.Parameters.AddWithValue("@Department", cmbFOutDepartment.Text.Trim());
                        cmd.Parameters.AddWithValue("@Items", txtFOutItems.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", txtFOutDescription.Text.Trim());

                        // Execute the query and get the last inserted ID
                        docs_out_info_id = Convert.ToInt64(cmd.ExecuteScalar());
                    }

                    // Insert filename and image data into filesoutimages table
                    Image image = pbFilesOut.Image;
                    if (image != null)
                    {
                        // You can decide which format to use here. For example, let's assume you want to use JPEG format.
                        ImageFormatType selectedFormat = ImageFormatType.Jpeg;
                        byte[] imageData = ImageToByteArray(image, selectedFormat);

                        string insertQuery = "INSERT INTO filesoutimages (`docs_out_info_id`, `filename`, `image`) VALUES (@docs_out_info_id, @filename, @image)";
                        using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@docs_out_info_id", docs_out_info_id);
                            cmd.Parameters.AddWithValue("@filename", txtFileOutname.Text.Trim()); // Provide filename
                            cmd.Parameters.AddWithValue("@image", imageData);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Display error message if no image is selected
                        MessageBox.Show("No image selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Get the username from lblUsername
                    string username = lblUsername.Text;

                    // Log the activity to the audit trail
                    LogAuditTrail(username, "Files Out Added", docs_out_info_id.ToString());

                    MessageBox.Show("Files saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillGridViewFilesOut();
                    pnFilesOut.Visible = true;
                    pnFilesOutAdd.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Accounts
        private void pictureBox_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            // Clear the textbox if the default text is displayed
            if (txtUsername.Text == DefaultTextUser)
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black; // Optionally change text color to black
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            // Restore the default text if the textbox is left empty
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = DefaultTextUser;
                txtUsername.ForeColor = System.Drawing.Color.Gray; // Optionally change text color to gray
            }
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            // Clear the textbox if the default text is displayed
            if (txtEmail.Text == DefaultTextEmail)
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = Color.Black; // Optionally change text color to black
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            // Restore the default text if the textbox is left empty
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = DefaultTextEmail;
                txtEmail.ForeColor = System.Drawing.Color.Gray; // Optionally change text color to gray
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            // Clear the textbox if the default text is displayed
            if (txtPassword.Text == DefaultTextPass)
            {
                txtPassword.PasswordChar = '*';
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black; // Optionally change text color to black
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            // Restore the default text if the textbox is left empty
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.PasswordChar = '\0';
                txtPassword.Text = DefaultTextPass;
                txtPassword.ForeColor = System.Drawing.Color.Gray; // Optionally change text color to gray
            }
        }

        private void txtRetypePassword_Enter(object sender, EventArgs e)
        {
            // Clear the textbox if the default text is displayed
            if (txtRetypePassword.Text == DefaultTextRetypePass)
            {
                txtRetypePassword.PasswordChar = '*';
                txtRetypePassword.Text = "";
                txtRetypePassword.ForeColor = Color.Black; // Optionally change text color to black
            }
        }

        private void txtRetypePassword_Leave(object sender, EventArgs e)
        {
            // Restore the default text if the textbox is left empty
            if (string.IsNullOrWhiteSpace(txtRetypePassword.Text))
            {
                txtRetypePassword.PasswordChar = '\0';
                txtRetypePassword.Text = DefaultTextRetypePass;
                txtRetypePassword.ForeColor = System.Drawing.Color.Gray; // Optionally change text color to gray
            }
        }

        private void btnUserSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region Retrieve Data
                // Retrieve data from form
                string username = txtUsername.Text;
                string email = txtEmail.Text;
                string password = txtPassword.Text;
                string retypePassword = txtRetypePassword.Text;
                string usertype = cmbUsertype.SelectedItem?.ToString();
                DateTime timeLogin = DateTime.Now;
                #endregion

                #region Validate Passwords
                // Check if passwords match
                if (password != retypePassword)
                {
                    MessageBox.Show("Passwords do not match!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion

                #region Hash Password
                // Generate salt and hash the password
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
                #endregion

                #region Convert Picture to Byte Array
                // Convert picture to byte array
                byte[] picture = null;
                if (pictureBox.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox.Image.Save(ms, pictureBox.Image.RawFormat);
                        picture = ms.ToArray();
                    }
                }
                #endregion

                #region Database Operations
                // Database operations
                DatabaseConnection dbcon = new DatabaseConnection();
                using (MySqlConnection conn = dbcon.GetConnection())
                {
                    conn.Open();

                    // Insert user data
                    string userQuery = "INSERT INTO users (username, email, password, salt, usertype, time_login) " +
                                       "VALUES (@Username, @Email, @Password, @Salt, @Usertype, @TimeLogin)";

                    using (MySqlCommand cmd = new MySqlCommand(userQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.Parameters.AddWithValue("@Salt", salt);
                        cmd.Parameters.AddWithValue("@Usertype", usertype);
                        cmd.Parameters.AddWithValue("@TimeLogin", timeLogin);

                        cmd.ExecuteNonQuery();
                    }

                    // Get the inserted user's ID
                    string getIdQuery = "SELECT LAST_INSERT_ID()";
                    int userId;
                    using (MySqlCommand getIdCmd = new MySqlCommand(getIdQuery, conn))
                    {
                        userId = Convert.ToInt32(getIdCmd.ExecuteScalar());
                    }

                    // Insert user picture
                    if (picture != null)
                    {
                        string pictureQuery = "INSERT INTO userImages (user_id, picture) VALUES (@UserId, @Picture)";
                        using (MySqlCommand pictureCmd = new MySqlCommand(pictureQuery, conn))
                        {
                            pictureCmd.Parameters.AddWithValue("@UserId", userId);
                            pictureCmd.Parameters.AddWithValue("@Picture", picture);
                            pictureCmd.ExecuteNonQuery();
                        }
                    }

                    #region
                    // Get the username from lblUsername
                    string usernames = lblUsername.Text;

                    // Log the activity to the audit trail
                    LogAuditTrail(usernames, "Created New Account", userId.ToString());
                    #endregion
                }
                #endregion

                #region Show Success Message
                MessageBox.Show("Signup successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pnAccountList.Visible = true;
                pnAccCotainer.Visible = false;
                FillDataGridViewAccount();
                //clearTextbox();
                #endregion
            }
            catch (Exception ex)
            {
                #region Handle Exception
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                #endregion
            }
        }

        private void FillDataGridViewAccount()
        {
            try
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    string query = AccountQueryUtility.GetAccountQueryUtility();

                    MySqlDataAdapter sda = new MySqlDataAdapter(query, conn);
                    System.Data.DataTable dt = new System.Data.DataTable();

                    sda.Fill(dt);

                    dgvAccount.DataSource = dt;

                    // Add action buttons column
                    AddActionButtonsAccount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddActionButtonsAccount()
        {
            // Clear existing columns first
            dgvAccount.Columns.Clear();

            // Add original columns from the DataTable
            foreach (System.Data.DataColumn column in ((System.Data.DataTable)dgvAccount.DataSource).Columns)
            {
                DataGridViewTextBoxColumn textColumn = new DataGridViewTextBoxColumn
                {
                    HeaderText = column.ColumnName,
                    DataPropertyName = column.ColumnName,
                    Name = column.ColumnName
                };
                dgvAccount.Columns.Add(textColumn);
            }

            // Create and add the "Edit" button column
            DataGridViewImageColumn editImageColumn = new DataGridViewImageColumn();
            editImageColumn.Name = "Edit";
            editImageColumn.HeaderText = "";
            editImageColumn.Image = Properties.Resources.actions;
            editImageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAccount.Columns.Add(editImageColumn);

            // Create and add the "Delete" button column
            DataGridViewImageColumn deleteImageColumn = new DataGridViewImageColumn();
            deleteImageColumn.Name = "Delete";
            deleteImageColumn.HeaderText = "";
            deleteImageColumn.Image = Properties.Resources.material_symbols_delete_outline;
            deleteImageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAccount.Columns.Add(deleteImageColumn);

            // Set cursor to hand when hovering over buttons
            dgvAccount.CellMouseEnter += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && (dgvAccount.Columns[e.ColumnIndex] is DataGridViewImageColumn))
                {
                    dgvAccount.Cursor = Cursors.Hand;
                }
            };

            dgvAccount.CellMouseLeave += (sender, e) =>
            {
                dgvAccount.Cursor = Cursors.Default;
            };

            // Adjust the width of the columns to fit the "ACTION" text
            using (Graphics g = dgvAccount.CreateGraphics())
            {
                SizeF textSize = g.MeasureString("ACTION", dgvAccount.ColumnHeadersDefaultCellStyle.Font);
                int totalWidth = (int)textSize.Width + 20; // Add some padding
                int columnWidth = totalWidth / 2;

                dgvAccount.Columns["Edit"].Width = columnWidth;
                dgvAccount.Columns["Delete"].Width = columnWidth;
            }
        }

        private void dgvAccount_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1) // Header row
            {
                // Check if the column is "Edit" or "Delete"
                if (e.ColumnIndex == dgvAccount.Columns["Edit"].Index || e.ColumnIndex == dgvAccount.Columns["Delete"].Index)
                {
                    // Paint the background
                    e.PaintBackground(e.CellBounds, true);

                    // Calculate the bounds for the merged cell
                    System.Drawing.Rectangle rect = e.CellBounds;

                    if (e.ColumnIndex == dgvAccount.Columns["Edit"].Index)
                    {
                        rect.Width += dgvAccount.Columns["Delete"].Width;
                    }
                    else if (e.ColumnIndex == dgvAccount.Columns["Delete"].Index)
                    {
                        rect.X -= dgvAccount.Columns["Edit"].Width;
                        rect.Width += dgvAccount.Columns["Edit"].Width;
                    }

                    // Draw the text
                    using (Brush brush = new SolidBrush(e.CellStyle.ForeColor))
                    {
                        StringFormat format = new StringFormat();
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        e.Graphics.DrawString("ACTION", e.CellStyle.Font, brush, rect, format);
                    }

                    // Skip default painting
                    e.Handled = true;
                }
            }
        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    if (dgvAccount.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        // Perform edit action
                        DataGridViewRow row = dgvAccount.Rows[e.RowIndex];
                        string username = row.Cells["username"].Value.ToString(); // Ensure this matches your column name

                        PopulateEditFieldsAccount(username);

                        pnAccountList.Visible = false;
                        pnAccCotainer.Visible = true;

                        btnUserSave.Visible = false;
                        btnUpdateUserSave.Visible = true;

                        //txtPassword.Text = "Enter your New Password";
                    }
                    // Check if the "Delete" button column is clicked
                    else if (e.ColumnIndex == dgvAccount.Columns["Delete"].Index && e.RowIndex >= 0)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete this account and its image?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                // Retrieve the ID or unique identifier of the row to delete
                                int accountId = Convert.ToInt32(dgvAccount.Rows[e.RowIndex].Cells["NO."].Value);

                                // Perform the delete operation
                                DatabaseConnection dbConnection = new DatabaseConnection();
                                using (MySqlConnection conn = dbConnection.GetConnection())
                                {
                                    conn.Open();

                                    // First, delete the image from userimages table
                                    string deleteImageQuery = $"DELETE FROM userimages WHERE user_id = {accountId}";
                                    MySqlCommand deleteImageCmd = new MySqlCommand(deleteImageQuery, conn);
                                    int imageRowsAffected = deleteImageCmd.ExecuteNonQuery();

                                    // Then, delete the user from users table
                                    string deleteUserQuery = $"DELETE FROM users WHERE id = {accountId}";
                                    MySqlCommand deleteUserCmd = new MySqlCommand(deleteUserQuery, conn);
                                    int userRowsAffected = deleteUserCmd.ExecuteNonQuery();

                                    if (userRowsAffected > 0)
                                    {
                                        MessageBox.Show("Account and its associated image deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        // Refresh DataGridView after deletion
                                        FillDataGridViewAccount();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to delete account and its associated image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateEditFieldsAccount(string username)
        {
            using (MySqlConnection connection = new DatabaseConnection().GetConnection())
            {
                // Prepare SQL command
                string query = "SELECT u.id, u.username, u.email, u.usertype, ui.picture FROM users u LEFT JOIN userimages ui ON u.id = ui.user_id WHERE u.username = @username;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);

                try
                {
                    // Open the connection
                    connection.Open();
                    // Execute the command
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtUserId.Text = reader["id"].ToString();
                            txtUsername.Text = reader["username"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                            cmbUsertype.SelectedItem = reader["usertype"].ToString();

                            // Check if image data exists in the reader
                            if (reader["picture"] != DBNull.Value)
                            {
                                byte[] imageByteArray = (byte[])reader["picture"];

                                // Load image from imageByteArray into PictureBox
                                using (MemoryStream ms = new MemoryStream(imageByteArray))
                                {
                                    pictureBox.Image = Image.FromStream(ms);
                                }
                            }
                            else
                            {
                                pictureBox.Image = null; // Clear PictureBox if no image available
                            }
                        }
                        else
                        {
                            MessageBox.Show("No data found for the specified username.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            pnAccountList.Visible = false;
            pnAccCotainer.Visible = true;

            btnUserSave.Visible = true;
            btnUpdateUserSave.Visible = false;
        }

        private void txtUserSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Cast the DataGridView's DataSource to a DataTable
                System.Data.DataTable dt = (System.Data.DataTable)dgvAccount.DataSource;

                if (dt != null)
                {
                    // Initialize a list to store filter conditions
                    List<string> filterConditions = new List<string>();

                    // Iterate through each column in the DataTable
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.DataType == typeof(string))
                        {
                            // Add a filter condition for string columns
                            filterConditions.Add($"[{column.ColumnName}] LIKE '%{txtUserSearch.Text}%'");
                        }
                        else if (column.DataType == typeof(int) || column.DataType == typeof(double) || column.DataType == typeof(decimal))
                        {
                            // Try parsing the search text to a number and add a filter condition for numeric columns
                            if (decimal.TryParse(txtUserSearch.Text, out decimal searchNumber))
                            {
                                filterConditions.Add($"CONVERT([{column.ColumnName}], 'System.String') LIKE '%{txtUserSearch.Text}%'");
                            }
                        }
                    }

                    // Combine filter conditions into a single expression
                    string filterExpression = string.Join(" OR ", filterConditions);

                    // Apply the filter to the DataTable's DefaultView
                    dt.DefaultView.RowFilter = filterExpression;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while filtering: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewTicket_Click(object sender, EventArgs e)
        {
            tsmiTicketDash.PerformClick();
        }

        private void btnGotoTotalTicket_Click(object sender, EventArgs e)
        {
            tsmiTicketDash.PerformClick();
        }

        private void btnGotoResolvedTicket_Click(object sender, EventArgs e)
        {
            tsmiTicketDash.PerformClick();
        }

        private void tsmiChartTech_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem1_Click(tsmiChartTech, EventArgs.Empty);
            chartTech.BringToFront();
            //Chart
            LoadTechnicianPerformanceChart();
        }

        private void tsmiChartEncoder_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem1_Click(tsmiChartTech, EventArgs.Empty);
            chartEncoder.BringToFront();
            //Chart
            LoadEncoderPerformanceChart();
        }

        private void btnExpoExcelTicket_Click(object sender, EventArgs e)
        {
            /*
            if (dgvTicketEncoder.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel.ApplicationClass MExcel = new Microsoft.Office.Interop.Excel.ApplicationClass();
                MExcel.Application.Workbooks.Add(Type.Missing);
                for (int i = 1; i < dgvTicketEncoder.Columns.Count + 1; i++)
                {
                    MExcel.Cells[1, i] = dgvTicketEncoder.Columns[i - 1].HeaderText;
                }
                for (int i = 0; i < dgvTicketEncoder.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvTicketEncoder.Columns.Count; j++)
                    {
                        MExcel.Cells[i + 2, j + 1] = dgvTicketEncoder.Rows[i].Cells[j].Value.ToString();
                    }
                }
                MExcel.Columns.AutoFit();
                MExcel.Rows.AutoFit();
                MExcel.Columns.Font.Size = 12;
                MExcel.Visible = true;
            }
            else
            {
                MessageBox.Show("No records found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
        }

        private void btnExpoPdfTicket_Click(object sender, EventArgs e)
        {
            // Get the path to the user's desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Set the file name to "Ticket-Encoder.pdf"
            string fileName = "Ticket-Encoder.pdf";

            // Combine the desktop path with the file name
            string fullPath = Path.Combine(desktopPath, fileName);

            // Create a new instance of the PdfExporter and export the DataGridView to PDF
            PdfExporter exporter = new PdfExporter();
            exporter.ExportDataGridViewToPdf(dgvTicketEncoder, fullPath);
        }

        private void btnExpoPdfTicketTech_Click(object sender, EventArgs e)
        {
            // Get the path to the user's desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Set the file name to "Ticket-Encoder.pdf"
            string fileName = "Ticket-Technician.pdf";

            // Combine the desktop path with the file name
            string fullPath = Path.Combine(desktopPath, fileName);

            // Create a new instance of the PdfExporter and export the DataGridView to PDF
            PdfExporter exporter = new PdfExporter();
            exporter.ExportDataGridViewToPdfTech(dgvTicketTech, fullPath);
        }

        private void btnExpoPdfFilesIn_Click(object sender, EventArgs e)
        {
            // Get the path to the user's desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Set the file name to "Ticket-Encoder.pdf"
            string fileName = "Files-In.pdf";

            // Combine the desktop path with the file name
            string fullPath = Path.Combine(desktopPath, fileName);

            // Create a new instance of the PdfExporter and export the DataGridView to PDF
            PdfExporter exporter = new PdfExporter();
            exporter.ExportDataGridViewToPdfFilesIn(dgvFilesIn, fullPath);
        }

        private void btnExpoPdfFilesOut_Click(object sender, EventArgs e)
        {
            // Get the path to the user's desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Set the file name to "Files-Out.pdf"
            string fileName = "Files-Out.pdf";

            // Combine the desktop path with the file name
            string fullPath = Path.Combine(desktopPath, fileName);

            // Create a new instance of the PdfExporter and export the DataGridView to PDF
            PdfExporter exporter = new PdfExporter();
            exporter.ExportDataGridViewToPdfFilesOut(dgvFilesOut, fullPath);
        }

        private void btnExpoPdfVol_Click(object sender, EventArgs e)
        {
            // Get the path to the user's desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Set the file name to "Files-Out.pdf"
            string fileName = "Report.pdf";

            // Combine the desktop path with the file name
            string fullPath = Path.Combine(desktopPath, fileName);

            // Create a new instance of the PdfExporter and export the DataGridView to PDF
            PdfExporter exporter = new PdfExporter();
            exporter.ExportDataGridViewToPdfVol(reportTicketVolume1.dgvReportDay, fullPath);
        }

        private void btnExpoPdfAudit_Click(object sender, EventArgs e)
        {
            // Get the path to the user's desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Set the file name to "Files-Out.pdf"
            string fileName = "Audit-Trail.pdf";

            // Combine the desktop path with the file name
            string fullPath = Path.Combine(desktopPath, fileName);

            // Create a new instance of the PdfExporter and export the DataGridView to PDF
            PdfExporter exporter = new PdfExporter();
            exporter.ExportDataGridViewToPdfAudit(audit_Trail1.dgvAudit, fullPath);
        }
    }
}
