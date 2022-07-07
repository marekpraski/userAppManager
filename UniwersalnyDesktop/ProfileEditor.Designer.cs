﻿
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
            this.dgvProfileApps = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNazwa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSerwer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBazaDanych = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRaport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelDomena = new System.Windows.Forms.Label();
            this.labelLdap = new System.Windows.Forms.Label();
            this.btnDodajAplikacje = new System.Windows.Forms.ToolStripButton();
            this.btnUsunAplikacje = new System.Windows.Forms.ToolStripButton();
            this.btnDodajProfil = new System.Windows.Forms.ToolStripButton();
            this.btnUsunProfil = new System.Windows.Forms.ToolStripButton();
            this.btnEdytujProfil = new System.Windows.Forms.ToolStripButton();
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
            this.dgvProfileApps.Location = new System.Drawing.Point(12, 84);
            this.dgvProfileApps.Name = "dgvProfileApps";
            this.dgvProfileApps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfileApps.Size = new System.Drawing.Size(1048, 380);
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
            this.btnDodajAplikacje,
            this.btnUsunAplikacje,
            this.btnDodajProfil,
            this.btnEdytujProfil,
            this.btnUsunProfil});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1071, 25);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "domena";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(313, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "ldap";
            // 
            // labelDomena
            // 
            this.labelDomena.AutoSize = true;
            this.labelDomena.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelDomena.Location = new System.Drawing.Point(149, 62);
            this.labelDomena.Name = "labelDomena";
            this.labelDomena.Size = new System.Drawing.Size(41, 13);
            this.labelDomena.TabIndex = 18;
            this.labelDomena.Text = "label4";
            // 
            // labelLdap
            // 
            this.labelLdap.AutoSize = true;
            this.labelLdap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLdap.Location = new System.Drawing.Point(346, 62);
            this.labelLdap.Name = "labelLdap";
            this.labelLdap.Size = new System.Drawing.Size(41, 13);
            this.labelLdap.TabIndex = 19;
            this.labelLdap.Text = "label4";
            // 
            // btnDodajAplikacje
            // 
            this.btnDodajAplikacje.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
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
            this.btnUsunAplikacje.Image = global::UniwersalnyDesktop.Properties.Resources.LinkRemoved_16x;
            this.btnUsunAplikacje.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUsunAplikacje.Name = "btnUsunAplikacje";
            this.btnUsunAplikacje.Size = new System.Drawing.Size(23, 22);
            this.btnUsunAplikacje.Text = "Usuń aplikację z profilu";
            this.btnUsunAplikacje.Click += new System.EventHandler(this.btnUsunAplikacje_Click);
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
            // btnUsunProfil
            // 
            this.btnUsunProfil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUsunProfil.Image = global::UniwersalnyDesktop.Properties.Resources.delete;
            this.btnUsunProfil.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUsunProfil.Name = "btnUsunProfil";
            this.btnUsunProfil.Size = new System.Drawing.Size(23, 22);
            this.btnUsunProfil.Text = "Usuń profil";
            // 
            // btnEdytujProfil
            // 
            this.btnEdytujProfil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEdytujProfil.Image = global::UniwersalnyDesktop.Properties.Resources.Edit_16x;
            this.btnEdytujProfil.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdytujProfil.Name = "btnEdytujProfil";
            this.btnEdytujProfil.Size = new System.Drawing.Size(23, 22);
            this.btnEdytujProfil.Text = "Edytuj profil";
            this.btnEdytujProfil.Click += new System.EventHandler(this.btnEdytujProfil_Click);
            // 
            // ProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 473);
            this.Controls.Add(this.labelLdap);
            this.Controls.Add(this.labelDomena);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dgvProfileApps);
            this.Controls.Add(this.cbProfiles);
            this.Controls.Add(this.label1);
            this.Name = "ProfileEditor";
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
        private System.Windows.Forms.ToolStripButton btnUsunAplikacje;
        private System.Windows.Forms.ToolStripButton btnDodajAplikacje;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNazwa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSerwer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBazaDanych;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRaport;
        private System.Windows.Forms.ToolStripButton btnDodajProfil;
        private System.Windows.Forms.ToolStripButton btnUsunProfil;
        private System.Windows.Forms.ToolStripButton btnEdytujProfil;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelDomena;
        private System.Windows.Forms.Label labelLdap;
    }
}