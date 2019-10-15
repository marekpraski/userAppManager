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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
            this.userTreeView = new System.Windows.Forms.TreeView();
            this.appListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rolaListView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.saveAndCloseButton = new System.Windows.Forms.ToolStripButton();
            this.statusInformationButton = new System.Windows.Forms.ToolStripButton();
            this.refreshButton = new System.Windows.Forms.ToolStripButton();
            this.helpButton = new System.Windows.Forms.ToolStripButton();
            this.editAppsLabel = new System.Windows.Forms.Label();
            this.editRolaLabel = new System.Windows.Forms.Label();
            this.editUsersLabel = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // userTreeView
            // 
            this.userTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.userTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.userTreeView.Location = new System.Drawing.Point(12, 40);
            this.userTreeView.Name = "userTreeView";
            this.userTreeView.Size = new System.Drawing.Size(295, 383);
            this.userTreeView.TabIndex = 0;
            this.userTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.UserTreeView_BeforeSelect);
            this.userTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.userTreeView_AfterSelect);
            this.userTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.UserTreeView_NodeMouseClick);
            this.userTreeView.Leave += new System.EventHandler(this.userTreeView_Leave);
            // 
            // appListView
            // 
            this.appListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.appListView.CheckBoxes = true;
            this.appListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.appListView.Enabled = false;
            this.appListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.appListView.Location = new System.Drawing.Point(313, 40);
            this.appListView.MultiSelect = false;
            this.appListView.Name = "appListView";
            this.appListView.Size = new System.Drawing.Size(178, 383);
            this.appListView.TabIndex = 2;
            this.appListView.UseCompatibleStateImageBehavior = false;
            this.appListView.View = System.Windows.Forms.View.Details;
            this.appListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.AppListView_ItemChecked);
            this.appListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.AppListView_ItemSelectionChanged);
            this.appListView.Leave += new System.EventHandler(this.AppListView_Leave);
            this.appListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AppListView_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "nazwa";
            this.columnHeader1.Width = 173;
            // 
            // rolaListView
            // 
            this.rolaListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.rolaListView.CheckBoxes = true;
            this.rolaListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.rolaListView.Enabled = false;
            this.rolaListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rolaListView.HideSelection = false;
            this.rolaListView.Location = new System.Drawing.Point(497, 40);
            this.rolaListView.MultiSelect = false;
            this.rolaListView.Name = "rolaListView";
            this.rolaListView.ShowItemToolTips = true;
            this.rolaListView.Size = new System.Drawing.Size(257, 383);
            this.rolaListView.TabIndex = 2;
            this.rolaListView.UseCompatibleStateImageBehavior = false;
            this.rolaListView.View = System.Windows.Forms.View.Details;
            this.rolaListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.rolaListView_ItemChecked);
            this.rolaListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RolaListView_MouseClick);
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
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "użytkownicy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(310, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "aplikacje";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(494, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "role aplikacji";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveButton,
            this.saveAndCloseButton,
            this.statusInformationButton,
            this.refreshButton,
            this.helpButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(766, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButton.Enabled = false;
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(23, 22);
            this.saveButton.Text = "zapisz zmiany";
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // saveAndCloseButton
            // 
            this.saveAndCloseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveAndCloseButton.Enabled = false;
            this.saveAndCloseButton.Image = ((System.Drawing.Image)(resources.GetObject("saveAndCloseButton.Image")));
            this.saveAndCloseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveAndCloseButton.Name = "saveAndCloseButton";
            this.saveAndCloseButton.Size = new System.Drawing.Size(23, 22);
            this.saveAndCloseButton.Text = "zapisz zmiany i zamknij okno";
            this.saveAndCloseButton.Click += new System.EventHandler(this.SaveAndCloseButton_Click);
            // 
            // statusInformationButton
            // 
            this.statusInformationButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.statusInformationButton.Enabled = false;
            this.statusInformationButton.Image = ((System.Drawing.Image)(resources.GetObject("statusInformationButton.Image")));
            this.statusInformationButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.statusInformationButton.Name = "statusInformationButton";
            this.statusInformationButton.Size = new System.Drawing.Size(23, 22);
            this.statusInformationButton.Text = "pokaż zmiany";
            this.statusInformationButton.Click += new System.EventHandler(this.StatusInformationButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshButton.Image")));
            this.refreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(23, 22);
            this.refreshButton.Text = "odśwież";
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // helpButton
            // 
            this.helpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpButton.Image = ((System.Drawing.Image)(resources.GetObject("helpButton.Image")));
            this.helpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(23, 22);
            this.helpButton.Text = "pomoc";
            this.helpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // editAppsLabel
            // 
            this.editAppsLabel.AutoSize = true;
            this.editAppsLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.editAppsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.editAppsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.editAppsLabel.Location = new System.Drawing.Point(446, 24);
            this.editAppsLabel.Name = "editAppsLabel";
            this.editAppsLabel.Size = new System.Drawing.Size(35, 13);
            this.editAppsLabel.TabIndex = 9;
            this.editAppsLabel.Text = "edytuj";
            this.editAppsLabel.Click += new System.EventHandler(this.EditAppsLabel_Click);
            // 
            // editRolaLabel
            // 
            this.editRolaLabel.AutoSize = true;
            this.editRolaLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.editRolaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.editRolaLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.editRolaLabel.Location = new System.Drawing.Point(706, 24);
            this.editRolaLabel.Name = "editRolaLabel";
            this.editRolaLabel.Size = new System.Drawing.Size(35, 13);
            this.editRolaLabel.TabIndex = 10;
            this.editRolaLabel.Text = "edytuj";
            this.editRolaLabel.Click += new System.EventHandler(this.EditRolaLabel_Click);
            // 
            // editUsersLabel
            // 
            this.editUsersLabel.AutoSize = true;
            this.editUsersLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.editUsersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.editUsersLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.editUsersLabel.Location = new System.Drawing.Point(260, 23);
            this.editUsersLabel.Name = "editUsersLabel";
            this.editUsersLabel.Size = new System.Drawing.Size(35, 13);
            this.editUsersLabel.TabIndex = 11;
            this.editUsersLabel.Text = "edytuj";
            this.editUsersLabel.Click += new System.EventHandler(this.EditUsersLabel_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 441);
            this.Controls.Add(this.editUsersLabel);
            this.Controls.Add(this.editRolaLabel);
            this.Controls.Add(this.editAppsLabel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.appListView);
            this.Controls.Add(this.rolaListView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userTreeView);
            this.Name = "AdminForm";
            this.Text = "Panel Administratora";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdminForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView userTreeView;
        private System.Windows.Forms.ListView appListView;
        private System.Windows.Forms.ListView rolaListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.ToolStripButton helpButton;
        private System.Windows.Forms.ToolStripButton statusInformationButton;
        private System.Windows.Forms.ToolStripButton saveAndCloseButton;
        private System.Windows.Forms.Label editAppsLabel;
        private System.Windows.Forms.Label editRolaLabel;
        private System.Windows.Forms.ToolStripButton refreshButton;
        private System.Windows.Forms.Label editUsersLabel;
    }
}