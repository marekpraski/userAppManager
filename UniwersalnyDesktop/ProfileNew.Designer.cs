
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbNazwa = new System.Windows.Forms.TextBox();
            this.tbDomena = new System.Windows.Forms.TextBox();
            this.tbLdap = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnZapisz = new System.Windows.Forms.ToolStripButton();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "domena";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "ldap";
            // 
            // tbNazwa
            // 
            this.tbNazwa.Location = new System.Drawing.Point(67, 28);
            this.tbNazwa.Name = "tbNazwa";
            this.tbNazwa.Size = new System.Drawing.Size(586, 20);
            this.tbNazwa.TabIndex = 3;
            // 
            // tbDomena
            // 
            this.tbDomena.Location = new System.Drawing.Point(67, 57);
            this.tbDomena.Name = "tbDomena";
            this.tbDomena.Size = new System.Drawing.Size(276, 20);
            this.tbDomena.TabIndex = 4;
            // 
            // tbLdap
            // 
            this.tbLdap.Location = new System.Drawing.Point(67, 88);
            this.tbLdap.Name = "tbLdap";
            this.tbLdap.Size = new System.Drawing.Size(276, 20);
            this.tbLdap.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "logo";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZapisz});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(665, 25);
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
            this.pictureBoxLogo.Location = new System.Drawing.Point(67, 125);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(276, 100);
            this.pictureBoxLogo.TabIndex = 6;
            this.pictureBoxLogo.TabStop = false;
            // 
            // ProfileNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 232);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.tbLdap);
            this.Controls.Add(this.tbDomena);
            this.Controls.Add(this.tbNazwa);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbNazwa;
        private System.Windows.Forms.TextBox tbDomena;
        private System.Windows.Forms.TextBox tbLdap;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnZapisz;
    }
}