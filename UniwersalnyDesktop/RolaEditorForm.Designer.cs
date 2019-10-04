namespace UniwersalnyDesktop
{
    partial class RolaEditorForm
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
            this.appModuleListView = new System.Windows.Forms.ListView();
            this.headerAppModule = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerModuleAccess = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // appModuleListView
            // 
            this.appModuleListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.appModuleListView.CheckBoxes = true;
            this.appModuleListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerAppModule,
            this.headerModuleAccess});
            this.appModuleListView.HideSelection = false;
            this.appModuleListView.Location = new System.Drawing.Point(554, 13);
            this.appModuleListView.Name = "appModuleListView";
            this.appModuleListView.Size = new System.Drawing.Size(333, 422);
            this.appModuleListView.TabIndex = 6;
            this.appModuleListView.UseCompatibleStateImageBehavior = false;
            this.appModuleListView.View = System.Windows.Forms.View.Details;
            // 
            // headerAppModule
            // 
            this.headerAppModule.Text = "moduły aplikacji";
            this.headerAppModule.Width = 232;
            // 
            // headerModuleAccess
            // 
            this.headerModuleAccess.Text = "uprawnienia";
            this.headerModuleAccess.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.headerModuleAccess.Width = 96;
            // 
            // RolaEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 450);
            this.Controls.Add(this.appModuleListView);
            this.Name = "RolaEditorForm";
            this.Text = "DBRolaEditorForm";
            this.Controls.SetChildIndex(this.undoButton, 0);
            this.Controls.SetChildIndex(this.saveButton, 0);
            this.Controls.SetChildIndex(this.loadNextButton, 0);
            this.Controls.SetChildIndex(this.remainingRowsLabel, 0);
            this.Controls.SetChildIndex(this.appModuleListView, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView appModuleListView;
        private System.Windows.Forms.ColumnHeader headerAppModule;
        private System.Windows.Forms.ColumnHeader headerModuleAccess;
    }
}