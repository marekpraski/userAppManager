
namespace UniwersalnyDesktop
{
    partial class DesktopProfileEditor
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
            this.dgvProfileApps = new System.Windows.Forms.DataGridView();
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnZapisz = new System.Windows.Forms.ToolStripButton();
            this.btnDodaj = new System.Windows.Forms.ToolStripButton();
            this.btnUsun = new System.Windows.Forms.ToolStripButton();
            this.btnOdswiez = new System.Windows.Forms.ToolStripButton();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNazwa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSerwer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBazaDanych = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRaport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileApps)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvProfileApps
            // 
            this.dgvProfileApps.AllowUserToAddRows = false;
            this.dgvProfileApps.AllowUserToDeleteRows = false;
            this.dgvProfileApps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProfileApps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colNazwa,
            this.colSerwer,
            this.colBazaDanych,
            this.colRaport});
            this.dgvProfileApps.Location = new System.Drawing.Point(12, 74);
            this.dgvProfileApps.Name = "dgvProfileApps";
            this.dgvProfileApps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfileApps.Size = new System.Drawing.Size(1048, 449);
            this.dgvProfileApps.TabIndex = 9;
            // 
            // cbProfiles
            // 
            this.cbProfiles.FormattingEnabled = true;
            this.cbProfiles.Location = new System.Drawing.Point(101, 28);
            this.cbProfiles.Name = "cbProfiles";
            this.cbProfiles.Size = new System.Drawing.Size(916, 21);
            this.cbProfiles.TabIndex = 8;
            this.cbProfiles.SelectedIndexChanged += new System.EventHandler(this.cbProfiles_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Profile desktopu:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZapisz,
            this.btnDodaj,
            this.btnUsun,
            this.btnOdswiez});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1071, 25);
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
            this.btnZapisz.Text = "Zapisz profil";
            this.btnZapisz.Click += new System.EventHandler(this.btnZapisz_Click);
            // 
            // btnDodaj
            // 
            this.btnDodaj.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDodaj.Image = global::UniwersalnyDesktop.Properties.Resources.Add_16x;
            this.btnDodaj.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDodaj.Name = "btnDodaj";
            this.btnDodaj.Size = new System.Drawing.Size(23, 22);
            this.btnDodaj.Text = "Dodaj aplikacje do profilu";
            this.btnDodaj.Click += new System.EventHandler(this.btnDodaj_Click);
            // 
            // btnUsun
            // 
            this.btnUsun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUsun.Image = global::UniwersalnyDesktop.Properties.Resources.delete;
            this.btnUsun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUsun.Name = "btnUsun";
            this.btnUsun.Size = new System.Drawing.Size(23, 22);
            this.btnUsun.Text = "Usuń aplikację z profilu";
            this.btnUsun.Click += new System.EventHandler(this.btnUsun_Click);
            // 
            // btnOdswiez
            // 
            this.btnOdswiez.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOdswiez.Image = global::UniwersalnyDesktop.Properties.Resources.Refresh_16x;
            this.btnOdswiez.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOdswiez.Name = "btnOdswiez";
            this.btnOdswiez.Size = new System.Drawing.Size(23, 22);
            this.btnOdswiez.Text = "Odśwież profil";
            this.btnOdswiez.Click += new System.EventHandler(this.btnOdswiez_Click);
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
            // colSerwer
            // 
            this.colSerwer.HeaderText = "Serwer";
            this.colSerwer.Name = "colSerwer";
            this.colSerwer.Width = 150;
            // 
            // colBazaDanych
            // 
            this.colBazaDanych.HeaderText = "Baza danych";
            this.colBazaDanych.Name = "colBazaDanych";
            this.colBazaDanych.Width = 150;
            // 
            // colRaport
            // 
            this.colRaport.HeaderText = "Raport";
            this.colRaport.Name = "colRaport";
            this.colRaport.Width = 500;
            // 
            // DesktopProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 535);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dgvProfileApps);
            this.Controls.Add(this.cbProfiles);
            this.Controls.Add(this.label1);
            this.Name = "DesktopProfileEditor";
            this.Text = "Zarządzanie profilami";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileApps)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvProfileApps;
        private System.Windows.Forms.ComboBox cbProfiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnZapisz;
        private System.Windows.Forms.ToolStripButton btnUsun;
        private System.Windows.Forms.ToolStripButton btnOdswiez;
        private System.Windows.Forms.ToolStripButton btnDodaj;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNazwa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSerwer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBazaDanych;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRaport;
    }
}