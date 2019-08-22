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
        private QueryData desktopData;

        private DBReader reader;
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
            readDesktopData();

            if (desktopData.getQueryData().Count > 0)
            {
                DesktopForm desktop = new DesktopForm(userData, userPassword, desktopData, currentPath);
                this.Hide();
                desktop.ShowDialog();
            }
            // niczego nie przeczytał bo użytkownik nie ma uprawnień do żadnych programów
            // w chwili obecnej z tą kwerendą która jest zdefiniowana w ProgramSettings nie zadziała, bo wyświetlam wszystkie programy niezależnie od dostępu użytkownika
            //trzeba by filtrować np. po Grant_app
            else
            {
                MyMessageBox.display("użytkownik nie ma dostępu do żadnych programów", MessageBoxType.Error);
            }
        }

        private void openAdminForm()
        {
            MyMessageBox.display("loguję jako admin", MessageBoxType.Information);
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
            readUserata();
            if (userData.getHeaders().Count>0)      //niczego nie przeczytał bo brak loginu, uprawnień do bazy danych, złe hasło itp błąd kwerendy
            {
                return true;
            }
            else
            {
                return false;
            }                
        }

        private void passwordTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                logIn();
            }
        }

        private void readUserata()
        {
            DBConnector connector = new DBConnector(userLogin, userPassword);
#if DEBUG
            currentPath = @"C:\SMD\SoftMineDesktop";
#else
            currentPath = connector.currentPath;
#endif
            if (connector.validateConfigFile())
            {
                SqlConnection dbConnection = connector.getDBConnection(ConnectionSources.serverNameInFile, ConnectionTypes.sqlAuthorisation);
                reader = new DBReader(dbConnection);

                string query = ProgramSettings.desktopUserDataQueryTemplate + "'" + userLogin + "'";
                userData = reader.readFromDB(query);
            }
        }

        private void readDesktopData()
        {
            string query = ProgramSettings.desktopAppDataQueryTemplate + "'" + userLogin + "'";
            desktopData = reader.readFromDB(query);
        }
    }
}
