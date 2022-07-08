
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using DatabaseInterface;
using System.Windows.Forms;
using UtilityTools;

namespace UniwersalnyDesktop
{
    public partial class AddUserForm : Form
    {
        private List<string> userWindowsAuth = new List<string>();
        public string windowField;
        private NumberHandler nh = new NumberHandler();
        private readonly string[] authorisationTypes = new string[] { "Domenowe i sql", "Tylko domenowe", "Tylko sql" };

        public AddUserForm()
        {
            InitializeComponent();
        }

        #region metody na starcie formatki

        private void AddUserPermission_Load(object sender, EventArgs e)
        {
            loadPolaUzytkownicyIUprawnienia();
            formSettingsMixedAuthentication();
        }

        private void loadPolaUzytkownicyIUprawnienia()
        {
            typUzytkownikaCB.Items.Add(authorisationTypes[0]);
            typUzytkownikaCB.Items.Add(authorisationTypes[1]);
            typUzytkownikaCB.Items.Add(authorisationTypes[2]);
            typUzytkownikaCB.SelectedIndex = 0;
        }

        #endregion

        #region pobieranie użytkowników domenowych z systemu
        ///  <summary>
        ///  getUsersForDomain function
        ///  add users for domain to uzytkownikCB
        ///  </summary>
        ///
        private void getDomainUsers(string domain)
        {
            if (userWindowsAuth.Count == 0)
            {
                try
                {
                    PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain, "DC=" + domain + ",DC=local");

                    try
                    {
                        UserPrincipal qbeUser = new UserPrincipal(ctx);

                        PrincipalSearcher srch = new PrincipalSearcher(qbeUser);

                        List<string> userList = new List<string>();
                        foreach (var users in srch.FindAll())
                        {
                            string name = users.Name;
                            userList.Add(name);
                        }

                        userList.Sort();
                        userWindowsAuth = userList;
                        foreach (var a in userList)
                        {
                            cbUzytkownik.Items.Add(a);
                        }
                    }
                    catch (Exception exe)
                    {
                        MessageBoxError.ShowBox(exe);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxError.ShowBox(ex);
                }
            }
            else
            {
                foreach (var a in userWindowsAuth)
                {
                    cbUzytkownik.Items.Add(a);
                }
            }
        }
        #endregion

        #region zmiana wyboru w kombo typu użytkownika
        ///  <summary>
        ///  typUzytkownikaCB_SelectedIndexChanged function
        ///  load function getUsersForDomain based on authentication method
        ///  </summary>
        ///
        private void typUzytkownikaCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[1])
            {
                formSettingsWindowsAuthentication();

                string domainName = System.Environment.UserDomainName;
                getDomainUsers(domainName);
            }
            else if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[2])
            {
                formSettingsSqlAuthentication();
            }
            else if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[0])
            {
                formSettingsMixedAuthentication();

                string domainName = System.Environment.UserDomainName;
                getDomainUsers(domainName);

            }
        }

        private void formSettingsWindowsAuthentication()
        {
            gbDane.Visible = false;
            cbUzytkownik.Visible = true;
            label7.Visible = true;
            this.Height = 130;
        }

        private void formSettingsSqlAuthentication()
        {
            gbDane.Visible = true;
            gbDane.Location = new Point(12, 61);
            cbUzytkownik.Visible = false;
            label7.Visible = false;
            this.Height = 284;
        }

        private void formSettingsMixedAuthentication()
        {
            gbDane.Visible = true;
            gbDane.Location = new Point(12, 94);
            cbUzytkownik.Visible = true;
            label7.Visible = true;
            this.Height = 327;

            imieTB.Enabled = false;
            nazwiskoTB.Enabled = false;

            loginTB.Enabled = true;
        }
        #endregion

        #region zmiana wyboru użytkownika w kombo
        ///  <summary>
        ///  uzytkownikCB_SelectedIndexChanged function
        ///  load function checkIfFieldsAreFilled
        ///  </summary>
        ///
        private void uzytkownikCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillData();
        }

        ///  <summary>
        ///  fillData function
        ///  fill data based on authentication method: Mix
        ///  </summary>
        ///
        private void fillData()
        {
            if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[0])
            {
                if (cbUzytkownik.SelectedIndex > -1)
                {
                    string domainName = System.Environment.UserDomainName;
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName, "DC=" + domainName + ",DC=local"))
                    {
                        UserPrincipal user = new UserPrincipal(pc);
                        user = UserPrincipal.FindByIdentity(pc, cbUzytkownik.Text);
                        if (user != null)
                        {
                            windowField = domainName + "\\" + user.SamAccountName.ToString();
                            imieTB.Text = user.GivenName;
                            nazwiskoTB.Text = user.Surname;
                            loginTB.Text = user.GivenName.ToLower();
                        }
                    }
                }
            }
        }
        ///  <summary>
        ///  checkIfFieldsAreFilled function
        ///  enable/disable button - zatwierdzB based on authentication method
        ///  </summary>
        ///
        private bool checkIfFieldsAreFilled()
        {
            if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[1])
                return windowsMandatoryFieldsFilled();
            else if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[2])
                return sqlMandatoryFieldsFilled();
            else if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[0])
                return sqlMandatoryFieldsFilled() && windowsMandatoryFieldsFilled();

            return false;
        }
        private bool sqlMandatoryFieldsFilled()
        {
            if (!String.IsNullOrEmpty(loginTB.Text) && !String.IsNullOrEmpty(tbHaslo.Text))
                return true;

            return false;
        }
        private bool windowsMandatoryFieldsFilled()
        {
            if (cbUzytkownik.SelectedIndex > -1)
                return true;

            return false;
        }
        #endregion

        #region naciśnięcie przycisku Zapisz
        private void btnZapisz_Click(object sender, EventArgs e)
        {
            if (!checkIfFieldsAreFilled())
            {
                MessageBox.Show("Nie wszystkie pola są wypełnione");
                return;
            }

            if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[1])
                addWindowsUser();

            else if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[2])
                addSqlUser();

            else if (typUzytkownikaCB.SelectedItem.ToString() == authorisationTypes[0])
                addMixedUser();
        }

        #endregion

        #region dodawanie użytkowników do bazy

        private void addWindowsUser()
        {
            if (cbUzytkownik.SelectedIndex > -1)
            {
                string domainName = System.Environment.UserDomainName;
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName, "DC=" + domainName + ",DC=local"))
                {
                    UserPrincipal user = new UserPrincipal(pc);
                    user = UserPrincipal.FindByIdentity(pc, cbUzytkownik.Text);
                    if (user != null)
                    {
                        string nazwaTabeli = "users_list";
                        List<string> nazwyKolumn = new List<string>();
                        nazwyKolumn.Add("windows_user");
                        nazwyKolumn.Add("imie_user");
                        nazwyKolumn.Add("nazwisko_user");
                        nazwyKolumn.Add("login_user");

                        List<string> nazwyWartosci = new List<string>();
                        nazwyWartosci.Add(domainName + "\\" + user.SamAccountName.ToString());
                        nazwyWartosci.Add(user.GivenName);
                        nazwyWartosci.Add(user.Surname);
                        nazwyWartosci.Add("NULL");

                        string sqlDelete = "DELETE  FROM " + nazwaTabeli + " WHERE windows_user = '" + domainName + "\\" + user.SamAccountName.ToString() + "';";
                        string sql = "INSERT INTO " + nazwaTabeli + " ( " + string.Join(",", nazwyKolumn.ToArray()) + " ) VALUES ( '" + string.Join("','", nazwyWartosci.ToArray()) + "'); ";

                        string procedura = "BEGIN TRY " +
                            " exec AddUserDB " + "'" + domainName + "\\" + user.SamAccountName.ToString() + "'" + ", NULL, 'SoftMineDesktop', 'SoftMine','dbo'" +
                            " SELECT 'SUCCESS' " +
                            " END TRY " +
                            " BEGIN CATCH " +
                            " SELECT ERROR_MESSAGE() AS ErrorMessage; " +
                            " END CATCH ";

                        QueryData qd = new DBReader(LoginForm.dbConnection).readFromDB(procedura);
                        string comment = qd.getDataValue(0, 0).ToString();

                        if (comment == "SUCCESS")
                        {
                            if (new DBWriter(LoginForm.dbConnection).executeQuery(sqlDelete + sql))
                            {
                                MessageBox.Show("Użytkownik został dodany.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Użytkownik nie został dodany.\n" + comment, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void addSqlUser()
        {
            string nazwaTabeli = "users_list";
            List<string> nazwyKolumn = new List<string>();
            nazwyKolumn.Add("imie_user");
            nazwyKolumn.Add("nazwisko_user");
            nazwyKolumn.Add("login_user");

            List<string> nazwyWartosci = new List<string>();
            nazwyWartosci.Add(imieTB.Text);
            nazwyWartosci.Add(nazwiskoTB.Text);
            string loginName = loginTB.Text;
            nazwyWartosci.Add(loginName);


            int nameExist = checkIfLoginIsInDatabase(loginName);
            if (nameExist == 1)
            {
                DialogResult dg = MessageBox.Show("Login " + loginName + " jest już dodany do serwera. Czy dodać go do bazy danych desktopu?", "Uwaga", MessageBoxButtons.YesNo, MessageBoxIcon.None);
                if (dg == DialogResult.Yes)
                {
                    nameExist = 0;
                }
            }
            if (nameExist == 0)
            {
                if (oddzialTB.Text != "")
                {
                    int oddzial = nh.tryGetInt(oddzialTB.Text.ToString());
                    nazwyKolumn.Add("oddzial");
                    nazwyWartosci.Add(oddzial.ToString());
                }

                string sqlDelete = "DELETE  FROM " + nazwaTabeli + " WHERE login_user = '" + loginTB.Text + "';";

                string sql = "INSERT INTO " + nazwaTabeli + " ( " + string.Join(",", nazwyKolumn.ToArray()) + " ) VALUES ( '" + string.Join("','", nazwyWartosci.ToArray()) + "'); ";

                string procedura = " BEGIN TRY " +
                        " exec AddUserDB " + "'" + loginTB.Text.ToString() + "'" + ", '" + tbHaslo.Text + "' , 'SoftMineDesktop', 'SoftMine','dbo' " +
                        " SELECT 'SUCCESS' " +
                        " END TRY " +
                        " BEGIN CATCH " +
                        " SELECT ERROR_MESSAGE() AS ErrorMessage; " +
                        " END CATCH ";

                QueryData qd = new DBReader(LoginForm.dbConnection).readFromDB(procedura);
                string comment = qd.getDataValue(0, 0).ToString();

                if (comment == "SUCCESS")
                {
                    if (new DBWriter(LoginForm.dbConnection).executeQuery(sqlDelete + sql))
                    {
                        MessageBox.Show("Użytkownik został dodany.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                    MessageBox.Show("Użytkownik nie został dodany.\n" + comment, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addMixedUser()
        {
            string nazwaTabeli = "users_list";
            List<string> nazwyKolumn = new List<string>();
            nazwyKolumn.Add("imie_user");
            nazwyKolumn.Add("nazwisko_user");
            nazwyKolumn.Add("login_user");
            nazwyKolumn.Add("windows_user");


            List<string> nazwyWartosci = new List<string>();
            nazwyWartosci.Add(imieTB.Text);
            nazwyWartosci.Add(nazwiskoTB.Text);
            string loginName = loginTB.Text;
            nazwyWartosci.Add(loginName);
            nazwyWartosci.Add(windowField);


            int nameExist = checkIfLoginIsInDatabase(loginName);
            if (nameExist == 1)
            {
                DialogResult dg = MessageBox.Show("Login " + loginName + " jest już dodany do serwera. Czy dodać go do bazy danych desktopu?", "Uwaga", MessageBoxButtons.YesNo, MessageBoxIcon.None);
                if (dg == DialogResult.Yes)
                {
                    nameExist = 0;
                }
            }
            if (nameExist == 0)
            {
                if (oddzialTB.Text != "")
                {
                    int oddzial = nh.tryGetInt(oddzialTB.Text.ToString());
                    nazwyKolumn.Add("oddzial");
                    nazwyWartosci.Add(oddzial.ToString());
                }

                string sqlDelete = "DELETE  FROM " + nazwaTabeli + " WHERE login_user = '" + loginTB.Text + "';";

                string sql = "INSERT INTO " + nazwaTabeli + " ( " + string.Join(",", nazwyKolumn.ToArray()) + " ) VALUES ( '" + string.Join("','", nazwyWartosci.ToArray()) + "'); ";
                string procedura1 = " BEGIN TRY " +
                                    " exec AddUserDB " + "'" + loginTB.Text.ToString() + "'" + ", '" + tbHaslo.Text + "' , 'SoftMineDesktop', 'SoftMine','dbo' " +
                                    " SELECT 'SUCCESS' " +
                                    " END TRY " +
                                    " BEGIN CATCH " +
                                    " SELECT ERROR_MESSAGE() AS ErrorMessage; " +
                                    " END CATCH ";
                string procedura2 = " BEGIN TRY " +
                                    "exec AddUserDB " + "'" + windowField + "'" + ", NULL, 'SoftMineDesktop', 'SoftMine','dbo' " +
                                        " SELECT 'SUCCESS' " +
                                        " END TRY " +
                                        " BEGIN CATCH " +
                                        " SELECT ERROR_MESSAGE() AS ErrorMessage; " +
                                        " END CATCH ";

                QueryData qd = new DBReader(LoginForm.dbConnection).readFromDB(procedura1 + procedura2);
                string comment = qd.getDataValue(0, 0).ToString();

                if (comment == "SUCCESS")
                {
                    if (new DBWriter(LoginForm.dbConnection).executeQuery(sqlDelete + sql))
                    {
                        MessageBox.Show("Użytkownik został dodany.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                    MessageBox.Show("Użytkownik nie został dodany.\n" + comment, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public int checkIfLoginIsInDatabase(string name)
        {
            string sql = " SELECT name FROM syslogins WHERE name ='" + name + "';";
            return new DBReader(LoginForm.dbConnection).readFromDB(sql).dataRowsNumber;
        }

        #endregion

    }
}