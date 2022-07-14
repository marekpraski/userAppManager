
namespace UniwersalnyDesktop
{
    partial class ProfileNew
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbNazwa = new System.Windows.Forms.TextBox();
            this.btnLogo = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnZapisz = new System.Windows.Forms.ToolStripButton();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbConfig = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "nazwa";
            // 
            // tbNazwa
            // 
            this.tbNazwa.Location = new System.Drawing.Point(67, 28);
            this.tbNazwa.Name = "tbNazwa";
            this.tbNazwa.Size = new System.Drawing.Size(586, 20);
            this.tbNazwa.TabIndex = 3;
            // 
            // btnLogo
            // 
            this.btnLogo.Location = new System.Drawing.Point(67, 68);
            this.btnLogo.Name = "btnLogo";
            this.btnLogo.Size = new System.Drawing.Size(40, 23);
            this.btnLogo.TabIndex = 7;
            this.btnLogo.Text = "logo";
            this.btnLogo.UseVisualStyleBackColor = true;
            this.btnLogo.Click += new System.EventHandler(this.btnLogo_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZapisz});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1075, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnZapisz
            // 
            this.btnZapisz.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZapisz.Image = global::UniwersalnyDesktop.Properties.Resources.Save_16x;
            this.btnZapisz.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZapisz.Name = "btnZapisz";
            this.btnZapisz.Size = new System.Drawing.Size(23, 22);
            this.btnZapisz.Text = "Zapisz";
            this.btnZapisz.Click += new System.EventHandler(this.btnZapisz_Click);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Location = new System.Drawing.Point(12, 97);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(150, 102);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxLogo.TabIndex = 11;
            this.pictureBoxLogo.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "plik konfiguracyjny config.xml";
            // 
            // tbConfig
            // 
            this.tbConfig.Location = new System.Drawing.Point(177, 97);
            this.tbConfig.Multiline = true;
            this.tbConfig.Name = "tbConfig";
            this.tbConfig.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbConfig.Size = new System.Drawing.Size(886, 561);
            this.tbConfig.TabIndex = 29;
            // 
            // ProfileNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 670);
            this.Controls.Add(this.tbConfig);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnLogo);
            this.Controls.Add(this.tbNazwa);
            this.Controls.Add(this.label1);
            this.Name = "ProfileNew";
            this.Text = "ProfileNew";
            this.Load += new System.EventHandler(this.ProfileNew_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbNazwa;
        private System.Windows.Forms.Button btnLogo;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnZapisz;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbConfig;
    }
}