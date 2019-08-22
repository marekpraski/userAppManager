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
    public partial class AdminForm : Form
    {
        private TreeNode previousSelectedNode = null;

        public AdminForm()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        public void populateTreeview(List<string> items)
        {
            for (int k = 0; k < 2; k++)
            {
                TreeNode[] childNodes = new TreeNode[items.Count];
                for (int i = 0; i < items.Count; i++)
                {
                    TreeNode treeNode = new TreeNode(items[i]);
                    childNodes[i] = treeNode;
                }

                TreeNode parentNode = new TreeNode("nodes"+k, childNodes);
                treeView1.Nodes.Add(parentNode);
            }
        }

        public void populateListview(List<string> rows)
        {
            foreach (string row in rows)
            {
                ListViewItem listRow = new ListViewItem(row);
                listView1.Items.Add(listRow);
            }
        }

            public void populateListbox(List<string> rows)
            {
            listBox1.DataSource = rows;
            
            }

       //podświetla zaznaczony wiersz w drzewie na wybrany kolor gdy użytkownik kliknie w inny obiekt w formularzu
       //domyślnie ten kolor jest bladoszary, dla mnie zbyt niewidoczny
        private void TreeView1_Leave(object sender, EventArgs e)
        {
            treeView1.SelectedNode.BackColor = System.Drawing.Color.Aqua;
            previousSelectedNode = treeView1.SelectedNode;
        }

        //zmienia kolor poprzednio zaznaczonego wiersza w drzewie na biały
        private void AdminForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (previousSelectedNode != null)
            {
                previousSelectedNode.BackColor = treeView1.BackColor;
            }
            
        }

       

        
    }
}
