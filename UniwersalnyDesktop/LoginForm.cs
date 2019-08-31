using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public partial class LoginForm : Form
    {
        private string userLogin;
        private string userPassword;

        private string currentPath = "";     //katalog z którego uruchamiany jest program, wykrywany przez DBConnector i ustawiany tutaj
                                             //dla DEBUGA ustawiony jest w metodzie ReadAllData

        private QueryData userData;     //login_user=0, windows_user=1,imie_user=2, nazwisko_user=3

        private DBReader dbReader;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            userLogin = userNameTextBox.Text;
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            userPassword = passwordTextBox.Text;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            logIn();
            
        }

        private void logIn()
        {
            if (verifyUser())
            {
                ProgramSettings.UserType userType = getUserType();
                switch (userType)
                {
                    case ProgramSettings.UserType.Administrator:
                        openAdminForm();
                        break;
                    case ProgramSettings.UserType.RegularUser:
                        openDesktopForm();
                        break;
                }                
            }
        }

        private void openDesktopForm()
        {           
            DesktopForm desktop = new DesktopForm(userData, userPassword, dbReader, currentPath);
            this.Hide();
            desktop.ShowDialog();
        }

        private void openAdminForm()
        {
            AdminForm adminForm = new AdminForm(userLogin, dbReader);
            //this.Hide();
            adminForm.ShowDialog();
            //MyMessageBox.display("loguję jako admin", MessageBoxType.Information);
        }

        private ProgramSettings.UserType getUserType()
        {
            string userName = userData.getQueryData()[0].ToList()[2] + " " + userData.getQueryData()[0].ToList()[3];
            if(userName.Equals(ProgramSettings.administratorName))
            {
                return ProgramSettings.UserType.Administrator;
            }
            else
            {
                return ProgramSettings.UserType.RegularUser;
            }
        }

        private bool verifyUser()
        {
            if (readUserata())
            {
                if (userData.getHeaders().Count > 0)      //niczego nie przeczytał bo brak loginu, uprawnień do bazy danych, złe hasło itp błąd kwerendy
                {
                    return true;
                }               
            }
            return false;
        }

        private void passwordTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                logIn();
            }
        }

        private bool readUserata()
        {
            DBConnector dbConnector = new DBConnector(userLogin, userPassword);
#if DEBUG
            currentPath = @"C:\testDesktop\conf";
#else
            currentPath = Application.StartupPath;
#endif
            if (dbConnector.validateConfigFile(currentPath))
            {
                SqlConnection dbConnection = dbConnector.getDBConnection(ConnectionSources.serverNameInFile, ConnectionTypes.sqlAuthorisation);
                dbReader = new DBReader(dbConnection);

                string query = ProgramSettings.desktopUserDataQueryTemplate + "'" + userLogin + "'";
                userData = dbReader.readFromDB(query);
                return true;
            }
            return false;
        }

        private void LoginForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                logIn();
            }
        }
    }
}
