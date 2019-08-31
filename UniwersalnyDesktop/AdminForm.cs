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
        #region Region - parametry

        private DBReader dbReader;
        private string adminLogin;                              //login użytkownika, ale z założenia jest to Administrator skoro jest w tym oknie, inna nazwa bo chcę odróżnić od "user" który jest zwykłym użytkownikiem

        private Dictionary<string, string> sqlUsersDict = null;          //lista użytkowników sql, kluczem jest login a wartością nazwa do wyświetlenia w drzewie
        //private List<string> sqlLogins = null;                          //lista loginów sql dla potrzeb identyfikacji użytkowników w programie + do tworzenia zapytań sql
        private Dictionary<string, string> windowsUsersDict = null;      //lista użytkowników domenowych, kluczem jest login a wartością nazwa do wyświetlenia w drzewie
        //private List<string> windowsLogins = null;                          //lista loginów domenowych dla potrzeb identyfikacji użytkowników w programie + do tworzenia zapytań sql

        private List<string> appNameList = null;                           //lista nazw wszystkich dostępnych aplikacji

        private Dictionary<string, List<string[]>> appRolesDict = null;          //dla każdej aplikacji, tabela wszystkich ról dla niej dostępnych    0= ID_rola, 1= name_rola, 2= descr_rola

        // dla każdego użytkownika domenowego i sql, słownik aplikacji, do których ma on uprawnienia wraz z ich rolami
        //klucz : użytkownik, wartość : słownik; klucz = appDisplayName, wartość = name_rola
        private Dictionary<string, Dictionary<string, string>> userAppsDict = null;        

        //zmienne służące do zmiany domyślnego koloru zaznaczonego elementu w drzewie użytkowników i liście aplikacji, gdy stają się one nieaktywne
        private string selectedUser = "";
        private TreeNode previousSelectedUser = null;
        private ListViewItem selectedApp = null;
        private ListViewItem previousSelectedApp = null;

        //do zapamiętywania która rola aplikacji była poprzednio zaznaczona, użyta do zapewnienia, że tylko jedna rola jest zaznaczona
        ListViewItem previousCheckedRole = null;


        private bool appRoleListViewLoaded = false;                 //bez tej zmiennej event AppRoleListView_ItemChecked jest uruchamiany podczas ładowania listy 
                                                                    //AppRoleListView tyle razy ile jest wpisów na liście roli aplikacji
                                                                    //co powoduje widoczne opóźnienie w wyświetleniu tej listy
        Dictionary<string, string> duplicatedUsers;                 //jeżeli loginy sql lub domenowe się powtarzają to ten program nie może działać poprawnie
                                                                    //wychwytuję powtarzające się loginy i je wyświetlam

        #endregion

        public AdminForm(string adminLogin, DBReader dbReader)
        {
            this.dbReader = dbReader;
            this.adminLogin = adminLogin;
            InitializeComponent();
            readAllData();
            setupAdminForm();
        }

        private void setupAdminForm()
        {
            populateUserTreeview();         //użytkownicy sql i domenowi
            populateAppListview();         //aplikacje
        }

        #region Region - wczytywanie danych na starcie formularza
        private void readAllData()
        {
            getUsers();
            getAppData();
            getAppRoles();
            getUserApps();
        }

        private void getUsers()
        {
            string query = ProgramSettings.userQueryTemplate + "'" + adminLogin + "'";
            List<string[]> userData = dbReader.readFromDB(query).getQueryDataAsStrings();
            List<string> imiona = convertColumnDataToList(userData, ProgramSettings.userImieIndex);
            List<string> nazwiska = convertColumnDataToList(userData, ProgramSettings.userNazwiskoIndex);

            string user = "";
            string userDisplayName = "";

            //tworzę nazwy użytkowników sql do wyświetlania
            List<string> sqlLogins = convertColumnDataToList(userData, ProgramSettings.userSqlLoginIndex);
            sqlUsersDict = new Dictionary<string, string>();
            
            for (int i = 0; i < sqlLogins.Count; i++)
            {
                if (sqlLogins[i] != "")
                {
                    userDisplayName = sqlLogins[i] + " (" + imiona[i] + " " + nazwiska[i] + ")";
                    sqlUsersDict.Add(sqlLogins[i], userDisplayName);                          
                }
            }

            //tworzę nazwy użytkowników domenowych do wyświetlania
            List<string> windowsLogins = convertColumnDataToList(userData, ProgramSettings.userWindowsLoginIndex);
            windowsUsersDict = new Dictionary<string, string>();
            duplicatedUsers = new Dictionary<string, string>();
            for (int i = 0; i < windowsLogins.Count; i++)
            {
                if (windowsLogins[i] != "")
                {
                    
                    try
                    {
                        user = windowsLogins[i].ToLower();        //celowa redundancja, żeby móc przekazać do komuniaktu błedu, zamieniam na lowercase, bo były powtórzenia
                                                                //zakładam, że login sql nie będzie się powtarzał, więc tam nie sprawdzam
                        userDisplayName = windowsLogins[i] + " (" + imiona[i] + " " + nazwiska[i] + ")";
                        windowsUsersDict.Add(user, userDisplayName);            
                    }
                    catch (ArgumentException ex)
                    {
                        MyMessageBox.display(ex.Message + "/n/r zdublowana nazwa użytkownika: " + user);
                        duplicatedUsers.Add(user, userDisplayName);
                    }
                }
            }
            //usuwam ze słowników wszystkich zduplikowanych użytkowników, bo w kolejnych kwerendach wyszukujących programów dla użytkowników wyniki są niejednoznaczne
            if (duplicatedUsers.Count > 0)
            {
                foreach (string duplicatedUser in duplicatedUsers.Keys)
                {
                    windowsUsersDict.Remove(duplicatedUser);
                }
            }
        }

        
        private void getAppData()
        {
            string query = ProgramSettings.appListQueryTemplate;
            QueryData appData = dbReader.readFromDB(query);                 //lista danych dla wszystkich dostępnych aplikacji: 0= ap.ID_app, 1= ap.appDisplayName
            appNameList = convertColumnDataToList(appData.getQueryDataAsStrings());
        }

        private void getAppRoles()
        {
            appRolesDict = new Dictionary<string, List<string[]>>();
            foreach (string appName in appNameList)
            {
                string query = ProgramSettings.appRolesQueryTemplate + "'" + appName + "'";
                QueryData appRolesData = dbReader.readFromDB(query);
                List<string[]> appRolesList = appRolesData.getQueryDataAsStrings();
                appRolesDict.Add(appName, appRolesList);
            }
        }

        private void getUserApps()
        {
            userAppsDict = new Dictionary<string, Dictionary<string, string>>();

            //dodaję do słownika aplikacje użytkowników domenowych
            foreach (string winUser in windowsUsersDict.Keys)
            {
                if (winUser != "")
                {
                    string query = ProgramSettings.windowsUserAppsQueryTemplate + "'" + winUser + "'";
                    QueryData windowsUserData = dbReader.readFromDB(query);
                    List<string> appList = convertColumnDataToList(windowsUserData.getQueryDataAsStrings());
                    Dictionary<string, string> appRolaPairs = new Dictionary<string, string>();
                    foreach (string app in appList)
                    {
                        string appRola = getAppRola("windows_user", winUser, app);
                        appRolaPairs.Add(app, appRola);
                    }
                    userAppsDict.Add(winUser, appRolaPairs);
                }
            }

            //dodaję do słownika aplikacje użytkowników sql
            foreach (string sqlUser in sqlUsersDict.Keys)
            {
                if (sqlUser != "")
                {
                    string query = ProgramSettings.sqlUserAppsQueryTemplate + "'" + sqlUser + "'";
                    QueryData sqlUserData = dbReader.readFromDB(query);
                    List<string> appList = convertColumnDataToList(sqlUserData.getQueryDataAsStrings());
                    Dictionary<string, string> appRolaPairs = new Dictionary<string, string>();
                    foreach (string app in appList)
                    {
                        string appRola = getAppRola("login_user", sqlUser, app);
                        appRolaPairs.Add(app, appRola);
                    }
                    userAppsDict.Add(sqlUser, appRolaPairs);
                }
            }
        }

        private string getAppRola(string userType, string user, string app)
        {
            string query = ProgramSettings.userAppRolaQueryTemplate.Replace("@appDisplayName", app).Replace("@user", user).Replace("@loginType", userType);
            QueryData windowsUserData = dbReader.readFromDB(query);
            List<string> rolaList = convertColumnDataToList(windowsUserData.getQueryDataAsStrings());
            if (rolaList.Count > 0)
            {
                return rolaList[0];         //zawsze jest tylko jedna rola
            }
            return "";
        }

        #endregion

       
        
        #region Region - drzewo użytkowników
        public void populateUserTreeview()
        {
            string[] treeviewBranchNames = { "użytkownicy sql", "użytkownicy domenowi" };
            Dictionary<string, string>[] treeviewBranchItems = { sqlUsersDict, windowsUsersDict };

            for (int i = 0; i < treeviewBranchNames.Length; i++)            //nie używam foreach bo potrzebuję iteratora do znalezienia nazwy gałęzi
            {
                Dictionary<string, string> oneBranchItems = treeviewBranchItems[i];
                TreeNode[] childNodes = populateTreviewBranch(oneBranchItems);
                TreeNode parentNode = new TreeNode(treeviewBranchNames[i], childNodes);
                parentNode.Tag = "";                                                    //przypisuę pusty string, żeby mi nie wywalało błędu podczas wyciągania tekstu z taga w metodzie userTreeView_AfterSelect
                userTreeView.Nodes.Add(parentNode);
            }

            //jeżeli są jakieś zduplikowane loginy to do drzewa dodaję dodatkową gałąź
            if (duplicatedUsers.Count > 0)
            {
                TreeNode[] childNodes = populateTreviewBranch(duplicatedUsers);
                TreeNode parentNode = new TreeNode("zduplikowane loginy", childNodes);
                parentNode.ForeColor = Color.Red;
                parentNode.Tag = "";
                userTreeView.Nodes.Add(parentNode);
            }
        }

        
        public TreeNode[] populateTreviewBranch(Dictionary<string, string> items)
        {
            TreeNode[] childNodes = new TreeNode[items.Count];
            int i = 0;
            string userDisplayName;
            foreach (string login in items.Keys)
            {
                items.TryGetValue(login, out userDisplayName);
                TreeNode treeNode = new TreeNode(userDisplayName);
                treeNode.Tag = login;
                childNodes[i] = treeNode;
                i++;
            }
            return childNodes;
        }

        //podświetla zaznaczony wiersz w drzewie na wybrany kolor gdy użytkownik kliknie w inny obiekt w formularzu
        //domyślnie ten kolor jest bladoszary, dla mnie zbyt niewidoczny
        private void TreeView1_Leave(object sender, EventArgs e)
        {
            userTreeView.SelectedNode.BackColor = System.Drawing.Color.Aqua;
            previousSelectedUser = userTreeView.SelectedNode;
        }

        
        //zmienia kolor poprzednio zaznaczonego wiersza w drzewie/liście na domyślny
        private void userTreeView_Click(object sender, EventArgs e)
        {
            if (previousSelectedUser != null)
            {
                previousSelectedUser.BackColor = userTreeView.BackColor;
            }            
        }

        private void userTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {           

            //odfajkowuję wszystkie aplikacje
            foreach (ListViewItem listItem in appListView.Items)
            {
                listItem.Checked = false;
            }

            selectedUser = userTreeView.SelectedNode.Tag.ToString();

            //jeżeli user ma jakieś uprawnienia to te aplikacje zafajkowuję oraz rolę zaznaczonej
            if (userAppsDict.ContainsKey(selectedUser) && userAppsDict[selectedUser].Count > 0)
            {
                foreach (string appName in userAppsDict[selectedUser].Keys)
                {
                    ListViewItem app = appListView.FindItemWithText(appName);       //nazwa aplikacji jest kluczem
                    app.Checked = true;
                }
                checkAppRolaCheckbox();
            }
            //jeżeli użytkownik nie ma uprawnień do żadnych aplikacji to odfajkowuję wszystkie role
            //"" oznacza nazwy gałęzi drzewa
            else if (!selectedUser.Equals(""))
            {
                {
                    uncheckAllAppRolaCheckboxes();
                }
            }
        }

        #endregion

        

        #region Region - lista programów
        private void populateAppListview()
        {
            foreach (string row in appNameList)
            {
                ListViewItem listRow = new ListViewItem(row);
                appListView.Items.Add(listRow);
            }
        }


        //zaznaczenie aplikacji na liście appListView powoduje wypełnienie roli tej aplikacji w appRoleListView oraz zafajkowanie checkboxa roli, którą ma ten użytkowni
        //nie używam zdarzenia "SelectedIndexChanged" bo nie zapewnia mi żądanej funkcjonalności
        private void AppListView_Click(object sender, EventArgs e)
        {
            selectedApp = getSelectedApp();
            if (selectedApp != previousSelectedApp)
            {
                updateAppRolaViewBox();
            }
            else
            {
                checkAppRolaCheckbox();         //jeżeli zaznaczona aplikacja się nie zmieniła to aktualizuję tylko role dla niej
            }
        }
       

        private void resetPreviousSelectedAppColour()
        {
            if (previousSelectedApp != null)
            {
                previousSelectedApp.BackColor = appListView.BackColor;
            }
            previousSelectedApp = selectedApp;
        }

        //zmienia kolor zaznaczonego po kliknięciu w inne okno; żeby działało parametr HideSelection musi być true
        private void AppListView_Leave(object sender, EventArgs e)
        {
            selectedApp.BackColor = Color.Aqua;
        }

        private ListViewItem getSelectedApp()
        {

            if (appListView.SelectedIndices.Count > 0)
            {
                int intselectedindex = appListView.SelectedIndices[0];
                return appListView.Items[intselectedindex];
            }
            return null;
        }

        #endregion


        #region Region - lista ról aplikacji

        private void populateRolaListView()
        {
            List<string> roles = convertColumnDataToList(appRolesDict[selectedApp.Text], ProgramSettings.roleIndex);
            List<string> roleDescr = convertColumnDataToList(appRolesDict[selectedApp.Text], ProgramSettings.roleDescrIndex);
            ListViewItem[] roleRange = new ListViewItem[roles.Count];

            for (int i = 0; i < roles.Count; i++)
            {
                string rola = roles[i];
                string descr = roleDescr[i];
                ListViewItem item = new ListViewItem(rola);
                item.SubItems.Add(descr);
                roleRange[i] = item;
            }
            appRoleListView.Items.AddRange(roleRange);
        }

        private void updateAppRolaViewBox()
        {
            try
            {
                //resetuję ustawienia widoku listy ról aplikacji
                appRoleListView.Items.Clear();
                appRoleListViewLoaded = false;
                resetPreviousSelectedAppColour();

                //wypełniam listę rolami, jeżeli są
                if (appRolesDict[selectedApp.Text].Count > 0)
                {
                    populateRolaListView();
                    appRoleListViewLoaded = true;
                }

                //jeżeli zaznaczony użytkownik ma uprawnienia do zaznaczonej aplikacji, wówczas zaznacza się rola tego użytkownika w tej aplikacji
                checkAppRolaCheckbox();

            }
            //błędy do obsłużenia warunkami w debugu
            catch (NullReferenceException ex)
            {
#if DEBUG
                MyMessageBox.display(ex.Message + " updateAppRolaViewBox", MessageBoxType.Error);
#endif
            }
            catch (ArgumentOutOfRangeException exc)
            {
#if DEBUG
                MyMessageBox.display(exc.Message + " updateAppRolaViewBox", MessageBoxType.Error);
#endif
            }
        }


        private void checkAppRolaCheckbox()
        {
            if (appRoleListViewLoaded)
            {
                //wyciągam słownik ról dla zaznaczonego użytkownika
                Dictionary<string, string> appRolaPairs;
                try
                {
                    userAppsDict.TryGetValue(selectedUser, out appRolaPairs);

                    //zaznaczonym użytkownikiem nie jest nazwa gałęzi (wtedy appRolaPairs == null)
                    //dla zaznaczonej aplikacji ten użytkownik ma uprawnienia i aplikacja ta ma rolę
                    if ((appRolaPairs != null) && (appRolaPairs.Count > 0))
                    {
                        //ze słownika wyciągam rolę zaznaczonej aplikacji
                        string appRola = "";

                        appRolaPairs.TryGetValue(selectedApp.Text, out appRola);
                        //wyrzuca wyjątek jeżeli program ma role, ale użytkownik nie ma do niego uprawnień, tj jego appRola.Equals("")
                        if (appRola != null)
                        {
                            ListViewItem rolaItem = appRoleListView.FindItemWithText(appRola);

                            if (selectedApp.Checked == true)     //użytkownik ma uprawnienia do zaznaczonej aplikacji
                            {
                                rolaItem.Checked = true;    //warunek, że zaznaczona może być tylko jedna rola obsługuje zdarzenie AfterChecked
                            }
                            else
                            {
                                rolaItem.Checked = false;
                            }
                        }
                    }
                }
                catch (ArgumentNullException ex)
                {
                    MyMessageBox.display(ex.Message + "  checkAppRolaCheckbox", MessageBoxType.Error);
                }
                catch (NullReferenceException exc)
                {
                    MyMessageBox.display(exc.Message + "  checkAppRolaCheckbox", MessageBoxType.Error);
                }             
            }
        }

        private void uncheckAllAppRolaCheckboxes()
        {
            //odfajkowuję tylko wtedy, jeżeli coś jest załadowane
            if (appRoleListViewLoaded)
            {
                ListViewItem checkedRole = getCheckedAppRole();
                if (checkedRole != null)
                {                                                   
                    checkedRole.Checked = false;
                }
            }
        }

        private ListViewItem getCheckedAppRole()
        {
            ListView.CheckedListViewItemCollection checkedRoles = appRoleListView.CheckedItems;
            if (checkedRoles.Count > 0)
            {
                ListViewItem checkedRole = checkedRoles[0];                //zawsze będzie tylko jedna, więc zawsze na pozycji 0   
                return checkedRole;
            }
            return null;
        }


        //checkbox może być zaznaczony tylko przy jednej roli
        private void AppRoleListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (appRoleListViewLoaded)      //uruchamiam kod metody tylko wtedy, gdy do listy władowane są wszystkie wpisy
            {
                try
                {
                    if (previousCheckedRole != null)
                    {
                       previousCheckedRole.Checked = false;
                    }
                    ListViewItem checkedRole = getCheckedAppRole();
                    if (checkedRole !=null)
                    {
                        previousCheckedRole = checkedRole;
                    }
                    else
                    {
                        previousCheckedRole = null;
                    }
                }
                //te błędy powinienem obsłużyć warunkami, więc łapię je tylko w debugu
                catch (ArgumentOutOfRangeException exc)
                {
#if DEBUG
                    MyMessageBox.display(exc.Message + "  AppRoleListView_ItemChecked", MessageBoxType.Error);
#endif
                }
                catch (NullReferenceException ex)
                {
#if DEBUG
                    MyMessageBox.display(ex.Message + "  AppRoleListView_ItemChecked", MessageBoxType.Error);
#endif
                }
            }
        }

        #endregion

        private List<string> convertColumnDataToList(List<string[]> tableData, int columnNr = 0)
        {
            List<string> columnData = new List<string>();
            for (int i = 0; i < tableData.Count; i++)
            {
                string columnItem = tableData[i][columnNr];
                columnData.Add(columnItem);
            }
            return columnData;
        }        
    }
}
