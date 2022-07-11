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
        public static string mainPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString();     //katalog z którego uruchamiany jest program, wykrywany przez DBConnector i ustawiany tutaj
                                                                                                                        //dla DEBUGA ustawiony jest w metodzie ReadAllData
        public static DesktopUser user;

        public static SqlConnection dbConnection { get; private set; }
        public LoginForm()
        {
            InitializeComponent();
            LoginForm.user = new DesktopUser();
#if DEBUG
            user.sqlLogin = "root";
            user.sqlPassword = "root";
            logIn();
            this.Hide();
#endif
        }

        #region interakcja z użytkownikiem

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            user.sqlLogin = userNameTextBox.Text;
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            user.sqlPassword = passwordTextBox.Text;
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

        private void btnOProgramie_Click(object sender, EventArgs e)
        {
            Form newForm = new About();
            newForm.ShowDialog();
        }

        #endregion

        #region logowanie - uruchamianie okna admina lub zwykłego użytkownika
        private void logIn()
        {
            dbConnection = createSqlConnection();
            QueryData qd = readUserata();
            if (qd == null || qd.dataRowsNumber == 0)
            {
                MessageBox.Show("Brak użytkownika w bazie danych lub błędne hasło");
                return;
            }
            getUserData(qd);
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

        private void openDesktopForm()
        {
            DesktopForm desktop = new DesktopForm(user);
            //this.Hide();
            desktop.ShowDialog();
        }

        private void openAdminForm()
        {
            AdminForm adminForm = new AdminForm(user);
            //this.Hide();
            adminForm.ShowDialog();
        }
        #endregion

        #region tworzenie połaczenia sql do bazy
        private SqlConnection createSqlConnection()
        {
            //if (!dbConnector.validateConfigFile(mainPath))
            //    return null;

            XmlReader confReader = new XmlReader(mainPath + @"..\conf\config.xml");

            DBConnectionData connData = new DBConnectionData()
            {
                serverName = confReader.getNodeValue("server"),
                dbName = confReader.getNodeValue("db_desktop"),
                login = user.sqlPassword,
                password = user.sqlPassword
            };

            return new DBConnector().getDBConnection(connData);
        } 
        #endregion

        #region czytanie danych użytkownika

        private QueryData readUserata()
        {
            string query = @"select  login_user, windows_user, imie_user, nazwisko_user from users_list where login_user = '" + user.sqlLogin + "'";
            return new DBReader(dbConnection).readFromDB(query);
        }
        private void getUserData(QueryData qd)
        {
            user.firstName = qd.getDataValue(0, "imie_user").ToString();
            user.surname = qd.getDataValue(0, "nazwisko_user").ToString();
            user.windowsLogin = qd.getDataValue(0, "windows_user").ToString();
        }

        private ProgramSettings.UserType getUserType()
        {            
            if (user.displayName.Equals(ProgramSettings.administratorName))
                return ProgramSettings.UserType.Administrator;
            else
                return ProgramSettings.UserType.RegularUser;
        }
        #endregion

    }
}
