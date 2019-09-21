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
            treeView1.SelectedNode = backupNode;
            treeView1.SelectedNode.BackColor = Color.DodgerBlue;
            treeView1.SelectedNode.ForeColor = Color.White;
            textBox1.Text = selectedNow.Text;
            if (selectedPrevious != null)
            {
                textBox2.Text = backupNode.Text;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {



        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            MyMessageBox.display("mouse down");
            mouseClicked = true;

        }
    }
}
