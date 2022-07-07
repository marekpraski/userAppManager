
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

        public AddUserForm()
        {
            InitializeComponent();
        }

        #region metody na starcie formatki

        private void AddUserPermission_Load(object sender, EventArgs e)
        {
            loadPolaUzytkownicyIUprawnienia();
            this.Height = 100;
            btnZapisz.Enabled = false;
        }

        private void loadPolaUzytkownicyIUprawnienia()
        {
            typUzytkownikaCB.Items.Add("Windows authentication");
            typUzytkownikaCB.Items.Add("SQL Server authentication");
            typUzytkownikaCB.Items.Add("Mix");
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
                            uzytkownikCB.Items.Add(a);
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
                    uzytkownikCB.Items.Add(a);
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
            if (typUzytkownikaCB.SelectedItem.ToString() == "Windows authentication")
            {
                gbDane.Visible = false;
                uzytkownikCB.Visible = true;
                label7.Visible = true;
                this.Height = 130;

                string domainName = System.Environment.UserDomainName;
                getDomainUsers(domainName);
            }
            else if (typUzytkownikaCB.SelectedItem.ToString() == "SQL Server authentication")
            {
                gbDane.Visible = true;
                gbDane.Location = new Point(12, 61);
                uzytkownikCB.Visible = false;
                label7.Visible = false;
                this.Height = 321;
            }
            else if (typUzytkownikaCB.SelectedItem.ToString() == "Mix")
            {
                gbDane.Visible = true;
                gbDane.Location = new Point(12, 94);
                uzytkownikCB.Visible = true;
                label7.Visible = true;
                this.Height = 364;

                imieTB.Enabled = false;
                nazwiskoTB.Enabled = false;

                loginTB.Enabled = true;

                string domainName = System.Environment.UserDomainName;
                getDomainUsers(domainName);

            }
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
            checkIfFieldsAreFilled();
            //fill data of the selecetd user
            fillData();
        }

        ///  <summary>
        ///  fillData function
        ///  fill data based on authentication method: Mix
        ///  </summary>
        ///
        private void fillData()
        {
            if (typUzytkownikaCB.SelectedItem.ToString() == "Mix")
            {
                if (uzytkownikCB.SelectedIndex > -1)
                {
                    string domainName = System.Environment.UserDomainName;
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName, "DC=" + domainName + ",DC=local"))
                    {
                        UserPrincipal user = new UserPrincipal(pc);
                        user = UserPrincipal.FindByIdentity(pc, uzytkownikCB.Text);
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
        private void checkIfFieldsAreFilled()
        {
            if (typUzytkownikaCB.SelectedItem.ToString() == "Windows authentication")
            {
                if (uzytkownikCB.SelectedIndex > -1)
                {
                    btnZapisz.Enabled = true;
                }
            }
            else if (typUzytkownikaCB.SelectedItem.ToString() == "SQL Server authentication")
            {
                if (loginTB.Text != "" && hasloTB.Text != "" && potwierdzHasloTB.Text != "")
                {
                    if (hasloTB.Text != potwierdzHasloTB.Text)
                    {
                        btnZapisz.Enabled = false;
                    }
                    else
                    {
                        btnZapisz.Enabled = true;
                    }
                }
                else
                {
                    btnZapisz.Enabled = false;
                }
            }
            else if (typUzytkownikaCB.SelectedItem.ToString() == "Mix")
            {
                if (uzytkownikCB.SelectedIndex > -1)
                {
                    if (loginTB.Text != "" && hasloTB.Text != "" && potwierdzHasloTB.Text != "")
                    {
                        if (hasloTB.Text != potwierdzHasloTB.Text)
                        {
                            btnZapisz.Enabled = false;
                        }
                        else
                        {
                            btnZapisz.Enabled = true;
                        }
                    }
                    else
                    {
                        btnZapisz.Enabled = false;
                    }
                }
            }
        }
        #endregion

        #region zmiana tekstu w polach tekstowych
        ///  <summary>
        ///  loginTB_TextChanged function
        ///  load function checkIfFieldsAreFilled
        ///  </summary>
        ///
        private void loginTB_TextChanged(object sender, EventArgs e)
        {
            checkIfFieldsAreFilled();
        }
        ///  <summary>
        ///  hasloTB_TextChanged function
        ///  load functino checkIfFieldsAreFilled
        ///  </summary>
        ///
        private void hasloTB_TextChanged(object sender, EventArgs e)
        {
            checkIfFieldsAreFilled();
        }
        ///  <summary>
        ///  potwierdzHasloTB_TextChanged function
        ///  load function checkIfFieldsAreFilled
        ///  </summary>
        ///
        private void potwierdzHasloTB_TextChanged(object sender, EventArgs e)
        {
            checkIfFieldsAreFilled();
        }
        #endregion

        #region walidacja zawartości pól obowiązkowych
        // Validate a required field. Return true if the field is valid.
        private bool RequiredFieldIsBlank(ErrorProvider err1, ErrorProvider err2, TextBox txt1, TextBox txt2)
        {

            if (txt1.Text == txt2.Text || txt1.Text == "" || txt2.Text == "")
            {
                // Clear the error.
                err1.SetError(txt1, "");
                err2.SetError(txt2, "");
                return false;
            }
            else
            {
                // Set the error.
                err1.SetError(txt1, "Hasło i potwierdzenie hasła są różne.");
                err2.SetError(txt2, "Hasło i potwierdzenie hasła są różne.");
                return false;
            }
        }

        private void hasloTB_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = RequiredFieldIsBlank(errorProvider1, errorProvider2, hasloTB, potwierdzHasloTB);
        }

        private void potwierdzHasloTB_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = RequiredFieldIsBlank(errorProvider1, errorProvider2, hasloTB, potwierdzHasloTB);
        } 
        #endregion

        #region naciśnięcie przycisku Zapisz
        private void btnZapisz_Click(object sender, EventArgs e)
        {
            if (typUzytkownikaCB.SelectedItem.ToString() == "Windows authentication")
                addWindowsUser();

            else if (typUzytkownikaCB.SelectedItem.ToString() == "SQL Server authentication")
                addSqlUser();

            else if (typUzytkownikaCB.SelectedItem.ToString() == "Mix")
                addMixedUser();
        }

        #endregion

        #region dodawanie użytkowników do bazy

        private void addWindowsUser()
        {
            if (uzytkownikCB.SelectedIndex > -1)
            {
                string domainName = System.Environment.UserDomainName;
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName, "DC=" + domainName + ",DC=local"))
                {
                    UserPrincipal user = new UserPrincipal(pc);
                    user = UserPrincipal.FindByIdentity(pc, uzytkownikCB.Text);
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
            if (!mandatoryFieldsCorrect())
                return;

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
                        " exec AddUserDB " + "'" + loginTB.Text.ToString() + "'" + ", '" + hasloTB.Text + "' , 'SoftMineDesktop', 'SoftMine','dbo' " +
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
            if (!mandatoryFieldsCorrect())
                return;

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
                                    " exec AddUserDB " + "'" + loginTB.Text.ToString() + "'" + ", '" + hasloTB.Text + "' , 'SoftMineDesktop', 'SoftMine','dbo' " +
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
        private bool mandatoryFieldsCorrect()
        {
            if (String.IsNullOrEmpty(loginTB.Text) || String.IsNullOrEmpty(hasloTB.Text) || String.IsNullOrEmpty(potwierdzHasloTB.Text))
                return false;

            if (hasloTB.Text != potwierdzHasloTB.Text)
            {
                string msg = "Podane hasła się róźnią.";
                MessageBox.Show(msg, "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        public int checkIfLoginIsInDatabase(string name)
        {
            string sql = " SELECT name FROM syslogins WHERE name ='" + name + "';";
            return new DBReader(LoginForm.dbConnection).readFromDB(sql).dataRowsNumber;
        }

        #endregion

    }
}