namespace UniwersalnyDesktop
{
    public partial class UserEditorForm
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
            this.btnNowy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNowy
            // 
            this.btnNowy.Location = new System.Drawing.Point(466, 13);
            this.btnNowy.Name = "btnNowy";
            this.btnNowy.Size = new System.Drawing.Size(75, 23);
            this.btnNowy.TabIndex = 6;
            this.btnNowy.Text = "Nowy";
            this.btnNowy.UseVisualStyleBackColor = true;
            this.btnNowy.Click += new System.EventHandler(this.btnNowy_Click);
            // 
            // UserEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 488);
            this.Controls.Add(this.btnNowy);
            this.Name = "UserEditorForm";
            this.Text = "Edytor użytkowników";
            this.Load += new System.EventHandler(this.UserEditorForm_Load);
            this.Controls.SetChildIndex(this.undoButton, 0);
            this.Controls.SetChildIndex(this.saveButton, 0);
            this.Controls.SetChildIndex(this.loadNextButton, 0);
            this.Controls.SetChildIndex(this.remainingRowsLabel, 0);
            this.Controls.SetChildIndex(this.btnNowy, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNowy;
    }
}