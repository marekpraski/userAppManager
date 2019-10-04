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
            this.appModuleGridview = new System.Windows.Forms.DataGridView();
            this.moduleNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.appModuleGridview)).BeginInit();
            this.SuspendLayout();
            // 
            // appModuleGridview
            // 
            this.appModuleGridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.appModuleGridview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.moduleNameColumn});
            this.appModuleGridview.Location = new System.Drawing.Point(560, 13);
            this.appModuleGridview.Name = "appModuleGridview";
            this.appModuleGridview.Size = new System.Drawing.Size(244, 422);
            this.appModuleGridview.TabIndex = 6;
            // 
            // moduleNameColumn
            // 
            this.moduleNameColumn.HeaderText = "nazwa modułu";
            this.moduleNameColumn.Name = "moduleNameColumn";
            this.moduleNameColumn.Width = 200;
            // 
            // AppEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 450);
            this.Controls.Add(this.appModuleGridview);
            this.Name = "AppEditorForm";
            this.Text = "Edytor aplikacji";
            this.Controls.SetChildIndex(this.undoButton, 0);
            this.Controls.SetChildIndex(this.saveButton, 0);
            this.Controls.SetChildIndex(this.loadNextButton, 0);
            this.Controls.SetChildIndex(this.remainingRowsLabel, 0);
            this.Controls.SetChildIndex(this.appModuleGridview, 0);
            ((System.ComponentModel.ISupportInitialize)(this.appModuleGridview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView appModuleGridview;
        private System.Windows.Forms.DataGridViewTextBoxColumn moduleNameColumn;
    }
}