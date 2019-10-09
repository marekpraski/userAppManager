namespace UniwersalnyDesktop
{
    partial class AppEditorForm
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
            this.moduleDatagrid = new UniwersalnyDesktop.EditableDatagridControl();
            this.SuspendLayout();
            // 
            // moduleDatagrid
            // 
            this.moduleDatagrid.Location = new System.Drawing.Point(551, 13);
            this.moduleDatagrid.Name = "moduleDatagrid";
            this.moduleDatagrid.Size = new System.Drawing.Size(359, 433);
            this.moduleDatagrid.TabIndex = 6;
            // 
            // AppEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 450);
            this.Controls.Add(this.moduleDatagrid);
            this.Name = "AppEditorForm";
            this.Text = "Edytor aplikacji";
            this.Controls.SetChildIndex(this.undoButton, 0);
            this.Controls.SetChildIndex(this.saveButton, 0);
            this.Controls.SetChildIndex(this.loadNextButton, 0);
            this.Controls.SetChildIndex(this.remainingRowsLabel, 0);
            this.Controls.SetChildIndex(this.moduleDatagrid, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EditableDatagridControl moduleDatagrid;
    }
}