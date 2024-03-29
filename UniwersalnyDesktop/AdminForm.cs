﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace UniwersalnyDesktop
{
    public partial class AdminForm : Form
    {

        #region Region - parametry

        private DBReader dbReader;
        private SqlConnection dbConnection;
        private string adminLogin;                              //login użytkownika, ale z założenia jest to Administrator skoro jest w tym oknie, inna nazwa bo chcę odróżnić od "user" który jest zwykłym użytkownikiem

        //
        //słowniki danych podstawowych
        //
        private Dictionary<string, DesktopUser> allUsersDict;            //lista wszystkich użytkowników desktopu, kluczem jest Id
        private Dictionary<string, string> sqlUsersDict;            //lista użytkowników sql, kluczem jest Id, wartością nazwa użytkownika wyświetlana w drzewie
        private Dictionary<string, string> windowsUsersDict;      //lista użytkowników domenowych, kluczem jest Id, , wartością nazwa użytkownika wyświetlana w drzewie
        private Dictionary<string, string> duplicatedWindowsUsers;        //jeżeli loginy  domenowe się powtarzają to ten program nie może działać poprawnie
                                                                                 //wychwytuję powtarzające się loginy i je wyświetlam


        private Dictionary<string, App> appDictionary;             //lista wszystkich aplikacji zdefiniowanych w desktopie, kluczem jest Id
        private Dictionary<string, Rola> rolaDict;                //lista wszystkich ról aplikacji, kluczem jest Id_rola
        private Dictionary<string, AppModule> moduleDict;       //lista wszystkich modułów aplikacji, kluczem jest ID_mod


        //
        //zmienne służące do zmiany domyślnego koloru zaznaczonego elementu w drzewie użytkowników i liście aplikacji, gdy stają się one nieaktywne
        //
        private TreeNode currentSelectedUser = null;
        private TreeNode previousSelectedUser = null;

        private ListViewItem currentSelectedApp = null;
        private ListViewItem previousSelectedApp = null;

        //do zapamiętywania która rola aplikacji była poprzednio zaznaczona, użyta do zapewnienia, że tylko jedna rola jest zaznaczona
        private ListViewItem previousCheckedRola = null;
        private ListViewItem currentCheckedRola = null;


        private bool userTreeViewMouseClicked = false;                //nie chcę eventów na treeView wywoływać podczas ładowania go z bazy na starcie programu


        private bool rolaListMouseClicked = false;                 //bez tej zmiennej event AppRoleListView_ItemChecked jest uruchamiany podczas ładowania listy 
                                                                   //rolaistView tyle razy ile jest wpisów na liście roli aplikacji
                                                                   //co powoduje widoczne opóźnienie w wyświetleniu tej listy
        private bool appListMouseClicked = false;

        //
        //zapamiętywanie zmian
        //

        private Dictionary<string, DesktopUser> userBackupDict = null;       //używana do trzymania oryginałów, gdyby po dokonaniu zmian 
                                                                             //użytkownik zrezygnował z ich zapisania do bazy; klucz: user ID

        private Dictionary<DesktopUser, Dictionary<App, AppDataItem>> userAppChangeDict = null;        //zapisuję zmiany w uprawnieniach do aplikacji i rolach
                                                                                                  

        #endregion

        //
        //
        //czytanie danych oraz ładowanie drzewa użytkowników i listy aplikacji na starcie formularza
        //po tej akcji formularz się otwiera i czeka na pierwszą interakcję użytkownika
        //
        //


        public AdminForm(string adminLogin, SqlConnection dbConnection, DBReader dbReader)
        {
            this.dbReader = dbReader;
            this.adminLogin = adminLogin;
            this.dbConnection = dbConnection;
            InitializeComponent();
            createDictionaries();
            readAllData();
            setupAdminForm();
        }


        #region Region - wczytywanie danych na starcie formularza

        private void createDictionaries()
        {
            allUsersDict = new Dictionary<string, DesktopUser>();

            sqlUsersDict = new Dictionary<string, string>();
            windowsUsersDict = new Dictionary<string, string>();
            duplicatedWindowsUsers = new Dictionary<string, string>();

            appDictionary = new Dictionary<string, App>();
            rolaDict = new Dictionary<string, Rola>();
            moduleDict = new Dictionary<string, AppModule>();

            //przygotowanie do zapisywania zmian
            userBackupDict = new Dictionary<string, DesktopUser>();
            userAppChangeDict = new Dictionary<DesktopUser, Dictionary<App, AppDataItem>>();
        }


        private void setupAdminForm()
        {
            populateUserTreeview();                                 //użytkownicy sql i domenowi
            populateAppListview();                                   //aplikacje
        }

 
        private void readAllData()
        {
            getUserData();
            getAppData();
            getAppModules();
            getRolaData();
            getUserApps();
        }


        private void getUserData()
        {
            string query = SqlQueries.getUsers + "'" + adminLogin + "'";
            List<string[]> userData = dbReader.readFromDB(query).getQueryDataAsStrings();

            foreach(string[] data in userData)
            {
                DesktopUser desktopUser = new DesktopUser();
                desktopUser.name = data[SqlQueries.userImieIndex];
                desktopUser.surname = data[SqlQueries.userNazwiskoIndex];
                desktopUser.sqlLogin = data[SqlQueries.userSqlLoginIndex];
                desktopUser.windowsLogin = data[SqlQueries.userWindowsLoginIndex];
                desktopUser.id = data[SqlQueries.userIdIndex];
                allUsersDict.Add(data[SqlQueries.userIdIndex], desktopUser);
            }
            groupUsers();

            //usuwam ze słowników wszystkich zduplikowanych użytkowników, bo w kolejnych kwerendach wyszukujących programów dla użytkowników wyniki są niejednoznaczne
            removeDuplicatedWindowsUsers();
        }

       
        private void groupUsers()
        {
            string userDisplayName = "";
            DesktopUser user = null;

            foreach (string userId in allUsersDict.Keys)
            {
                allUsersDict.TryGetValue(userId, out user);
                if (user.sqlLogin != "")
                {
                    userDisplayName = user.sqlLogin + " (" + user.name + " " + user.surname + ")";      //nazwa użytkownika wyświetlana w drzewie
                    sqlUsersDict.Add(user.id, userDisplayName);
                }

                if (user.windowsLogin != "")
                {
                    userDisplayName = user.windowsLogin + " (" + user.name + " " + user.surname + ")";      //nazwa użytkownika wyświetlana w drzewie
                    windowsUsersDict.Add(user.id, userDisplayName);
                }
            }
        }

        private void removeDuplicatedWindowsUsers()
        {
            Dictionary<string,List<string>> users = new Dictionary<string,List<string>>();  //kluczem jest login windowsowy małymi literami, wartością jest lista id
            DesktopUser user = null;
            List<string> idList = null;

            //wypełniam słownik, żeby pogrupować użytkowników po ich loginach windowsowych SPROWADZONYCH DO LOWERCASE 
            foreach (string userId in windowsUsersDict.Keys)
            {
                allUsersDict.TryGetValue(userId, out user);
 
                string windowsLogin = user.windowsLogin.ToLower();
                string id = user.id;

                if (users.ContainsKey(windowsLogin))
                {
                    users.TryGetValue(windowsLogin, out idList);
                    idList.Add(id);
                }
                else
                {
                    idList = new List<string>();
                    idList.Add(id);
                    users.Add(windowsLogin, idList);
                }
            }

            foreach(string windowsLogin in users.Keys)
            {
                users.TryGetValue(windowsLogin, out idList);
                if (idList.Count > 1)                           //tzn login powtarza się
                {
                    foreach(string userId in idList)
                    {
                        windowsUsersDict.Remove(userId);
                        duplicatedWindowsUsers.Add(userId, windowsLogin + " (id użytkownika = " + userId +")");
                    }
                }
            }
        }

        private void getAppData()
        {
            string query = SqlQueries.getAppList.Replace("@filter", SqlQueries.appFilter_DisplayNameNotNull);
            List<string[]> appData = dbReader.readFromDB(query).getQueryDataAsStrings(); 

            foreach (string[] data in appData)
            {
                App app = new App();
                app.id = data[SqlQueries.getAppList_appIdIndex];
                app.displayName = data[SqlQueries.getAppList_appDisplayNameIndex];
                appDictionary.Add(data[SqlQueries.getAppList_appIdIndex], app);
            }
        }


        private void getAppModules()
        {
            App app;

            string query = SqlQueries.getAppModuleList;
            QueryData modulesData = dbReader.readFromDB(query);
            List<string[]> moduleDataList = modulesData.getQueryDataAsStrings();
            foreach (string[] moduleData in moduleDataList)
            {
                AppModule module = new AppModule();
                module.id = moduleData[SqlQueries.getAppModuleList_moduleIdIndex];
                module.name = moduleData[SqlQueries.getAppModuleList_moduleNameIndex];
                module.appId = moduleData[SqlQueries.getAppModuleList_moduleAppIdIndex];

                appDictionary.TryGetValue(module.appId, out app);
                app.addModule(module);
                moduleDict.Add(module.id, module);
            }
        }



        private void getRolaData()
        {
            App app;

            string query = SqlQueries.getRolaList.Replace("@filter", "");
            List<string[]> appRolaData = dbReader.readFromDB(query).getQueryDataAsStrings();
            foreach (string[] rolaData in appRolaData)
            {
                Rola rola = new Rola();
                rola.id = rolaData[SqlQueries.getRolaList_rolaIdIndex];
                rola.appId = rolaData[SqlQueries.getRolaList_rolaAppIdIndex];
                rola.name = rolaData[SqlQueries.getRolaList_rolaNameIndex];
                rola.description = rolaData[SqlQueries.getRolaList_rolaDescrIndex];
                rola.appName = rolaData[SqlQueries.getRolaList_rolaAppNameIndex];

                getRolaAppModules(rola);

                rolaDict.Add(rolaData[SqlQueries.getRolaList_rolaIdIndex], rola);

                //dodaję role aplikacji w każdej aplikacji
                appDictionary.TryGetValue(rola.appId, out app);
                app.addRola(rola);
            }
        }

        private void getRolaAppModules(Rola rola)
        {
            AppModule module;
            string query = SqlQueries.getRolaModules + rola.id;
            List<string[]> rolaModuleData = dbReader.readFromDB(query).getQueryDataAsStrings();
            if (rolaModuleData.Count > 0)
            {
                foreach (string[] moduleData in rolaModuleData)
                {
                    moduleDict.TryGetValue(moduleData[SqlQueries.getRolaModules_moduleId], out module);
                    string grantApp = moduleData[SqlQueries.getRolaModules_GrantApp];
                    rola.addModule(module, grantApp);
                }
            }
        }

        //dane każdego użytkownika uzupełniam o zestawienie aplikacji do których ma uprawnienia wraz z rolami
        private void getUserApps()
        {
            DesktopUser user = null;
            
            foreach (string userId in allUsersDict.Keys)
            {
                string query = SqlQueries.getUserApps + "'" + userId + "'";
                List<string[]> appList = dbReader.readFromDB(query).getQueryDataAsStrings();
                List<string> appIdList = convertColumnDataToList(appList);                  //ta kwerenda zwraca pojedynczą listę, tj tylko id

                allUsersDict.TryGetValue(userId, out user);
                if (appIdList.Count > 0)
                {
                    App app;
                    Rola rola;
                    foreach (string appId in appIdList)
                    {
                        appDictionary.TryGetValue(appId, out app);
                        string rolaId = getAppRola(userId, appId);
                        rolaDict.TryGetValue(rolaId, out rola);
 
                        user.addUpdateApp(app, rola);
                    }
                }
            }
        }


        private string getAppRola(string userId, string appId)
        {
            string query = SqlQueries.getUserAppRola.Replace("@appId", appId).Replace("@userId", userId);
            QueryData windowsUserData = dbReader.readFromDB(query);
            List<string> rolaList = convertColumnDataToList(windowsUserData.getQueryDataAsStrings());
            if (rolaList.Count > 0)
            {
                return rolaList[0];         //zawsze jest tylko jedna rola
            }
            return "";
        }


        public void populateUserTreeview()
        {
            string[] treeviewBranchNames = { "użytkownicy sql", "użytkownicy domenowi" };
            Dictionary<string, string>[] treeviewBranchItems = { sqlUsersDict, windowsUsersDict };

            for (int i = 0; i < treeviewBranchNames.Length; i++)            //nie używam foreach bo potrzebuję iteratora do znalezienia nazwy gałęzi
            {
                Dictionary<string, string> oneBranchItems = treeviewBranchItems[i];
                TreeNode[] childNodes = populateTreviewBranch(oneBranchItems);
                TreeNode parentNode = new TreeNode(treeviewBranchNames[i], childNodes);
                parentNode.Name = "";                                                    //przypisuę pusty string, żeby mi nie wywalało błędu podczas wyciągania tekstu z taga w metodzie userTreeView_AfterSelect
                userTreeView.Nodes.Add(parentNode);
            }

            //jeżeli są jakieś zduplikowane loginy to do drzewa dodaję dodatkową gałąź
            if (duplicatedWindowsUsers.Count > 0)
            {
                TreeNode[] childNodes = populateTreviewBranch(duplicatedWindowsUsers);
                TreeNode parentNode = new TreeNode("zduplikowane loginy", childNodes);
                parentNode.ForeColor = Color.Red;
                parentNode.Name = "";
                userTreeView.Nodes.Add(parentNode);
            }
        }


        public TreeNode[] populateTreviewBranch(Dictionary<string, string> items)
        {
            TreeNode[] childNodes = new TreeNode[items.Count];
            int i = 0;
            string userDisplayName = "";
            DesktopUser user = null;
            foreach (string userId in items.Keys)
            {
                items.TryGetValue(userId, out userDisplayName);
                allUsersDict.TryGetValue(userId, out user);
                TreeNode treeNode = new TreeNode(userDisplayName);
                treeNode.Name = user.id;
                childNodes[i] = treeNode;
                i++;
            }
            return childNodes;
        }


        private void populateAppListview()
        {
            App app = null;
            foreach (string appId in appDictionary.Keys)
            {
                appDictionary.TryGetValue(appId, out app);
                ListViewItem listRow = new ListViewItem(app.displayName);
                listRow.Name = appId;
                appListView.Items.Add(listRow);
            }
        }


        #endregion


        //
        //
        //zdarzenia 
        //
        //

        #region Region - interakcja z użytkownikiem - pasek narzędziowy

        private void HelpButton_Click(object sender, EventArgs e)
        {
            string helpMessage = "Jeżeli aplikacja ma rolę, to odznaczenie roli nie spowoduje zapisania zmian ";
            MyMessageBox.display(helpMessage);
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            saveChanges();
            resetAdminForm();
        }



        private void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            saveChanges();
            saveButton.Enabled = false;         //gdy true, pyta czy zapisać zmiany
            this.Dispose();
            this.Close();
        }


        private void StatusInformationButton_Click(object sender, EventArgs e)
        {
            displayChanges();
        }



        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if (userAppChangeDict.Count>0)
            {
                MyMessageBox.display("Należy najpierw zapisać zmiany", MessageBoxType.Error);
            }
            else
            {
                resetAdminForm();
            }
        }


        #endregion




        #region - Region - interakcja z użytkownikiem - labele (linki)

        private void EditAppsLabel_Click(object sender, EventArgs e)
        {
            AppEditorForm appEditor = new AppEditorForm( dbConnection, SqlQueries.getAppList.Replace("@filter", ""), appDictionary);
            appEditor.ShowDialog();
        }


        private void EditRolaLabel_Click(object sender, EventArgs e)
        {
            if (currentSelectedApp != null)
            {
                App app;
                appDictionary.TryGetValue(currentSelectedApp.Name, out app);
                string query = SqlQueries.getRolaList.Replace("@filter", SqlQueries.getRolaList_rolaFilter_AppId) + app.id;

                RolaEditorForm dbRolaEditor = new RolaEditorForm(dbConnection, query, app);
                dbRolaEditor.ShowDialog();
            }
            else
            {
                MyMessageBox.display("należy zaznaczyć aplikację");
            }

        }


        private void EditUsersLabel_Click(object sender, EventArgs e)
        {
            UserEditorForm userEditor = new UserEditorForm(dbConnection, SqlQueries.getUsers +"'root'");
            userEditor.ShowDialog();
        }

        #endregion


        #region Region - zdarzenia na formularzu AdminForm


        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saveButton.Enabled)
            {
                string userId = currentSelectedUser.Name;
                MyMessageBoxResults result = MyMessageBox.display("Czy zapisać zmiany?", MessageBoxType.YesNoCancel);
                if (result == MyMessageBoxResults.Yes)
                {
                    saveChanges();
                    userBackupDict.Clear();
                    userAppChangeDict.Clear();
                    //ładowanie ustawień dla nowego użytkownika
                }
                else if (result == MyMessageBoxResults.Cancel)
                {
                    e.Cancel = true;
                }
                else     //result == MyMessageBoxResults.No
                {
                    //nic nie rób, po prostu zamknij formularz
                }
            }
        }


        #endregion



        #region Region - interakcja z użytkownikiem - zdarzenia na drzewie użytkowników


        //podświetla zaznaczony wiersz w drzewie na wybrany kolor gdy użytkownik kliknie w inny obiekt w formularzu
        //domyślnie ten kolor jest bladoszary, dla mnie zbyt niewidoczny
        private void userTreeView_Leave(object sender, EventArgs e)
        {
            try
            {
                currentSelectedUser.BackColor = Color.Aqua;
                currentSelectedUser.ForeColor = Color.Black;
            }
            catch (NullReferenceException ex)
            {
                MyMessageBox.display(ex.Message);
            }


        }


        private void UserTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            userTreeViewMouseClicked = true;
        }



        private void UserTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (userTreeViewMouseClicked)
            {
                //zapobiegam wielokrotnemu wywoływaniu zdarzenia ItemChecked na liście aplikacji
                appListMouseClicked = false;
                rolaListMouseClicked = false;
            }
        }


        private void userTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (userTreeViewMouseClicked)
            {
                toggleSelectedNodeColour();

                if (currentSelectedUser.Name.Equals(""))          //"" oznacza, że zaznaczona zostaje nazwa gałęzi
                {
                    appListView.Enabled = false;
                    rolaListView.Enabled = false;
                }
                else
                {
                    // resetuje ustawienia odfajkowując wszystkie checkboxy
                    uncheckAllApps();
                    uncheckRola();

                    updateListBoxes();
                    //updateRolaViewBox();

                    appListView.Enabled = true;
                }
                userTreeViewMouseClicked = false;
            }
        }

       

        #endregion




        #region Region - interakcja z użytkownikiem - zdarzenia na liście programów

        //zaznaczenie aplikacji na liście appListView powoduje wypełnienie roli tej aplikacji w appRoleListView oraz zafajkowanie checkboxa roli, którą ma ten użytkowni
        //do wychwycenia że to działanie użytkownika używam MouseDown bo MouseClick i Click nie występują wraz ze zdarzeniem ItemSelectionChanged

        private void AppListView_MouseDown(object sender, MouseEventArgs e)
        {
            appListMouseClicked = true;
        }


        private void AppListView_ItemChecked(object sender, ItemCheckedEventArgs e)  
        {
            if (appListMouseClicked)
            {
                ListViewItem item = e.Item;
                bool isChecked = item.Checked;
                string appId = item.Name;

                if (!isChecked && item==currentSelectedApp)
                {
                    uncheckRola();
                    rolaListView.Enabled = false;
                }
                else
                {
                    rolaListView.Enabled = true;
                }

                addUpdateUserApp(appId, isChecked);
                appListMouseClicked = false;
            }
        }


        private void AppListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)            //warunek powoduje, że zdarzenie wywoływane jest tylko podczs zaznaczania, bez niego wywoływane jest dwa razy: podczas odznaczania i zaznaczania
            {
                setCurrentSelectedApp();
                populateRolaListView();
                if(!currentSelectedApp.Checked)
                {
                    rolaListView.Enabled = false;
                }
                appListMouseClicked = false;
            }
        }


        //zmienia kolor zaznaczonego po kliknięciu w inne okno; żeby działało parametr HideSelection musi być true
        private void AppListView_Leave(object sender, EventArgs e)
        {
            currentSelectedApp.BackColor = Color.Aqua;
        }

        #endregion



        #region Region - interakcja z użytkownikiem - zdarzenia na liście ról

        private void rolaListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (rolaListMouseClicked)      //uruchamiam kod metody tylko wtedy, gdy użytkownik kliknie
            {
                string userId = currentSelectedUser.Name;
                if (!userId.Equals(""))                 //muszę tu też sprawdzić, bo odznaczanie checkboxa roli w przypadku zaznaczenia gałęzi wyzwala to zdarzenie i wywala się błąd na ID użytkownika==""
                {
                    try
                    {
                        previousCheckedRola = currentCheckedRola;
                        setCurrentCheckedRola();

                        if (currentSelectedApp != null)
                        {
                            addUpdateUserApp(currentSelectedApp.Name, currentSelectedApp.Checked);
                        }
                    }
                    catch (ArgumentNullException ex)
                    {
                        MyMessageBox.display(ex.Message + "  ArgumentNull rolaListView_ItemChecked", MessageBoxType.Error);
                    }
                    catch (NullReferenceException exc)
                    {
                        MyMessageBox.display(exc.Message + "  NullReference rolaListView_ItemChecked", MessageBoxType.Error);
                    }
                }
                rolaListMouseClicked = false;

                if (previousCheckedRola != null)
                {
                    //upewniam się, że checkbox jest zaznaczony tylko przy jednej roli, odznaczając poprzedni
                    previousCheckedRola.Checked = false;
                }
            }
        }


        private void RolaListView_MouseClick(object sender, MouseEventArgs e)
        {
            rolaListMouseClicked = true;
        }


        #endregion


        //
        //
        //aktualizacja listy aplikacji i ról na skutek interakcji użytkownika
        //
        //

        #region Region - metody wywoływane na drzewie użytkowników na skutek akcji użytkownika

        private void toggleSelectedNodeColour()
        {
            //zmieniam kolor poprzednio zaznaczonego wiersza w drzewie/liście na domyślny
            previousSelectedUser = currentSelectedUser;
            if (previousSelectedUser != null)
            {
                previousSelectedUser.BackColor = userTreeView.BackColor;
                previousSelectedUser.ForeColor = Color.Black;
            }

            currentSelectedUser = userTreeView.SelectedNode;
            currentSelectedUser.BackColor = Color.DodgerBlue;
            currentSelectedUser.ForeColor = Color.White;
        }


        #endregion



        #region Region - metody wywoływane na liście aplikacji na skutek akcji użytkownika

        private void updateListBoxes()
        {
            //wyszukuję aplikacji zaznaczonego użytkownika
            DesktopUser user = null;
            allUsersDict.TryGetValue(currentSelectedUser.Name, out user);              //parametr "name" zawiera Id użytkownika
            List<App> userApps = user.getApps();

            //jeżeli user ma jakieś uprawnienia to te aplikacje zafajkowuję oraz rolę zaznaczonej
            if (userApps != null)
            {
                foreach (App app in userApps)
                {
                    ListViewItem appItem = appListView.Items[app.id];
                    appItem.Checked = true;
                }
                checkRolaCheckbox();
            }
        }


        //odfajkowuję wszystkie aplikacje
        private void uncheckAllApps()
        {
            
            foreach (ListViewItem listItem in appListView.Items)
            {
                listItem.Checked = false;
            }
        }



        private void resetPreviousSelectedAppColour()
        {
            if (previousSelectedApp != null)
            {
                previousSelectedApp.BackColor = appListView.BackColor;
            }
            previousSelectedApp = currentSelectedApp;
        }



        private void setCurrentSelectedApp()
        {
            if (appListView.SelectedIndices.Count > 0)
            {
                int intselectedindex = appListView.SelectedIndices[0];
                currentSelectedApp = appListView.Items[intselectedindex];
            }
        }

        #endregion



        #region Region - metody wywoływane na liście ról na skutek akcji użytkownika

        //widok listy ról nie jest wypełniany na starcie, lecz
        //operacja występuje, gdy kliknięta zostanie aplikacja na liście aplikacji
        private void populateRolaListView()
        {
            if (currentSelectedApp != null)
            {
                rolaListView.Enabled = true;
                //resetuję ustawienia widoku listy ról aplikacji
                rolaListView.Items.Clear();

                previousCheckedRola = null;
                currentCheckedRola = null;
                resetPreviousSelectedAppColour();

                //wypełniam listę rolami, jeżeli są

                App app = null;
                appDictionary.TryGetValue(currentSelectedApp.Name, out app);

                List<string> rolaIdList = app.rolaIdList;
                if (rolaIdList.Count > 0)
                {
                    ListViewItem[] roleRange = new ListViewItem[rolaIdList.Count];
                    Rola rola = null;
                    int i = 0;

                    foreach (string rolaId in rolaIdList)
                    {
                        rolaDict.TryGetValue(rolaId, out rola);
                        string name = rola.name;
                        string descr = rola.description;
                        ListViewItem item = new ListViewItem(name);
                        item.Name = rolaId;
                        item.SubItems.Add(descr);
                        roleRange[i] = item;
                        i++;
                    }
                    rolaListView.Items.AddRange(roleRange);

                    //jeżeli zaznaczony użytkownik ma uprawnienia do zaznaczonej aplikacji, wówczas zaznacza się rola tego użytkownika w tej aplikacji
                    checkRolaCheckbox();
                }
            }
        }

        //zafajkowuje rolę zaznaczonej aplikacji jeżeli zaznaczony użytkownik ma do niej uprawnienia
        private void checkRolaCheckbox()
        {
            if (currentSelectedApp != null && currentSelectedUser !=null)          //bez tego warunku gdy wybiorę użytkownika to wyrzuca wyjątek, bo chce zaznaczyć rolę dla niewybranej aplikacji
            {
                DesktopUser user;
                allUsersDict.TryGetValue(currentSelectedUser.Name, out user);
                App app;
                appDictionary.TryGetValue(currentSelectedApp.Name, out app);
                string rolaId = user.getRolaId(app);

                try
                {
                    ListViewItem rolaItem;

                    if (!rolaId.Equals(""))
                    {
                        rolaItem = rolaListView.Items[rolaId];

                        if (currentSelectedApp.Checked == true && user.getAppData(app).isEnabled)     //użytkownik ma uprawnienia do zaznaczonej aplikacji
                                                                                                      //oraz sprawdzam, że aplikacja nie została usunięta(odfajkowana) przez Administratora 
                                                                                                    //zanim Administrator zaznaczył aplikację
                        {
                            rolaItem.Checked = true;        //warunek, że zaznaczona może być tylko jedna rola obsługuje zdarzenie AfterChecked
                            setCurrentCheckedRola();      //aktualizuję zmienną currentlyCheckedRola
                        }
                        else
                        {
                            rolaItem.Checked = false;
                        }
                    }
                }
                catch(NullReferenceException e)
                {
                    MyMessageBox.display(e.Message + "\r\n  AdminForm checkRolaCheckbox");
                }
            }
        }

        private void uncheckRola()
        {
            if (currentCheckedRola != null)
            {                                                   
                currentCheckedRola.Checked = false;
                currentCheckedRola = null;
                previousCheckedRola = null;
            }
        }

        //aktualizuję zmienną currentCheckedRola
        //w momencie wywoływania tej funkcji zaznaczone są dwie role, jedna ta co wcześniej i jedna ta co obecnie
        //dlatego porównuję je i rolą zaznaczoną obecnie jest ta, która jest różna od roli zaznaczonej wcześniej
        private void setCurrentCheckedRola()
        {
            ListView.CheckedListViewItemCollection checkedRoles = rolaListView.CheckedItems;
            if (checkedRoles.Count > 0)
            {
                foreach(ListViewItem rola in checkedRoles)
                {
                    if(rola != (previousCheckedRola))
                    {
                        currentCheckedRola = rola;
                    }
                }
            }
        }

        
        #endregion


        //
        //
        //zapamiętywanie zmian i zapisywanie zmian
        //
        //


        #region Region : zapamiętywanie zmian

        private DesktopUser getCurrentUser()
        {
            string userId = currentSelectedUser.Name;

            DesktopUser user = null;
            allUsersDict.TryGetValue(userId, out user);
            return user;
        }
              


        private void addUpdateUserApp(string appId, bool isChecked)
        {
            DesktopUser currentUser = getCurrentUser();
            DesktopUser backupedUser = backupUser(currentUser);
            App app;
            appDictionary.TryGetValue(appId, out app);

            if (isChecked)          //Administrator chce użytkownikowi aplikację dodać lub zaktualizować rolę aplikacji, którą użytkownik już ma
            {
                if (!backupedUser.hasApp(app))        //rzeczywiście użytkownik nie miał tej aplikacji oryginalnie
                {
                    addApp(currentUser, app);
                }
                else            //miał ją oryginalnie, więc Administrator  w tej sesji najpierw ją usunął a teraz chce ponownie dodać
                                //więc tak naprawdę chodzi o aktualizcję aplikacji a nie jej dodanie
                {
                    updateApp(currentUser, backupedUser, app);
                }
            }
            else            //Administrator chce użytkownikowi zabrać uprawnienia do aplikacji
            {
                if (currentUser.markAppDisabled(app))   //aplikację udało się odznaczyć, co może oznaczać, że użytkownik ją oryginalnie miał ale może też oznaczać, 
                                                        //że jej nie miał ale została ona dodana w tej sesji, po czym Administrator zmienił zdanie i ją chce usunąć
                {
                    if (backupedUser.hasApp(app))       //tak więc sprawdzam, czy oryginalnie ta aplikacja rzeczywiście była w portfolio użytkownika
                    {
                        addChangedAppDataToDict(currentUser, app);
                        saveButton.Enabled = true;
                        saveAndCloseButton.Enabled = true;
                        statusInformationButton.Enabled = true;
                    }
                    else                        //i jeżeli jej nie było, to po prostu usuwam ze zmian
                    {
                        currentUser.deleteApp(app);
                        deleteFromChangedAppDataDict(currentUser, app);
                    }
                }
            }
        }

        private void updateApp(DesktopUser currentUser, DesktopUser backupedUser, App app)
        {
            if (app.hasRola())
            {
                if (currentCheckedRola != null)
                {
                    string originalRolaId = backupedUser.getRolaId(app);

                    if (originalRolaId.Equals(currentCheckedRola.Name))
                    {
                        currentUser.addUpdateApp(app);              //użytkownik originalnie miał tę apkę z taką samą rolą więc tylko dodaję ją do bieżącego użytkownika
                        deleteFromChangedAppDataDict(currentUser, app);    //żeby się wyświetlała, ale nie chcę jej na liście zmian
                    }
                    else                    //użytkownik miał ją ale z inną rolą, należy zaktualizować
                    {
                        addApp(currentUser, app);
                    }
                }
            }
            else                    //ta apka nie ma roli
            {
                currentUser.addUpdateApp(app);              //a użytkownik originalnie ją miał, więc tylko dodaję ją do bieżącego użytkownika
                deleteFromChangedAppDataDict(currentUser, app);    //żeby się wyświetlała, ale nie chcę jej na liście zmian
            }
        }


        private void addApp(DesktopUser currentUser, App app)
        {
            Rola rola;
            if (!app.hasRola())         //tutaj aktualizuję tylko aplikację, która nie ma roli; aplikację która ma rolę aktualizuję po zaznaczeniu roli
            {
                currentUser.addUpdateApp(app);
                addChangedAppDataToDict(currentUser, app);
                saveButton.Enabled = true;
                saveAndCloseButton.Enabled = true;
                statusInformationButton.Enabled = true;
            }
            else
            {
                if (currentCheckedRola != null)     //ten fragment kodu uruchamiany jest ze zdarzenia ItemCheck na liście ról
                {
                    rolaDict.TryGetValue(currentCheckedRola.Name, out rola);
                    currentUser.addUpdateApp(app, rola);
                    addChangedAppDataToDict(currentUser, app);
                    saveButton.Enabled = true;
                    saveAndCloseButton.Enabled = true;
                    statusInformationButton.Enabled = true;
                }
            }
        }

        private void deleteFromChangedAppDataDict(DesktopUser user, App app)
        {
            Dictionary<App, AppDataItem> appChangeDict;
            userAppChangeDict.TryGetValue(user, out appChangeDict);
            appChangeDict.Remove(app);
            if(appChangeDict.Count == 0)
            {
                userAppChangeDict.Remove(user);
            }
            if(userAppChangeDict.Count == 0)
            {
                saveButton.Enabled = false;
                saveAndCloseButton.Enabled = false;
                statusInformationButton.Enabled = false;
            }
        }


        private void addChangedAppDataToDict(DesktopUser user, App app)
        {
            Dictionary<App, AppDataItem> appChangeDict;
            AppDataItem appData = user.getAppData(app);

            if (!userAppChangeDict.ContainsKey(user))
            {
                appChangeDict = new Dictionary<App, AppDataItem>();
                appChangeDict.Add(app, appData);

                userAppChangeDict.Add(user, appChangeDict);
            }
            else
            {
                userAppChangeDict.TryGetValue(user, out appChangeDict);
                
                if (appChangeDict.ContainsKey(app))
                {
                    appChangeDict[app] = appData;
                }
                else
                {
                    appChangeDict.Add(app, appData);
                }
            }
        }


        private void displayChanges()
        {
            ChangedDataBundle changedDataBundle = new ChangedDataBundle(userAppChangeDict, userBackupDict);
            DisplayChangesForm changes = new DisplayChangesForm(changedDataBundle);
            changes.ShowDialog();
        }


        private DesktopUser backupUser(DesktopUser user)
        {
            DesktopUser backupUser;

            if (!userBackupDict.ContainsKey(user.id))        //dodaję tylko raz, na początku, tj oryginał
            {
                backupUser = (DesktopUser) user.Clone();
                userBackupDict.Add(user.id, backupUser);
            }
            else
            {
                userBackupDict.TryGetValue(user.id, out backupUser);
            }
            return backupUser;
        }



        #endregion


        #region Region : zapisywanie zmian do bazy


        private void saveChanges()
        {
            ChangedDataBundle changedDataBundle = new ChangedDataBundle(userAppChangeDict, userBackupDict);

            DBWriter writer = new DBWriter(dbConnection);
            string query = generateQuery(changedDataBundle);
            MyMessageBox.display(query);
            writer.writeToDB(query);
        }


        private string generateQuery(ChangedDataBundle changedDataBundle)
        {
            string query = "";
            foreach (DesktopUser user in changedDataBundle.getUsers())
            {
                foreach (App app in changedDataBundle.getChangedUserApps(user))
                {
                    query += generateSingleQuery(user, changedDataBundle.getAppDataStatus(user, app), changedDataBundle.getNewAppData(user, app), changedDataBundle.getOldAppData(user, app));
                }
            }
            return query;
        }


        private string generateSingleQuery(DesktopUser user, string queryType, AppDataItem newAppData, AppDataItem oldAppData)
        {
            string newAppId = newAppData.appId;
            string newRolaId = newAppData.rolaId;
            string oldRolaId = "";
            string oldAppId = "";

            if (oldAppData != null)         //jest null wtedy, gdy robiony jest insert
            {
                oldRolaId = oldAppData.rolaId;
                oldAppId = oldAppData.appId;
            }

            string query = "";
            switch (queryType)
            {
                case "delete":
                    if (oldRolaId.Equals(""))
                    {
                        query = SqlQueries.deleteUserApp.Replace("@appId", oldAppId).Replace("@userId", user.id) + "\r\n";
                    }
                    else
                    {
                        query = SqlQueries.deleteUserAppAndRola.Replace("@appId", oldAppId).Replace("@rolaId", oldRolaId).Replace("@userId", user.id) + "\r\n";
                    }
                    break;
                case "update":
                        query = query = SqlQueries.updateUserRola.Replace("@newRolaId", newRolaId).Replace("@oldRolaId", oldRolaId).Replace("@userId", user.id) + "\r\n";
                    break;
                case "insert":
                    if (newRolaId.Equals(""))
                    {
                        query = SqlQueries.insertUserApp.Replace("@appId", newAppId).Replace("@userId", user.id) + "\r\n";
                    }
                    else
                    {
                        query = SqlQueries.insertUserAppAndRola.Replace("@appId", newAppId).Replace("@rolaId", newRolaId).Replace("@userId", user.id) + "\r\n";
                    }
                    break;
            }
            return query;
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


        private void resetAdminForm()
        {
            allUsersDict.Clear();
            sqlUsersDict.Clear();
            windowsUsersDict.Clear();
            duplicatedWindowsUsers.Clear();
            appDictionary.Clear();
            rolaDict.Clear();
            userAppChangeDict.Clear();
            userBackupDict.Clear();

            userTreeView.Nodes.Clear();
            appListView.Items.Clear();
            rolaListView.Items.Clear();

            currentSelectedUser = null;
            previousSelectedUser = null;
            currentSelectedApp = null;
            previousSelectedApp = null;
            previousCheckedRola = null;
            currentCheckedRola = null;
            userTreeViewMouseClicked = false;
            appListMouseClicked = false;
            rolaListMouseClicked = false;

            saveButton.Enabled = false;
            saveAndCloseButton.Enabled = false;
            statusInformationButton.Enabled = false;

            readAllData();
            setupAdminForm();
        }

    }
}
