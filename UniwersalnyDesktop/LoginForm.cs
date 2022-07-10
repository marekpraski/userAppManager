using DatabaseInterface;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;
using UtilityTools;

namespace UniwersalnyDesktop
{
    public partial class LoginForm : Form
    {
        public static string userLogin;
        public static string userPassword;

        public static string mainPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString();     //katalog z którego uruchamiany jest program, wykrywany przez DBConnector i ustawiany tutaj
                                                                                                                        //dla DEBUGA ustawiony jest w metodzie ReadAllData

        private QueryData userData;     //login_user=0, windows_user=1,imie_user=2, nazwisko_user=3

        private DBReader dbReader;
        public static SqlConnection dbConnection { get; private set; }
        public LoginForm()
        {
            InitializeComponent();
#if DEBUG
            userLogin = "root";
            userPassword = "root";
            logIn();
            this.Hide();
#endif
        }

        #region interakcja z użytkownikiem

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            userLogin = userNameTextBox.Text;
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            userPassword = passwordTextBox.Text;
        }

        private void btnZaloguj_Click(object sender, EventArgs e)
        {
            logIn();
        }

        private void passwordTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                logIn();
            }
        }

        private void LoginForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                logIn();
            }
        }

        private void btnUstawienia_Click(object sender, EventArgs e)
        {
            Form formConnectionConfiguration = new ConnectionConfigurator();
            formConnectionConfiguration.Show();
            formConnectionConfiguration.Activate();
        }

        #endregion

        #region logowanie - uruchamianie okna admina lub zwykłego użytkownika
        private void logIn()
        {
            dbConnection = createSqlConnection();
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
            DesktopForm desktop = new DesktopForm(userData, userPassword, dbReader, mainPath);
            //this.Hide();
            desktop.ShowDialog();
        }

        private void openAdminForm()
        {
            AdminForm adminForm = new AdminForm(userLogin);
            //this.Hide();
            adminForm.ShowDialog();
        }
        #endregion

        #region czytanie danych użytkownika
        private bool verifyUser()
        {
            this.userData = readUserata();
            return userData != null && userData.dataRowsNumber > 0;
        }

        private SqlConnection createSqlConnection()
        {
            //if (!dbConnector.validateConfigFile(mainPath))
            //    return null;

            XmlReader confReader = new XmlReader(mainPath + @"..\conf\config.xml");

            DBConnectionData connData = new DBConnectionData()
            {
                serverName = confReader.getNodeValue("server"),
                dbName = confReader.getNodeValue("db_desktop"),
                login = userPassword,
                password = userPassword
            };

            return new DBConnector().getDBConnection(connData);
        }


        private QueryData readUserata()
        {
            string query = @"select  login_user, windows_user, imie_user, nazwisko_user from users_list where login_user = '" + userLogin + "'";
            dbReader = new DBReader(dbConnection);
            return dbReader.readFromDB(query);
        }

        private ProgramSettings.UserType getUserType()
        {
            string userName = userData.getDataValue(0, "imie_user").ToString() + " " + userData.getDataValue(0, "nazwisko_user").ToString();

            if (userName.Equals(ProgramSettings.administratorName))
            {
                return ProgramSettings.UserType.Administrator;
            }
            else
            {
                return ProgramSettings.UserType.RegularUser;
            }
        } 
        #endregion

    }
}
