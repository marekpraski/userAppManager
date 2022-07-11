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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.narzędziaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edycjaListyPlikowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konfigurujSciezkiMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zmienHasloMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSoftmine = new System.Windows.Forms.TabPage();
            this.tabBentley = new System.Windows.Forms.TabPage();
            this.btnMicroModeler3D = new System.Windows.Forms.Button();
            this.btnMicrostation2 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabBentley.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.narzędziaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(387, 24);
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
            // edycjaListyPlikowMenuItem
            // 
            this.edycjaListyPlikowMenuItem.Name = "edycjaListyPlikowMenuItem";
            this.edycjaListyPlikowMenuItem.Size = new System.Drawing.Size(230, 22);
            this.edycjaListyPlikowMenuItem.Text = "Edycja listy plików *dgn";
            // 
            // konfigurujSciezkiMenuItem
            // 
            this.konfigurujSciezkiMenuItem.Name = "konfigurujSciezkiMenuItem";
            this.konfigurujSciezkiMenuItem.Size = new System.Drawing.Size(230, 22);
            this.konfigurujSciezkiMenuItem.Text = "Konfiguruj ścieżki do aplikacji";
            // 
            // zmienHasloMenuItem
            // 
            this.zmienHasloMenuItem.Name = "zmienHasloMenuItem";
            this.zmienHasloMenuItem.Size = new System.Drawing.Size(230, 22);
            this.zmienHasloMenuItem.Text = "Zmień hasło";
            this.zmienHasloMenuItem.Click += new System.EventHandler(this.zmienHasloMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSoftmine);
            this.tabControl1.Controls.Add(this.tabBentley);
            this.tabControl1.Location = new System.Drawing.Point(13, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(368, 437);
            this.tabControl1.TabIndex = 2;
            // 
            // tabSoftmine
            // 
            this.tabSoftmine.Location = new System.Drawing.Point(4, 22);
            this.tabSoftmine.Name = "tabSoftmine";
            this.tabSoftmine.Padding = new System.Windows.Forms.Padding(3);
            this.tabSoftmine.Size = new System.Drawing.Size(360, 411);
            this.tabSoftmine.TabIndex = 0;
            this.tabSoftmine.Text = "SoftMine";
            this.tabSoftmine.UseVisualStyleBackColor = true;
            // 
            // tabBentley
            // 
            this.tabBentley.Controls.Add(this.btnMicrostation2);
            this.tabBentley.Controls.Add(this.btnMicroModeler3D);
            this.tabBentley.Location = new System.Drawing.Point(4, 22);
            this.tabBentley.Name = "tabBentley";
            this.tabBentley.Padding = new System.Windows.Forms.Padding(3);
            this.tabBentley.Size = new System.Drawing.Size(360, 511);
            this.tabBentley.TabIndex = 1;
            this.tabBentley.Text = "Bentley";
            this.tabBentley.UseVisualStyleBackColor = true;
            // 
            // btnMicroModeler3D
            // 
            this.btnMicroModeler3D.Location = new System.Drawing.Point(6, 6);
            this.btnMicroModeler3D.Name = "btnMicroModeler3D";
            this.btnMicroModeler3D.Size = new System.Drawing.Size(170, 58);
            this.btnMicroModeler3D.TabIndex = 49;
            this.btnMicroModeler3D.Text = "MicroStation V8i\r\n(Modeler3D/Modeler2D)";
            this.btnMicroModeler3D.UseVisualStyleBackColor = true;
            this.btnMicroModeler3D.Click += new System.EventHandler(this.btnMicroModeler3D_Click);
            // 
            // btnMicrostation2
            // 
            this.btnMicrostation2.Location = new System.Drawing.Point(182, 6);
            this.btnMicrostation2.Name = "btnMicrostation2";
            this.btnMicrostation2.Size = new System.Drawing.Size(170, 58);
            this.btnMicrostation2.TabIndex = 50;
            this.btnMicrostation2.Text = "MicroStation\r\n(bez aplikacji SoftMine)";
            this.btnMicrostation2.UseVisualStyleBackColor = true;
            this.btnMicrostation2.Click += new System.EventHandler(this.btnMicrostation2_Click);
            // 
            // DesktopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 477);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DesktopForm";
            this.Text = "Desktop";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DesktopForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabBentley.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem narzędziaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem edycjaListyPlikowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem konfigurujSciezkiMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zmienHasloMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSoftmine;
        private System.Windows.Forms.TabPage tabBentley;
        private System.Windows.Forms.Button btnMicrostation2;
        private System.Windows.Forms.Button btnMicroModeler3D;
    }
}