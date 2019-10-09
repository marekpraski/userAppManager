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


        string[] items = { "aaa", "bbb", "cccc", "dddd" };

        bool mouseClicked = false;

        public static int testInt { get; set; }

        TreeNode selectedNow = null;
        TreeNode selectedPrevious = null;
        TreeNode backupNode = null;
        public Form1()
        {
            InitializeComponent();
            populateTreeview();
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

        private void changeControlLayout()
        {
            editableDatagridControl1.saveButtonDisable();
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


        private void Button1_Click(object sender, EventArgs e)
        {
            editableDatagridControl1.saveButtonEnable();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            editableDatagridControl1.saveButtonDisable();


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
    }
}
