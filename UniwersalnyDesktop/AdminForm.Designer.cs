namespace UniwersalnyDesktop
{
    partial class AdminForm
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
            this.userTreeView = new System.Windows.Forms.TreeView();
            this.appListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.appRoleListView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // userTreeView
            // 
            this.userTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.userTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.userTreeView.Location = new System.Drawing.Point(12, 30);
            this.userTreeView.Name = "userTreeView";
            this.userTreeView.Size = new System.Drawing.Size(295, 325);
            this.userTreeView.TabIndex = 0;
            this.userTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.userTreeView_AfterSelect);
            this.userTreeView.Click += new System.EventHandler(this.userTreeView_Click);
            this.userTreeView.Leave += new System.EventHandler(this.TreeView1_Leave);
            // 
            // appListView
            // 
            this.appListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.appListView.CheckBoxes = true;
            this.appListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.appListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.appListView.Location = new System.Drawing.Point(313, 30);
            this.appListView.MultiSelect = false;
            this.appListView.Name = "appListView";
            this.appListView.Size = new System.Drawing.Size(178, 325);
            this.appListView.TabIndex = 2;
            this.appListView.UseCompatibleStateImageBehavior = false;
            this.appListView.View = System.Windows.Forms.View.Details;
            this.appListView.Click += new System.EventHandler(this.AppListView_Click);
            this.appListView.Leave += new System.EventHandler(this.AppListView_Leave);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "nazwa";
            this.columnHeader1.Width = 173;
            // 
            // appRoleListView
            // 
            this.appRoleListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.appRoleListView.CheckBoxes = true;
            this.appRoleListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.appRoleListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.appRoleListView.Location = new System.Drawing.Point(497, 30);
            this.appRoleListView.MultiSelect = false;
            this.appRoleListView.Name = "appRoleListView";
            this.appRoleListView.Size = new System.Drawing.Size(257, 325);
            this.appRoleListView.TabIndex = 2;
            this.appRoleListView.UseCompatibleStateImageBehavior = false;
            this.appRoleListView.View = System.Windows.Forms.View.Details;
            this.appRoleListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.AppRoleListView_ItemChecked);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "rola";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "opis";
            this.columnHeader3.Width = 150;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "użytkownicy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(310, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "aplikacje";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(494, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "role aplikacji";
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 373);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.appListView);
            this.Controls.Add(this.appRoleListView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userTreeView);
            this.Name = "AdminForm";
            this.Text = "Panel Administratora";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView userTreeView;
        private System.Windows.Forms.ListView appListView;
        private System.Windows.Forms.ListView appRoleListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}