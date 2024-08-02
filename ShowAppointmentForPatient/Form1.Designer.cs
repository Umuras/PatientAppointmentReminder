namespace ShowAppointmentForPatient
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_appointmentTitle = new System.Windows.Forms.Label();
            this.txt_AppointmentInfos = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_appointmentTitle
            // 
            this.lbl_appointmentTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_appointmentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_appointmentTitle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbl_appointmentTitle.Location = new System.Drawing.Point(0, 0);
            this.lbl_appointmentTitle.Name = "lbl_appointmentTitle";
            this.lbl_appointmentTitle.Size = new System.Drawing.Size(800, 25);
            this.lbl_appointmentTitle.TabIndex = 2;
            this.lbl_appointmentTitle.Text = "RANDEVU BİLGİLERİ";
            this.lbl_appointmentTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_AppointmentInfos
            // 
            this.txt_AppointmentInfos.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txt_AppointmentInfos.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txt_AppointmentInfos.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_AppointmentInfos.Location = new System.Drawing.Point(30, 63);
            this.txt_AppointmentInfos.Multiline = true;
            this.txt_AppointmentInfos.Name = "txt_AppointmentInfos";
            this.txt_AppointmentInfos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_AppointmentInfos.Size = new System.Drawing.Size(730, 355);
            this.txt_AppointmentInfos.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txt_AppointmentInfos);
            this.Controls.Add(this.lbl_appointmentTitle);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_appointmentTitle;
        private System.Windows.Forms.TextBox txt_AppointmentInfos;
    }
}

