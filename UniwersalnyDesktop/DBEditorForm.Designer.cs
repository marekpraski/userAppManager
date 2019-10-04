﻿namespace UniwersalnyDesktop
{
       
        partial class DBEditorForm
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
            this.baseDataGridview = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.undoButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadNextButton = new System.Windows.Forms.Button();
            this.remainingRowsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridview)).BeginInit();
            this.SuspendLayout();
            // 
            // baseDataGridview
            // 
            this.baseDataGridview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.baseDataGridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.baseDataGridview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.baseDataGridview.Location = new System.Drawing.Point(13, 13);
            this.baseDataGridview.Name = "baseDataGridview";
            this.baseDataGridview.Size = new System.Drawing.Size(444, 425);
            this.baseDataGridview.TabIndex = 0;
            this.baseDataGridview.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.baseDataGridView_CellBeginEdit);
            this.baseDataGridview.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.baseDataGridView_CellClick);
            this.baseDataGridview.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.baseDataGridView_CellEndEdit);
            this.baseDataGridview.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.baseDataGridView_RowHeaderMouseClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            // 
            // undoButton
            // 
            this.undoButton.Location = new System.Drawing.Point(466, 65);
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(75, 23);
            this.undoButton.TabIndex = 2;
            this.undoButton.Text = "cofnij";
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(466, 95);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "zapisz";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadNextButton
            // 
            this.loadNextButton.Location = new System.Drawing.Point(466, 412);
            this.loadNextButton.Name = "loadNextButton";
            this.loadNextButton.Size = new System.Drawing.Size(75, 23);
            this.loadNextButton.TabIndex = 4;
            this.loadNextButton.Text = "button4";
            this.loadNextButton.UseVisualStyleBackColor = true;
            this.loadNextButton.Click += new System.EventHandler(this.LoadNextButton_Click);
            // 
            // remainingRowsLabel
            // 
            this.remainingRowsLabel.AutoSize = true;
            this.remainingRowsLabel.Location = new System.Drawing.Point(466, 393);
            this.remainingRowsLabel.Name = "remainingRowsLabel";
            this.remainingRowsLabel.Size = new System.Drawing.Size(35, 13);
            this.remainingRowsLabel.TabIndex = 5;
            this.remainingRowsLabel.Text = "label1";
            // 
            // DBEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 450);
            this.Controls.Add(this.remainingRowsLabel);
            this.Controls.Add(this.loadNextButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.undoButton);
            this.Controls.Add(this.baseDataGridview);
            this.Name = "DBEditorForm";
            this.Text = "DBEditorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DBEditorForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.DataGridView baseDataGridview;
        protected System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        protected System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        protected System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        protected System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        protected System.Windows.Forms.Button undoButton;
        protected System.Windows.Forms.Button saveButton;
        protected System.Windows.Forms.Button loadNextButton;
        protected System.Windows.Forms.Label remainingRowsLabel;
    }
}