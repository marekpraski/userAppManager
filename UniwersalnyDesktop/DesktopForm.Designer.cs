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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesktopForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSoftmine = new System.Windows.Forms.TabPage();
            this.tabBentley = new System.Windows.Forms.TabPage();
            this.btnMicrostation2 = new System.Windows.Forms.Button();
            this.btnMicroModeler3D = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cbProfile = new System.Windows.Forms.ComboBox();
            this.btnZmienHaslo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.tabControl1.SuspendLayout();
            this.tabBentley.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.tabBentley.Size = new System.Drawing.Size(360, 411);
            this.tabBentley.TabIndex = 1;
            this.tabBentley.Text = "Bentley";
            this.tabBentley.UseVisualStyleBackColor = true;
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
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZmienHaslo,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(387, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cbProfile
            // 
            this.cbProfile.FormattingEnabled = true;
            this.cbProfile.Location = new System.Drawing.Point(83, 1);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(250, 21);
            this.cbProfile.TabIndex = 4;
            this.cbProfile.SelectedIndexChanged += new System.EventHandler(this.cbProfile_SelectedIndexChanged);
            // 
            // btnZmienHaslo
            // 
            this.btnZmienHaslo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZmienHaslo.Image = global::UniwersalnyDesktop.Properties.Resources.ChangePassword_16x;
            this.btnZmienHaslo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZmienHaslo.Name = "btnZmienHaslo";
            this.btnZmienHaslo.Size = new System.Drawing.Size(23, 22);
            this.btnZmienHaslo.Text = "Zmień hasło";
            this.btnZmienHaslo.Click += new System.EventHandler(this.btnZmienHaslo_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // DesktopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 477);
            this.Controls.Add(this.cbProfile);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "DesktopForm";
            this.Text = "Desktop";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DesktopForm_FormClosed);
            this.Load += new System.EventHandler(this.DesktopForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabBentley.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSoftmine;
        private System.Windows.Forms.TabPage tabBentley;
        private System.Windows.Forms.Button btnMicrostation2;
        private System.Windows.Forms.Button btnMicroModeler3D;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnZmienHaslo;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ComboBox cbProfile;
    }
}