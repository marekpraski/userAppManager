namespace UniwersalnyDesktop
{
    partial class DesktopForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.narzędziaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zmienHasloMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konfigurujSciezkiMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edycjaListyPlikowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(80, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "brak połączenia z bazą danych";
            this.label1.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.narzędziaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(468, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // narzędziaToolStripMenuItem
            // 
            this.narzędziaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.edycjaListyPlikowMenuItem,
            this.konfigurujSciezkiMenuItem,
            this.zmienHasloMenuItem});
            this.narzędziaToolStripMenuItem.Name = "narzędziaToolStripMenuItem";
            this.narzędziaToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.narzędziaToolStripMenuItem.Text = "Narzędzia";
            // 
            // zmienHasloMenuItem
            // 
            this.zmienHasloMenuItem.Name = "zmienHasloMenuItem";
            this.zmienHasloMenuItem.Size = new System.Drawing.Size(230, 22);
            this.zmienHasloMenuItem.Text = "Zmień hasło";
            this.zmienHasloMenuItem.Click += new System.EventHandler(this.zmienHasloMenuItem_Click);
            // 
            // konfigurujSciezkiMenuItem
            // 
            this.konfigurujSciezkiMenuItem.Name = "konfigurujSciezkiMenuItem";
            this.konfigurujSciezkiMenuItem.Size = new System.Drawing.Size(230, 22);
            this.konfigurujSciezkiMenuItem.Text = "Konfiguruj ścieżki do aplikacji";
            // 
            // edycjaListyPlikowMenuItem
            // 
            this.edycjaListyPlikowMenuItem.Name = "edycjaListyPlikowMenuItem";
            this.edycjaListyPlikowMenuItem.Size = new System.Drawing.Size(230, 22);
            this.edycjaListyPlikowMenuItem.Text = "Edycja listy plików *dgn";
            // 
            // DesktopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 135);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DesktopForm";
            this.Text = "Desktop";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DesktopForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem narzędziaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem edycjaListyPlikowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem konfigurujSciezkiMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zmienHasloMenuItem;
    }
}