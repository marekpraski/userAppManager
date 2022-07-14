
namespace UniwersalnyDesktop
{
    partial class ProfileEditor
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
            this.dgvProfileApps = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNazwa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnZapisz = new System.Windows.Forms.ToolStripButton();
            this.btnDodajProfil = new System.Windows.Forms.ToolStripButton();
            this.btnEdytujProfil = new System.Windows.Forms.ToolStripButton();
            this.btnUsunProfil = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDodajAplikacje = new System.Windows.Forms.ToolStripButton();
            this.btnUsunAplikacje = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDodajUzytkownika = new System.Windows.Forms.ToolStripButton();
            this.btnUsunUzytkownika = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.dgvUzytkownik = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNazwaU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Serwer = new System.Windows.Forms.Label();
            this.labelSerwer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileApps)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUzytkownik)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvProfileApps
            // 
            this.dgvProfileApps.AllowUserToAddRows = false;
            this.dgvProfileApps.AllowUserToDeleteRows = false;
            this.dgvProfileApps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProfileApps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colNazwa});
            this.dgvProfileApps.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvProfileApps.Location = new System.Drawing.Point(168, 58);
            this.dgvProfileApps.Name = "dgvProfileApps";
            this.dgvProfileApps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfileApps.Size = new System.Drawing.Size(252, 380);
            this.dgvProfileApps.TabIndex = 9;
            // 
            // colId
            // 
            this.colId.HeaderText = "id";
            this.colId.Name = "colId";
            this.colId.Visible = false;
            // 
            // colNazwa
            // 
            this.colNazwa.HeaderText = "Nazwa aplikacji";
            this.colNazwa.Name = "colNazwa";
            this.colNazwa.Width = 200;
            // 
            // cbProfiles
            // 
            this.cbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfiles.FormattingEnabled = true;
            this.cbProfiles.Location = new System.Drawing.Point(69, 28);
            this.cbProfiles.Name = "cbProfiles";
            this.cbProfiles.Size = new System.Drawing.Size(619, 21);
            this.cbProfiles.TabIndex = 8;
            this.cbProfiles.SelectedIndexChanged += new System.EventHandler(this.cbProfiles_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Profil";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZapisz,
            this.btnDodajProfil,
            this.btnEdytujProfil,
            this.btnUsunProfil,
            this.toolStripSeparator2,
            this.toolStripSeparator1,
            this.btnDodajAplikacje,
            this.btnUsunAplikacje,
            this.toolStripSeparator4,
            this.toolStripSeparator3,
            this.btnDodajUzytkownika,
            this.btnUsunUzytkownika});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(699, 25);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnZapisz
            // 
            this.btnZapisz.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZapisz.Image = global::UniwersalnyDesktop.Properties.Resources.Save_16x;
            this.btnZapisz.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZapisz.Name = "btnZapisz";
            this.btnZapisz.Size = new System.Drawing.Size(23, 22);
            this.btnZapisz.Text = "Zapisz miany";
            this.btnZapisz.Click += new System.EventHandler(this.btnZapisz_Click);
            // 
            // btnDodajProfil
            // 
            this.btnDodajProfil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDodajProfil.Image = global::UniwersalnyDesktop.Properties.Resources.Add_16x;
            this.btnDodajProfil.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDodajProfil.Name = "btnDodajProfil";
            this.btnDodajProfil.Size = new System.Drawing.Size(23, 22);
            this.btnDodajProfil.Text = "Dodaj nowy profil";
            this.btnDodajProfil.Click += new System.EventHandler(this.btnDodajProfil_Click);
            // 
            // btnEdytujProfil
            // 
            this.btnEdytujProfil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEdytujProfil.Enabled = false;
            this.btnEdytujProfil.Image = global::UniwersalnyDesktop.Properties.Resources.Edit_16x;
            this.btnEdytujProfil.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdytujProfil.Name = "btnEdytujProfil";
            this.btnEdytujProfil.Size = new System.Drawing.Size(23, 22);
            this.btnEdytujProfil.Text = "Edytuj profil";
            this.btnEdytujProfil.Click += new System.EventHandler(this.btnEdytujProfil_Click);
            // 
            // btnUsunProfil
            // 
            this.btnUsunProfil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUsunProfil.Enabled = false;
            this.btnUsunProfil.Image = global::UniwersalnyDesktop.Properties.Resources.delete;
            this.btnUsunProfil.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUsunProfil.Name = "btnUsunProfil";
            this.btnUsunProfil.Size = new System.Drawing.Size(23, 22);
            this.btnUsunProfil.Text = "Usuń profil";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDodajAplikacje
            // 
            this.btnDodajAplikacje.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDodajAplikacje.Enabled = false;
            this.btnDodajAplikacje.Image = global::UniwersalnyDesktop.Properties.Resources.LinkValidator_16x;
            this.btnDodajAplikacje.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDodajAplikacje.Name = "btnDodajAplikacje";
            this.btnDodajAplikacje.Size = new System.Drawing.Size(23, 22);
            this.btnDodajAplikacje.Text = "Dodaj aplikacje do profilu";
            this.btnDodajAplikacje.Click += new System.EventHandler(this.btnDodajAplikacje_Click);
            // 
            // btnUsunAplikacje
            // 
            this.btnUsunAplikacje.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUsunAplikacje.Enabled = false;
            this.btnUsunAplikacje.Image = global::UniwersalnyDesktop.Properties.Resources.LinkRemoved_16x;
            this.btnUsunAplikacje.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUsunAplikacje.Name = "btnUsunAplikacje";
            this.btnUsunAplikacje.Size = new System.Drawing.Size(23, 22);
            this.btnUsunAplikacje.Text = "Usuń aplikację z profilu";
            this.btnUsunAplikacje.Click += new System.EventHandler(this.btnUsunAplikacje_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDodajUzytkownika
            // 
            this.btnDodajUzytkownika.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDodajUzytkownika.Enabled = false;
            this.btnDodajUzytkownika.Image = global::UniwersalnyDesktop.Properties.Resources.AddUser_16x;
            this.btnDodajUzytkownika.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDodajUzytkownika.Name = "btnDodajUzytkownika";
            this.btnDodajUzytkownika.Size = new System.Drawing.Size(23, 22);
            this.btnDodajUzytkownika.Text = "Dodaj użytkownika do profilu";
            this.btnDodajUzytkownika.Click += new System.EventHandler(this.btnDodajUzytkownika_Click);
            // 
            // btnUsunUzytkownika
            // 
            this.btnUsunUzytkownika.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUsunUzytkownika.Enabled = false;
            this.btnUsunUzytkownika.Image = global::UniwersalnyDesktop.Properties.Resources.DeleteUser_16x;
            this.btnUsunUzytkownika.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUsunUzytkownika.Name = "btnUsunUzytkownika";
            this.btnUsunUzytkownika.Size = new System.Drawing.Size(23, 22);
            this.btnUsunUzytkownika.Text = "Usuń użytkownika z profilu";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(66, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "logo";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Location = new System.Drawing.Point(12, 141);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(150, 102);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxLogo.TabIndex = 23;
            this.pictureBoxLogo.TabStop = false;
            // 
            // dgvUzytkownik
            // 
            this.dgvUzytkownik.AllowUserToAddRows = false;
            this.dgvUzytkownik.AllowUserToDeleteRows = false;
            this.dgvUzytkownik.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUzytkownik.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.colNazwaU});
            this.dgvUzytkownik.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvUzytkownik.Location = new System.Drawing.Point(426, 58);
            this.dgvUzytkownik.Name = "dgvUzytkownik";
            this.dgvUzytkownik.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUzytkownik.Size = new System.Drawing.Size(262, 380);
            this.dgvUzytkownik.TabIndex = 24;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // colNazwaU
            // 
            this.colNazwaU.HeaderText = "Użytkownik";
            this.colNazwaU.Name = "colNazwaU";
            this.colNazwaU.Width = 200;
            // 
            // Serwer
            // 
            this.Serwer.AutoSize = true;
            this.Serwer.Location = new System.Drawing.Point(13, 58);
            this.Serwer.Name = "Serwer";
            this.Serwer.Size = new System.Drawing.Size(35, 13);
            this.Serwer.TabIndex = 25;
            this.Serwer.Text = "label2";
            // 
            // labelSerwer
            // 
            this.labelSerwer.AutoSize = true;
            this.labelSerwer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSerwer.Location = new System.Drawing.Point(16, 84);
            this.labelSerwer.Name = "labelSerwer";
            this.labelSerwer.Size = new System.Drawing.Size(87, 13);
            this.labelSerwer.TabIndex = 26;
            this.labelSerwer.Text = "nie zdefiniowy";
            // 
            // ProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 450);
            this.Controls.Add(this.labelSerwer);
            this.Controls.Add(this.Serwer);
            this.Controls.Add(this.dgvUzytkownik);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dgvProfileApps);
            this.Controls.Add(this.cbProfiles);
            this.Controls.Add(this.label1);
            this.Name = "ProfileEditor";
            this.Text = "Zarządzanie profilami";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileApps)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUzytkownik)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvProfileApps;
        private System.Windows.Forms.ComboBox cbProfiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnUsunAplikacje;
        private System.Windows.Forms.ToolStripButton btnDodajAplikacje;
        private System.Windows.Forms.ToolStripButton btnDodajProfil;
        private System.Windows.Forms.ToolStripButton btnUsunProfil;
        private System.Windows.Forms.ToolStripButton btnEdytujProfil;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnDodajUzytkownika;
        private System.Windows.Forms.ToolStripButton btnUsunUzytkownika;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton btnZapisz;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.DataGridView dgvUzytkownik;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNazwa;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNazwaU;
        private System.Windows.Forms.Label Serwer;
        private System.Windows.Forms.Label labelSerwer;
    }
}