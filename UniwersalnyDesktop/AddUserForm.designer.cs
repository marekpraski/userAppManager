namespace UniwersalnyDesktop
{
    partial class AddUserForm
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
            this.components = new System.ComponentModel.Container();
            this.typUzytkownikaCB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUzytkownik = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.gbDane = new System.Windows.Forms.GroupBox();
            this.tbHaslo = new System.Windows.Forms.TextBox();
            this.oddzialTB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nazwiskoTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.imieTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.loginTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnZapisz = new System.Windows.Forms.ToolStripButton();
            this.gbDane.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // typUzytkownikaCB
            // 
            this.typUzytkownikaCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typUzytkownikaCB.FormattingEnabled = true;
            this.typUzytkownikaCB.Location = new System.Drawing.Point(124, 28);
            this.typUzytkownikaCB.Name = "typUzytkownikaCB";
            this.typUzytkownikaCB.Size = new System.Drawing.Size(173, 21);
            this.typUzytkownikaCB.TabIndex = 2;
            this.typUzytkownikaCB.SelectedIndexChanged += new System.EventHandler(this.typUzytkownikaCB_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Rodzaj poświadczeń (*)";
            // 
            // cbUzytkownik
            // 
            this.cbUzytkownik.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUzytkownik.FormattingEnabled = true;
            this.cbUzytkownik.Location = new System.Drawing.Point(126, 61);
            this.cbUzytkownik.Name = "cbUzytkownik";
            this.cbUzytkownik.Size = new System.Drawing.Size(173, 21);
            this.cbUzytkownik.TabIndex = 3;
            this.cbUzytkownik.SelectedIndexChanged += new System.EventHandler(this.uzytkownikCB_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Użytkownik (*)";
            // 
            // gbDane
            // 
            this.gbDane.Controls.Add(this.tbHaslo);
            this.gbDane.Controls.Add(this.oddzialTB);
            this.gbDane.Controls.Add(this.label8);
            this.gbDane.Controls.Add(this.nazwiskoTB);
            this.gbDane.Controls.Add(this.label6);
            this.gbDane.Controls.Add(this.imieTB);
            this.gbDane.Controls.Add(this.label4);
            this.gbDane.Controls.Add(this.label3);
            this.gbDane.Controls.Add(this.loginTB);
            this.gbDane.Controls.Add(this.label1);
            this.gbDane.Location = new System.Drawing.Point(12, 94);
            this.gbDane.Name = "gbDane";
            this.gbDane.Size = new System.Drawing.Size(315, 187);
            this.gbDane.TabIndex = 18;
            this.gbDane.TabStop = false;
            this.gbDane.Text = "Dane";
            // 
            // tbHaslo
            // 
            this.tbHaslo.Location = new System.Drawing.Point(117, 116);
            this.tbHaslo.Name = "tbHaslo";
            this.tbHaslo.Size = new System.Drawing.Size(167, 20);
            this.tbHaslo.TabIndex = 28;
            // 
            // oddzialTB
            // 
            this.oddzialTB.Location = new System.Drawing.Point(114, 153);
            this.oddzialTB.Name = "oddzialTB";
            this.oddzialTB.Size = new System.Drawing.Size(173, 20);
            this.oddzialTB.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(67, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Oddział";
            // 
            // nazwiskoTB
            // 
            this.nazwiskoTB.Location = new System.Drawing.Point(112, 46);
            this.nazwiskoTB.Name = "nazwiskoTB";
            this.nazwiskoTB.Size = new System.Drawing.Size(173, 20);
            this.nazwiskoTB.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Nazwisko";
            // 
            // imieTB
            // 
            this.imieTB.Location = new System.Drawing.Point(112, 10);
            this.imieTB.Name = "imieTB";
            this.imieTB.Size = new System.Drawing.Size(173, 20);
            this.imieTB.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(84, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Imię";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Hasło (*)";
            // 
            // loginTB
            // 
            this.loginTB.Location = new System.Drawing.Point(114, 82);
            this.loginTB.Name = "loginTB";
            this.loginTB.Size = new System.Drawing.Size(173, 20);
            this.loginTB.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Login (*)";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZapisz});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(348, 25);
            this.toolStrip1.TabIndex = 19;
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
            // AddUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 288);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.gbDane);
            this.Controls.Add(this.cbUzytkownik);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.typUzytkownikaCB);
            this.Controls.Add(this.label2);
            this.Name = "AddUserForm";
            this.Text = "Dodaj użytkownika";
            this.Load += new System.EventHandler(this.AddUserPermission_Load);
            this.gbDane.ResumeLayout(false);
            this.gbDane.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox typUzytkownikaCB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbUzytkownik;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox gbDane;
        private System.Windows.Forms.TextBox nazwiskoTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox imieTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox loginTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox oddzialTB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnZapisz;
        private System.Windows.Forms.TextBox tbHaslo;
    }
}