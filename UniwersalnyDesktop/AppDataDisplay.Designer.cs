namespace UniwersalnyDesktop
{
    partial class AppDataDisplay
    {
        /// <summary> 
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod wygenerowany przez Projektanta składników

        /// <summary> 
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować 
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.appNameLabel = new System.Windows.Forms.Label();
            this.newRolaName = new System.Windows.Forms.Label();
            this.newRolaDesc = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.oldRolaName = new System.Windows.Forms.Label();
            this.oldRolaDesc = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // appNameLabel
            // 
            this.appNameLabel.AutoSize = true;
            this.appNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.appNameLabel.Location = new System.Drawing.Point(115, 0);
            this.appNameLabel.Name = "appNameLabel";
            this.appNameLabel.Size = new System.Drawing.Size(106, 15);
            this.appNameLabel.TabIndex = 1;
            this.appNameLabel.Text = "nazwa aplikacji";
            // 
            // newRolaName
            // 
            this.newRolaName.AutoSize = true;
            this.newRolaName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.newRolaName.Location = new System.Drawing.Point(3, 18);
            this.newRolaName.Name = "newRolaName";
            this.newRolaName.Size = new System.Drawing.Size(77, 13);
            this.newRolaName.TabIndex = 3;
            this.newRolaName.Text = "newRolaName";
            // 
            // newRolaDesc
            // 
            this.newRolaDesc.AutoSize = true;
            this.newRolaDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.newRolaDesc.Location = new System.Drawing.Point(3, 35);
            this.newRolaDesc.Name = "newRolaDesc";
            this.newRolaDesc.Size = new System.Drawing.Size(74, 13);
            this.newRolaDesc.TabIndex = 5;
            this.newRolaDesc.Text = "newRolaDesc";
            this.newRolaDesc.MouseHover += new System.EventHandler(this.NewRolaDesc_MouseHover);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.ForeColor = System.Drawing.Color.Red;
            this.statusLabel.Location = new System.Drawing.Point(133, 79);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(61, 13);
            this.statusLabel.TabIndex = 7;
            this.statusLabel.Text = "statusLabel";
            // 
            // oldRolaName
            // 
            this.oldRolaName.AutoSize = true;
            this.oldRolaName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.oldRolaName.Location = new System.Drawing.Point(6, 18);
            this.oldRolaName.Name = "oldRolaName";
            this.oldRolaName.Size = new System.Drawing.Size(70, 13);
            this.oldRolaName.TabIndex = 8;
            this.oldRolaName.Text = "old rola name";
            // 
            // oldRolaDesc
            // 
            this.oldRolaDesc.AutoSize = true;
            this.oldRolaDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.oldRolaDesc.Location = new System.Drawing.Point(6, 35);
            this.oldRolaDesc.Name = "oldRolaDesc";
            this.oldRolaDesc.Size = new System.Drawing.Size(67, 13);
            this.oldRolaDesc.TabIndex = 9;
            this.oldRolaDesc.Text = "old rola desc";
            this.oldRolaDesc.MouseHover += new System.EventHandler(this.OldRolaDesc_MouseHover);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.newRolaDesc);
            this.groupBox1.Controls.Add(this.newRolaName);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox1.Location = new System.Drawing.Point(3, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 53);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "rola po zmianie";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.oldRolaName);
            this.groupBox2.Controls.Add(this.oldRolaDesc);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox2.Location = new System.Drawing.Point(167, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(160, 53);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "rola przed zmianą";
            // 
            // AppDataDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.appNameLabel);
            this.Name = "AppDataDisplay";
            this.Size = new System.Drawing.Size(330, 100);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label appNameLabel;
        private System.Windows.Forms.Label newRolaName;
        private System.Windows.Forms.Label newRolaDesc;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label oldRolaName;
        private System.Windows.Forms.Label oldRolaDesc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
