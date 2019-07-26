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
    public partial class LoginForm : Form
    {
        private string userName;
        private string userPassword;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            userName = userNameTextBox.Text;
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            userPassword = passwordTextBox.Text;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            DesktopForm desktop = new DesktopForm(userName, userPassword);                  
            desktop.ShowDialog();
        }

        private void passwordTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DesktopForm desktop = new DesktopForm(userName, userPassword);
                desktop.ShowDialog();
            }
        }
    }
}
