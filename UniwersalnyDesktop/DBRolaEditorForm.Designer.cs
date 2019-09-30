namespace UniwersalnyDesktop
{
    partial class DBRolaEditorForm
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
            this.appModule = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.moduleAccess = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // appModuleListView
            // 
            this.appModuleListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.appModuleListView.CheckBoxes = true;
            this.appModuleListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.appModule,
            this.moduleAccess});
            this.appModuleListView.HideSelection = false;
            this.appModuleListView.Location = new System.Drawing.Point(554, 13);
            this.appModuleListView.Name = "appModuleListView";
            this.appModuleListView.Size = new System.Drawing.Size(242, 422);
            this.appModuleListView.TabIndex = 6;
            this.appModuleListView.UseCompatibleStateImageBehavior = false;
            this.appModuleListView.View = System.Windows.Forms.View.Details;
            // 
            // appModule
            // 
            this.appModule.Text = "moduł aplikacji";
            this.appModule.Width = 166;
            // 
            // moduleAccess
            // 
            this.moduleAccess.Text = "dostęp";
            this.moduleAccess.Width = 72;
            // 
            // DBRolaEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 450);
            this.Controls.Add(this.appModuleListView);
            this.Name = "DBRolaEditorForm";
            this.Text = "DBRolaEditorForm";
            this.Controls.SetChildIndex(this.appModuleListView, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView appModuleListView;
        private System.Windows.Forms.ColumnHeader appModule;
        private System.Windows.Forms.ColumnHeader moduleAccess;
    }
}