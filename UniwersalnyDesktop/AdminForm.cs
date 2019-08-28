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
        private DBReader dbReader;
        private string adminLogin;                              //login użytkownika, ale z założenia jest to Administrator skoro jest w tym oknie, inna nazwa bo chcę odróżnić od "user" który jest zwykłym użytkownikiem

        private Dictionary<string, string> sqlUsersDict = null;          //lista użytkowników sql, kluczem jest login a wartością nazwa do wyświetlenia w drzewie
        //private List<string> sqlLogins = null;                          //lista loginów sql dla potrzeb identyfikacji użytkowników w programie + do tworzenia zapytań sql
        private Dictionary<string, string> windowsUsersDict = null;      //lista użytkowników domenowych, kluczem jest login a wartością nazwa do wyświetlenia w drzewie
        //private List<string> windowsLogins = null;                          //lista loginów domenowych dla potrzeb identyfikacji użytkowników w programie + do tworzenia zapytań sql

        private List<string> appNameList = null;                           //lista nazw wszystkich dostępnych aplikacji

        private Dictionary<string, List<string[]>> appRolesDict = null;          //dla każdej aplikacji, tabela wszystkich ról dla niej dostępnych    0= ID_rola, 1= name_rola, 2= descr_rola
        private Dictionary<string, List<string[]>> userAppsDict = null;        // dla każdego użytkownika domenowego i sql, lista aplikacji, do których ma on uprawnienia wraz z ich rolami
                                                                            //0 = appDisplayName, 1 = name_rola

        private TreeNode previousSelectedUser = null;                   //służy do zmiany domyślnego koloru zaznaczonego użytkownika
                                                                        //gdy treeView staje się nieaktywne


        private ListViewItem previousCheckedRole = null;
        private bool appRoleListViewLoaded = false;                 //bez tej zmiennej event AppRoleListView_ItemChecked jest uruchamiany podczas ładowania listy 
                                                                    //AppRoleListView tyle razy ile jest wpisów na liście roli aplikacji
                                                                    //co powoduje widoczne opóźnienie w wyświetleniu tej listy
        Dictionary<string, string> duplicatedUsers;                 //jeżeli loginy sql lub domenowe się powtarzają to ten program nie może działać poprawnie
                                                                //wychwytuję powtarzające się loginy i je wyświetlam


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
            userAppsDict = new Dictionary<string, List<string[]>>();

            //dodaję do słownika aplikacje użytkowników domenowych
            foreach (string winUser in windowsUsersDict.Keys)
            {
                if (winUser != "")
                {
                    string query = ProgramSettings.windowsUserAppsQueryTemplate + "'" + winUser + "'";
                    QueryData windowsUserData = dbReader.readFromDB(query);
                    List<string> appList = convertColumnDataToList(windowsUserData.getQueryDataAsStrings());
                    List<string[]> appRolaPairsList = new List<string[]>();
                    foreach (string app in appList)
                    {
                        string[] appRolaPair = new string[2];
                        string appRola = getAppRola("windows_user", winUser, app);
                        appRolaPair[0] = app;
                        appRolaPair[1] = appRola;
                        appRolaPairsList.Add(appRolaPair);
                    }
                    userAppsDict.Add(winUser, appRolaPairsList);
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
                    List<string[]> appRolaPairsList = new List<string[]>();
                    foreach (string app in appList)
                    {
                        string[] appRolaPair = new string[2];
                        string appRola = getAppRola("login_user", sqlUser, app);
                        appRolaPair[0] = app;
                        appRolaPair[1] = appRola;
                        appRolaPairsList.Add(appRolaPair);
                    }
                    userAppsDict.Add(sqlUser, appRolaPairsList);
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
                treeView1.Nodes.Add(parentNode);
            }

            //jeżeli są jakieś zduplikowane loginy to do drzewa dodaję dodatkową gałąź
            if (duplicatedUsers.Count > 0)
            {
                TreeNode[] childNodes = populateTreviewBranch(duplicatedUsers);
                TreeNode parentNode = new TreeNode("zduplikowane loginy", childNodes);
                parentNode.ForeColor = Color.Red;
                treeView1.Nodes.Add(parentNode);
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
            treeView1.SelectedNode.BackColor = System.Drawing.Color.Aqua;
            previousSelectedUser = treeView1.SelectedNode;
        }

        //zmienia kolor poprzednio zaznaczonego wiersza w drzewie/liście na domyślny

        private void TreeView1_Click(object sender, EventArgs e)
        {
            if (previousSelectedUser != null)
            {
                previousSelectedUser.BackColor = treeView1.BackColor;
            }
        }

        #endregion

        #region Region - interakcje pomiędzy treeview, listview i listbox

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (ListViewItem listItem in appListView.Items)
            {
                listItem.Checked = false;
            }
            try
            {
                foreach (ListViewItem listItem in appRoleListView.Items)
                {
                    listItem.Checked = false;
                }

                string user = treeView1.SelectedNode.Tag.ToString();

                if (userAppsDict.ContainsKey(user))
                {
                    foreach (string[] appRolaPair in userAppsDict[user])
                    {
                        ListViewItem item = appListView.FindItemWithText(appRolaPair[0]);       //nazwa aplikacji jest pierwsza w parze
                        item.Checked = true;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                //tag nie jest przypisany do gałęzi drzew, wtedy wywala ten błąd, ale to OK, nic nie muszę z tym robić
            }
        }

        #endregion


        #region Region - lista programów
        public void populateAppListview()
        {
            foreach (string row in appNameList)
            {
                ListViewItem listRow = new ListViewItem(row);
                appListView.Items.Add(listRow);
            }
        }
        

        #endregion


        
        private void AppListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            //zaznaczenie elementu w appListView powoduje wypełnienie appRoleListView
            appRoleListView.Items.Clear();
            appRoleListViewLoaded = false;
            ListViewItem selectedApp = null;
            string app = "";
            try
            {

                ListView.SelectedListViewItemCollection apps = appListView.SelectedItems;
                selectedApp = apps[0];             //zawsze zaznaczony może być tylko jeden program, a jeżeli jest więcej, to i tak biorę pierwszy

                app = selectedApp.Text;                 
                if (appRolesDict.ContainsKey(app))
                {
                    List<string> roles = convertColumnDataToList(appRolesDict[app], ProgramSettings.roleIndex);
                    List<string> roleDescr = convertColumnDataToList(appRolesDict[app], ProgramSettings.roleDescrIndex);
                    ListViewItem[] itemRange = new ListViewItem[roles.Count];

                    for (int i=0; i<roles.Count; i++)
                    {
                        string rola = roles[i];
                        string descr = roleDescr[i];
                        ListViewItem item = new ListViewItem(rola);
                        item.SubItems.Add(descr);
                        itemRange[i] = item;
                    }
                    appRoleListView.Items.AddRange(itemRange);
                }
                appRoleListViewLoaded = true;
            

                //jeżeli zaznaczony użytkownik ma uprawnienia do zaznaczonej aplikacji, wówczas zaznacza się rola tego użytkownika w tej aplikacji
                string user = treeView1.SelectedNode.Tag.ToString();        //odczytuję, który użytkownik jest zaznaczony
                List<string[]> appRolaPairs;
                userAppsDict.TryGetValue(user, out appRolaPairs);
                string appRola = "";
                foreach (ListViewItem listItem in appRoleListView.Items)
                {
                    listItem.Checked = false;
                }

                if (selectedApp.Checked == true)
                {
                    foreach (string[] appRolaPair in appRolaPairs)
                    {
                        if (appRolaPair[0].Equals(app))
                        {
                            appRola = appRolaPair[1];
                        }
                    }
                    ListViewItem item = appRoleListView.FindItemWithText(appRola);
                    item.Checked = true;                
                }
            }
            catch (NullReferenceException exc)
            {
                //nie muszę nic robić z wyjątkiem, oznacza tylko, że nie ma żadnych ról dla wybranego programu, lista pozostaje pusta
            }
            catch (ArgumentOutOfRangeException exc1)
            {

            }

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
                    ListView.CheckedListViewItemCollection checkedRoles = appRoleListView.CheckedItems;
                    ListViewItem checkedRole = checkedRoles[0];                                         //zawsze będzie tylko jeden, więc zawsze na pozycji 0
                    previousCheckedRole = checkedRole;
                }
                catch (ArgumentOutOfRangeException exc)
                {

                }
                catch (NullReferenceException ex)
                {

                }
            }
        }             
    }
}
