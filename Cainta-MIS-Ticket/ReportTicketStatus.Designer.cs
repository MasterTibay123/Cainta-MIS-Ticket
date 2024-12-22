namespace Cainta_MIS_Ticket
{
    partial class ReportTicketStatus
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTicketNotResolved = new System.Windows.Forms.Label();
            this.pnTop = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblDayDigit = new System.Windows.Forms.Label();
            this.lblDay = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTicketResolved = new System.Windows.Forms.Label();
            this.pnTitleHeader = new System.Windows.Forms.Panel();
            this.cmbSelect = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            this.pnTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnTitleHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.lblTicketNotResolved);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(451, 110);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(451, 190);
            this.panel2.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Poppins", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(183, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 36);
            this.label6.TabIndex = 0;
            this.label6.Text = "NOT RESOLVED";
            // 
            // lblTicketNotResolved
            // 
            this.lblTicketNotResolved.AutoSize = true;
            this.lblTicketNotResolved.BackColor = System.Drawing.Color.Transparent;
            this.lblTicketNotResolved.Font = new System.Drawing.Font("Poppins", 64F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTicketNotResolved.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(128)))));
            this.lblTicketNotResolved.Location = new System.Drawing.Point(220, 4);
            this.lblTicketNotResolved.Name = "lblTicketNotResolved";
            this.lblTicketNotResolved.Size = new System.Drawing.Size(90, 113);
            this.lblTicketNotResolved.TabIndex = 0;
            this.lblTicketNotResolved.Text = "0";
            // 
            // pnTop
            // 
            this.pnTop.Controls.Add(this.label3);
            this.pnTop.Controls.Add(this.lblDate);
            this.pnTop.Controls.Add(this.lblDayDigit);
            this.pnTop.Controls.Add(this.lblDay);
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.Location = new System.Drawing.Point(0, 50);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(902, 60);
            this.pnTop.TabIndex = 9;
            this.pnTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pnTop_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Poppins", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(11, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 36);
            this.label3.TabIndex = 4;
            this.label3.Text = "Report as of: ";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Font = new System.Drawing.Font("Poppins", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(7)))), ((int)(((byte)(15)))));
            this.lblDate.Location = new System.Drawing.Point(183, 18);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(28, 36);
            this.lblDate.TabIndex = 3;
            this.lblDate.Text = "0";
            // 
            // lblDayDigit
            // 
            this.lblDayDigit.AutoSize = true;
            this.lblDayDigit.BackColor = System.Drawing.Color.Transparent;
            this.lblDayDigit.Font = new System.Drawing.Font("Poppins", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDayDigit.ForeColor = System.Drawing.Color.White;
            this.lblDayDigit.Location = new System.Drawing.Point(798, 32);
            this.lblDayDigit.Name = "lblDayDigit";
            this.lblDayDigit.Size = new System.Drawing.Size(67, 31);
            this.lblDayDigit.TabIndex = 2;
            this.lblDayDigit.Text = "label2";
            // 
            // lblDay
            // 
            this.lblDay.AutoSize = true;
            this.lblDay.BackColor = System.Drawing.Color.Transparent;
            this.lblDay.Font = new System.Drawing.Font("Poppins", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDay.ForeColor = System.Drawing.Color.White;
            this.lblDay.Location = new System.Drawing.Point(763, 4);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(61, 28);
            this.lblDay.TabIndex = 2;
            this.lblDay.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Poppins", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.LightBlue;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of Tickets Per Status";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblTicketResolved);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 190);
            this.panel1.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Poppins", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(194, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 36);
            this.label4.TabIndex = 0;
            this.label4.Text = "RESOLVED";
            // 
            // lblTicketResolved
            // 
            this.lblTicketResolved.AutoSize = true;
            this.lblTicketResolved.BackColor = System.Drawing.Color.Transparent;
            this.lblTicketResolved.Font = new System.Drawing.Font("Poppins", 64F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTicketResolved.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(128)))));
            this.lblTicketResolved.Location = new System.Drawing.Point(208, 4);
            this.lblTicketResolved.Name = "lblTicketResolved";
            this.lblTicketResolved.Size = new System.Drawing.Size(90, 113);
            this.lblTicketResolved.TabIndex = 0;
            this.lblTicketResolved.Text = "0";
            // 
            // pnTitleHeader
            // 
            this.pnTitleHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(128)))));
            this.pnTitleHeader.Controls.Add(this.cmbSelect);
            this.pnTitleHeader.Controls.Add(this.label1);
            this.pnTitleHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTitleHeader.Location = new System.Drawing.Point(0, 0);
            this.pnTitleHeader.Name = "pnTitleHeader";
            this.pnTitleHeader.Size = new System.Drawing.Size(902, 50);
            this.pnTitleHeader.TabIndex = 8;
            // 
            // cmbSelect
            // 
            this.cmbSelect.Font = new System.Drawing.Font("Poppins", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbSelect.FormattingEnabled = true;
            this.cmbSelect.Location = new System.Drawing.Point(758, 13);
            this.cmbSelect.Name = "cmbSelect";
            this.cmbSelect.Size = new System.Drawing.Size(121, 36);
            this.cmbSelect.TabIndex = 1;
            this.cmbSelect.Text = "ALL";
            this.cmbSelect.Visible = false;
            // 
            // ReportTicketStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnTop);
            this.Controls.Add(this.pnTitleHeader);
            this.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ReportTicketStatus";
            this.Size = new System.Drawing.Size(902, 300);
            this.Load += new System.EventHandler(this.ReportTicketStatus_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnTop.ResumeLayout(false);
            this.pnTop.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnTitleHeader.ResumeLayout(false);
            this.pnTitleHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTicketNotResolved;
        private System.Windows.Forms.Panel pnTop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblDayDigit;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTicketResolved;
        private System.Windows.Forms.Panel pnTitleHeader;
        private System.Windows.Forms.ComboBox cmbSelect;
    }
}
