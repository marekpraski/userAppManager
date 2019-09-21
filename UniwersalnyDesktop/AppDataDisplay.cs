using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public partial class AppDataDisplay : UserControl
    {
        public AppDataDisplay()
        {
            InitializeComponent();
        }

        public void setAppName (string appName)
        {
            this.appNameLabel.Text = appName;
        }

        public void setNewRolaName (string rolaName)
        {
            this.newRolaName.Text = rolaName;
        }

        public void setNewRolaDesc (string rolaDesc)
        {
            this.newRolaDesc.Text = rolaDesc;
        }

        public void setOldRolaName(string rolaName)
        {
            this.oldRolaName.Text = rolaName;
        }

        public void setOldRolaDesc(string rolaDesc)
        {
            this.oldRolaDesc.Text = rolaDesc;
        }

        public void setStatus (string status)
        {
             this.statusLabel.Text = status;
        }

        private void NewRolaDesc_MouseHover(object sender, EventArgs e)
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.InitialDelay = 200;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(newRolaDesc, newRolaDesc.Text);
        }

        private void OldRolaDesc_MouseHover(object sender, EventArgs e)
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.InitialDelay = 200;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(oldRolaDesc, oldRolaDesc.Text);
        }
    }
}
