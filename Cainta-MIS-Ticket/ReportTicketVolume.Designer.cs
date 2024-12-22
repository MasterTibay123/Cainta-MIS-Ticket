namespace Cainta_MIS_Ticket
{
    partial class ReportTicketVolume
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnDate = new System.Windows.Forms.Panel();
            this.pnTicketEncoder = new System.Windows.Forms.Panel();
            this.dgvReportDay = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnTicketTech = new System.Windows.Forms.Panel();
            this.dgpTicketTechRepo = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnNormal = new System.Windows.Forms.Button();
            this.btnCross = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.lblTotalTicket = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDayDigit = new System.Windows.Forms.Label();
            this.lblDay = new System.Windows.Forms.Label();
            this.pnTop = new System.Windows.Forms.Panel();
            this.cmbSelect = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnNextDate = new System.Windows.Forms.Button();
            this.btnPrevDate = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnTitleHeader = new System.Windows.Forms.Panel();
            this.pnDate.SuspendLayout();
            this.pnTicketEncoder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportDay)).BeginInit();
            this.pnTicketTech.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgpTicketTechRepo)).BeginInit();
            this.pnTop.SuspendLayout();
            this.pnTitleHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnDate
            // 
            this.pnDate.Controls.Add(this.pnTicketEncoder);
            this.pnDate.Controls.Add(this.pnTicketTech);
            this.pnDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDate.Location = new System.Drawing.Point(0, 110);
            this.pnDate.Name = "pnDate";
            this.pnDate.Size = new System.Drawing.Size(902, 308);
            this.pnDate.TabIndex = 8;
            // 
            // pnTicketEncoder
            // 
            this.pnTicketEncoder.Controls.Add(this.dgvReportDay);
            this.pnTicketEncoder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTicketEncoder.Location = new System.Drawing.Point(0, 0);
            this.pnTicketEncoder.Name = "pnTicketEncoder";
            this.pnTicketEncoder.Size = new System.Drawing.Size(902, 308);
            this.pnTicketEncoder.TabIndex = 3;
            // 
            // dgvReportDay
            // 
            this.dgvReportDay.AllowUserToAddRows = false;
            this.dgvReportDay.AllowUserToDeleteRows = false;
            this.dgvReportDay.AllowUserToResizeColumns = false;
            this.dgvReportDay.AllowUserToResizeRows = false;
            this.dgvReportDay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReportDay.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvReportDay.BackgroundColor = System.Drawing.Color.White;
            this.dgvReportDay.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvReportDay.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReportDay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReportDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReportDay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReportDay.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReportDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReportDay.EnableHeadersVisualStyles = false;
            this.dgvReportDay.GridColor = System.Drawing.Color.White;
            this.dgvReportDay.Location = new System.Drawing.Point(0, 0);
            this.dgvReportDay.MultiSelect = false;
            this.dgvReportDay.Name = "dgvReportDay";
            this.dgvReportDay.ReadOnly = true;
            this.dgvReportDay.RowHeadersVisible = false;
            this.dgvReportDay.Size = new System.Drawing.Size(902, 308);
            this.dgvReportDay.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "date";
            this.Column1.HeaderText = "DATE";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "date";
            this.Column2.HeaderText = "TIME";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Visible = false;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "office";
            this.Column3.HeaderText = "OFFICE";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "name";
            this.Column4.HeaderText = "EMPLOYEES";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "resolved";
            this.Column5.HeaderText = "RESOLVED";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "id";
            this.Column6.HeaderText = "TICKET #";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // pnTicketTech
            // 
            this.pnTicketTech.Controls.Add(this.dgpTicketTechRepo);
            this.pnTicketTech.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTicketTech.Location = new System.Drawing.Point(0, 0);
            this.pnTicketTech.Name = "pnTicketTech";
            this.pnTicketTech.Size = new System.Drawing.Size(902, 308);
            this.pnTicketTech.TabIndex = 4;
            // 
            // dgpTicketTechRepo
            // 
            this.dgpTicketTechRepo.AllowUserToAddRows = false;
            this.dgpTicketTechRepo.AllowUserToDeleteRows = false;
            this.dgpTicketTechRepo.AllowUserToResizeColumns = false;
            this.dgpTicketTechRepo.AllowUserToResizeRows = false;
            this.dgpTicketTechRepo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgpTicketTechRepo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgpTicketTechRepo.BackgroundColor = System.Drawing.Color.White;
            this.dgpTicketTechRepo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgpTicketTechRepo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgpTicketTechRepo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgpTicketTechRepo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgpTicketTechRepo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgpTicketTechRepo.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgpTicketTechRepo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgpTicketTechRepo.EnableHeadersVisualStyles = false;
            this.dgpTicketTechRepo.GridColor = System.Drawing.Color.White;
            this.dgpTicketTechRepo.Location = new System.Drawing.Point(0, 0);
            this.dgpTicketTechRepo.MultiSelect = false;
            this.dgpTicketTechRepo.Name = "dgpTicketTechRepo";
            this.dgpTicketTechRepo.ReadOnly = true;
            this.dgpTicketTechRepo.RowHeadersVisible = false;
            this.dgpTicketTechRepo.Size = new System.Drawing.Size(902, 308);
            this.dgpTicketTechRepo.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "date";
            this.dataGridViewTextBoxColumn1.HeaderText = "DATE";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "date";
            this.dataGridViewTextBoxColumn2.HeaderText = "TIME";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "office";
            this.dataGridViewTextBoxColumn3.HeaderText = "OFFICE";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn4.HeaderText = "EMPLOYEES";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "resolved";
            this.dataGridViewTextBoxColumn5.HeaderText = "RESOLVED";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn6.HeaderText = "TICKET #";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // btnNormal
            // 
            this.btnNormal.BackColor = System.Drawing.Color.White;
            this.btnNormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNormal.ForeColor = System.Drawing.Color.Black;
            this.btnNormal.Location = new System.Drawing.Point(90, 20);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(25, 25);
            this.btnNormal.TabIndex = 6;
            this.btnNormal.Text = "-";
            this.btnNormal.UseVisualStyleBackColor = false;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnCross
            // 
            this.btnCross.BackColor = System.Drawing.Color.White;
            this.btnCross.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCross.ForeColor = System.Drawing.Color.Black;
            this.btnCross.Location = new System.Drawing.Point(62, 20);
            this.btnCross.Name = "btnCross";
            this.btnCross.Size = new System.Drawing.Size(25, 25);
            this.btnCross.TabIndex = 5;
            this.btnCross.Text = "b";
            this.btnCross.UseVisualStyleBackColor = false;
            this.btnCross.Click += new System.EventHandler(this.btnCross_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.BackColor = System.Drawing.Color.White;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheck.ForeColor = System.Drawing.Color.Black;
            this.btnCheck.Location = new System.Drawing.Point(34, 20);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(25, 25);
            this.btnCheck.TabIndex = 4;
            this.btnCheck.Text = "b";
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // lblTotalTicket
            // 
            this.lblTotalTicket.AutoSize = true;
            this.lblTotalTicket.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalTicket.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTicket.Location = new System.Drawing.Point(170, 22);
            this.lblTotalTicket.Name = "lblTotalTicket";
            this.lblTotalTicket.Size = new System.Drawing.Size(16, 20);
            this.lblTotalTicket.TabIndex = 3;
            this.lblTotalTicket.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(7)))), ((int)(((byte)(15)))));
            this.label2.Location = new System.Drawing.Point(118, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Total:";
            // 
            // lblDayDigit
            // 
            this.lblDayDigit.AutoSize = true;
            this.lblDayDigit.BackColor = System.Drawing.Color.Transparent;
            this.lblDayDigit.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayDigit.ForeColor = System.Drawing.Color.White;
            this.lblDayDigit.Location = new System.Drawing.Point(840, 36);
            this.lblDayDigit.Name = "lblDayDigit";
            this.lblDayDigit.Size = new System.Drawing.Size(35, 16);
            this.lblDayDigit.TabIndex = 2;
            this.lblDayDigit.Text = "label2";
            // 
            // lblDay
            // 
            this.lblDay.AutoSize = true;
            this.lblDay.BackColor = System.Drawing.Color.Transparent;
            this.lblDay.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay.ForeColor = System.Drawing.Color.White;
            this.lblDay.Location = new System.Drawing.Point(805, 7);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(35, 16);
            this.lblDay.TabIndex = 2;
            this.lblDay.Text = "label2";
            // 
            // pnTop
            // 
            this.pnTop.Controls.Add(this.btnNormal);
            this.pnTop.Controls.Add(this.btnCross);
            this.pnTop.Controls.Add(this.btnCheck);
            this.pnTop.Controls.Add(this.lblTotalTicket);
            this.pnTop.Controls.Add(this.label2);
            this.pnTop.Controls.Add(this.lblDayDigit);
            this.pnTop.Controls.Add(this.lblDay);
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.Location = new System.Drawing.Point(0, 50);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(902, 60);
            this.pnTop.TabIndex = 7;
            this.pnTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pnTop_Paint);
            // 
            // cmbSelect
            // 
            this.cmbSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbSelect.FormattingEnabled = true;
            this.cmbSelect.Items.AddRange(new object[] {
            "Encoder",
            "Technician"});
            this.cmbSelect.Location = new System.Drawing.Point(774, 14);
            this.cmbSelect.Name = "cmbSelect";
            this.cmbSelect.Size = new System.Drawing.Size(121, 27);
            this.cmbSelect.TabIndex = 3;
            this.cmbSelect.Text = "Select";
            this.cmbSelect.Visible = false;
            this.cmbSelect.SelectedIndexChanged += new System.EventHandler(this.cmbSelect_SelectedIndexChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.White;
            this.lblDate.Location = new System.Drawing.Point(657, 16);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(36, 16);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "DATE";
            // 
            // btnNextDate
            // 
            this.btnNextDate.FlatAppearance.BorderSize = 0;
            this.btnNextDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextDate.ForeColor = System.Drawing.Color.White;
            this.btnNextDate.Image = global::Cainta_MIS_Ticket.Properties.Resources.material_symbols_keyboard_arrow_down_rounded__1_;
            this.btnNextDate.Location = new System.Drawing.Point(732, 13);
            this.btnNextDate.Name = "btnNextDate";
            this.btnNextDate.Size = new System.Drawing.Size(23, 23);
            this.btnNextDate.TabIndex = 1;
            this.btnNextDate.UseVisualStyleBackColor = true;
            this.btnNextDate.Click += new System.EventHandler(this.btnNextDate_Click);
            // 
            // btnPrevDate
            // 
            this.btnPrevDate.FlatAppearance.BorderSize = 0;
            this.btnPrevDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevDate.ForeColor = System.Drawing.Color.White;
            this.btnPrevDate.Image = global::Cainta_MIS_Ticket.Properties.Resources.material_symbols_keyboard_arrow_down_rounded;
            this.btnPrevDate.Location = new System.Drawing.Point(628, 13);
            this.btnPrevDate.Name = "btnPrevDate";
            this.btnPrevDate.Size = new System.Drawing.Size(23, 23);
            this.btnPrevDate.TabIndex = 1;
            this.btnPrevDate.UseVisualStyleBackColor = true;
            this.btnPrevDate.Click += new System.EventHandler(this.btnPrevDate_Click);
            // 
            // btnToday
            // 
            this.btnToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToday.ForeColor = System.Drawing.Color.White;
            this.btnToday.Location = new System.Drawing.Point(533, 13);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(75, 23);
            this.btnToday.TabIndex = 1;
            this.btnToday.Text = "Today";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Poppins", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.LightBlue;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(298, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of Tickets Received";
            // 
            // pnTitleHeader
            // 
            this.pnTitleHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(128)))));
            this.pnTitleHeader.Controls.Add(this.cmbSelect);
            this.pnTitleHeader.Controls.Add(this.lblDate);
            this.pnTitleHeader.Controls.Add(this.btnNextDate);
            this.pnTitleHeader.Controls.Add(this.btnPrevDate);
            this.pnTitleHeader.Controls.Add(this.btnToday);
            this.pnTitleHeader.Controls.Add(this.label1);
            this.pnTitleHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTitleHeader.Location = new System.Drawing.Point(0, 0);
            this.pnTitleHeader.Name = "pnTitleHeader";
            this.pnTitleHeader.Size = new System.Drawing.Size(902, 50);
            this.pnTitleHeader.TabIndex = 6;
            // 
            // ReportTicketVolume
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnDate);
            this.Controls.Add(this.pnTop);
            this.Controls.Add(this.pnTitleHeader);
            this.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ReportTicketVolume";
            this.Size = new System.Drawing.Size(902, 418);
            this.Load += new System.EventHandler(this.ReportTicketVolume_Load);
            this.pnDate.ResumeLayout(false);
            this.pnTicketEncoder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportDay)).EndInit();
            this.pnTicketTech.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgpTicketTechRepo)).EndInit();
            this.pnTop.ResumeLayout(false);
            this.pnTop.PerformLayout();
            this.pnTitleHeader.ResumeLayout(false);
            this.pnTitleHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnDate;
        private System.Windows.Forms.Panel pnTicketEncoder;
        private System.Windows.Forms.Panel pnTicketTech;
        private System.Windows.Forms.Button btnCross;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Label lblTotalTicket;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDayDigit;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Panel pnTop;
        private System.Windows.Forms.ComboBox cmbSelect;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnNextDate;
        private System.Windows.Forms.Button btnPrevDate;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnTitleHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridView dgpTicketTechRepo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        public System.Windows.Forms.Button btnNormal;
        public System.Windows.Forms.DataGridView dgvReportDay;
    }
}
