using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public partial class Form1 : Form
    {


        string[] items = { "aaa", "bbb" , "cccc", "dddd" };
        string[] items2 = { "qq", "ww" , "ee", "rr" };
        bool[] items3 = { true, false, false, true };

        bool mouseClicked = false;

        public static int testInt { get; set; }
        private int i = 1;

        TreeNode selectedNow = null;
        TreeNode selectedPrevious = null;
        TreeNode backupNode = null;
        public Form1()
        {
            InitializeComponent();
            populateGridview();

            populateTreeview();
        }

        private void populateGridview()
        {


            for (int i=0; i<items2.Length; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = items[i];
                dataGridView1.Rows[i].Cells[1].Value = items2[i];
                dataGridView1.Rows[i].Cells[2].Value = items[i];

            }
            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
            dataGridView1.Columns.Insert(1, col);

            //dataGridView1.Columns[0].Visible = false;

            // Create and initialize a CheckBox.   
            CheckBox checkBox1 = new CheckBox();

            // Make the check box control appear as a toggle button.
            checkBox1.Appearance = Appearance.Normal;

            // Turn off the update of the display on the click of the control.
            checkBox1.AutoCheck = true;

            // Add the check box control to the form.
            Controls.Add(checkBox1);
        }

       

        public void populateTreeview()
        {
            TreeNode[] childNodes = new TreeNode[items.Length];
            int i = 0;
            foreach (string item in items)
            {
                
                TreeNode treeNode = new TreeNode(item);
                childNodes[i] = treeNode;
                i++;
            }
            TreeNode parentNode = new TreeNode("zduplikowane loginy", childNodes);

            treeView1.Nodes.Add(parentNode);
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(mouseClicked)
            {
                MyMessageBox.display("after select");
                mouseClicked = false;
            }
            
        }

 
        

        private void TreeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (mouseClicked)
            {
                MyMessageBox.display("before select");
            }
        }

        private void TreeView1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }



        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            MyMessageBox.display("mouse down");
            mouseClicked = true;

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //editableDatagridControl1.datagridColumnHide(0);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //editableDatagridControl1.datagridColumnShow(0);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            string[] s1 = { "aa", "bb" };
            string[] s2 = { "cc", "dd" };
            string[] s3 = { "1c", "dd" };
            string[] s4 = { "2c", "dd" };
            List<string[]> dataList = new List<string[]>();
            dataList.Add(s1);
            dataList.Add(s2);
            dataList.Add(s3);
            dataList.Add(s4);
            //editableDatagridControl1.populateDatagrid(dataList);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            List<bool> boolList = new List<bool>();
            boolList.Add(false);
            boolList.Add(true);
            boolList.Add(true);
            boolList.Add(false);
            //editableDatagridControl1.addCheckboxColumn(boolList);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
         
            this.textBox1.Text = i.ToString();
            i++;
            Class1 cl1 = new Class1();
            cl1.timerEvent += fm2OnttEvent;
            cl1.timer();

        }

        private void fm2OnttEvent(object sender, TimerEventArgs args)
        {
            this.textBox1.Text = args.time.ToString();
            this.dataGridView1.Rows.Add(args.time.ToString());
            this.Refresh();
        }

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell dgCell =  dataGridView1.SelectedCells[0];
            int x = dgCell.RowIndex;
            int y = dgCell.ColumnIndex;
            MyMessageBox.display("begin edit row " + x + " column " +y);
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell dgCell = dataGridView1.SelectedCells[0];
            int x = dgCell.RowIndex;
            int y = dgCell.ColumnIndex;
            MyMessageBox.display("end edit " + x + " column " + y);
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //DataGridViewCell dgCell = dataGridView1.SelectedCells[0];
            //int x = dgCell.RowIndex;
            //int y = dgCell.ColumnIndex;
            //MyMessageBox.display("value changed  " + x + " column " + y);        
        }

        private void DataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCell dgCell = dataGridView1.SelectedCells[0];
            int x = dgCell.RowIndex;
            int y = dgCell.ColumnIndex;
            MyMessageBox.display("mouse up " + x + " column " + y);
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell dgCell = dataGridView1.SelectedCells[0];
            int x = dgCell.RowIndex;
            int y = dgCell.ColumnIndex;
            MyMessageBox.display("cell click " + x + " column " + y);
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            DataGridViewCell dgCell = dataGridView1.SelectedCells[0];
            int x = dgCell.RowIndex;
            int y = dgCell.ColumnIndex;
            MyMessageBox.display("checked changed " + x + " column " + y);
        }

        private void CheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            DataGridViewCell dgCell = dataGridView1.SelectedCells[0];
            int x = dgCell.RowIndex;
            int y = dgCell.ColumnIndex;
            MyMessageBox.display("checked state changed " + x + " column " + y);
        }

        private void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCell dgCell = dataGridView1.SelectedCells[0];
            int x = dgCell.RowIndex;
            int y = dgCell.ColumnIndex;
            MyMessageBox.display("cell mouse clicked " + x + " column " + y);
        }

    }
}
