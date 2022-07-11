using System;
using System.Windows.Forms;
using System.Reflection;

namespace UniwersalnyDesktop
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            int major = version.Major;
            int minor = version.Minor;
            int build = version.Build;
            int revision = version.Revision;
            label2.Text = "wersja: " + major + "." + minor + "." + build + "." + revision;
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
