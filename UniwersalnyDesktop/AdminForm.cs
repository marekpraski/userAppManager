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

        //
        //słowniki danych podstawowych
        //
        private Dictionary<string, DesktopUser> allUsersDict = null;            //lista wszystkich użytkowników desktopu, kluczem jest Id
        private Dictionary<string, string> sqlUsersDict = null;            //lista użytkowników sql, kluczem jest Id, wartością nazwa użytkownika wyświetlana w drzewie
        private Dictionary<string, string> windowsUsersDict = null;      //lista użytkowników domenowych, kluczem jest Id, , wartością nazwa użytkownika wyświetlana w drzewie
        private Dictionary<string, App> appDictionary = null;               //lista wszystkich aplikacji zdefiniowanych w desktopie, kluczem jest Id
        private Dictionary<string, Rola> rolaDict = null;                //lista wszystkich ról aplikacji, kluczem jest Id_rola


        private Dictionary<string, string> duplicatedWindowsUsers = null;        //jeżeli loginy  domenowe się powtarzają to ten program nie może działać poprawnie
                                                                                //wychwytuję powtarzające się loginy i je wyświetlam

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


        private bool rolaListViewLoaded = false;                 //bez tej zmiennej event AppRoleListView_ItemChecked jest uruchamiany podczas ładowania listy 
                                                                    //AppRoleListView tyle razy ile jest wpisów na liście roli aplikacji
                                                                    //co powoduje widoczne opóźnienie w wyświetleniu tej listy

        //
        //zapamiętywanie zmian
        //
        private Dictionary<string, DesktopUser> userChangesDict = null;       //używana do trzymania oryginałów, gdyby po dokonaniu zmian 
                                                                                                //użytkownik zrezygnował z ich zapisania do bazy

        private Dictionary<string, string> saveToDbDict = null;                     //zapisuję kwerendy  zapisujące zmiany do bazy
                                                                                    //klucz = kombinacja loginu użytkownika + nazwa aplikacji
                                                                                    //wartość - kwerenda

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
            populateUserTreeview();                                 //użytkownicy sql i domenowi
            populateAppListview();                                   //aplikacje

            //przygotowanie do zapisywania zmian
            userChangesDict = new Dictionary<string, DesktopUser>();
            saveToDbDict = new Dictionary<string, string>();           
        }

        #region Region - wczytywanie danych na starcie formularza
        private void readAllData()
        {
            getUserData();
            getAppData();
            getRolaData();
            getUserApps();
        }
        

        private void getUserData()
        {
            allUsersDict = new Dictionary<string, DesktopUser>();
            string query = ProgramSettings.userQueryTemplate + "'" + adminLogin + "'";
            List<string[]> userData = dbReader.readFromDB(query).getQueryDataAsStrings();

            foreach(string[] data in userData)
            {
                DesktopUser desktopUser = new DesktopUser();
                desktopUser.name = data[ProgramSettings.userImieIndex];
                desktopUser.surname = data[ProgramSettings.userNazwiskoIndex];
                desktopUser.sqlLogin = data[ProgramSettings.userSqlLoginIndex];
                desktopUser.windowsLogin = data[ProgramSettings.userWindowsLoginIndex];
                desktopUser.id = data[ProgramSettings.userIdIndex];
                allUsersDict.Add(data[ProgramSettings.userIdIndex], desktopUser);
            }
            groupUsers();

            //usuwam ze słowników wszystkich zduplikowanych użytkowników, bo w kolejnych kwerendach wyszukujących programów dla użytkowników wyniki są niejednoznaczne
            removeDuplicatedWindowsUsers();
        }

       
        private void groupUsers()
        {
            string userDisplayName = "";
            DesktopUser user = null;

            sqlUsersDict = new Dictionary<string, string>();
            windowsUsersDict = new Dictionary<string, string>();
            duplicatedWindowsUsers = new Dictionary<string, string>();

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
            appDictionary = new Dictionary<string, App>();
            string query = ProgramSettings.appListQueryTemplate;
            List<string[]> appData = dbReader.readFromDB(query).getQueryDataAsStrings(); 

            foreach (string[] data in appData)
            {
                App app = new App();
                app.Id = data[ProgramSettings.appIdIndex];
                app.appDisplayName = data[ProgramSettings.appDisplayNameIndex];
                appDictionary.Add(data[ProgramSettings.appIdIndex], app);
            }
        }

        private void getRolaData()
        {
            rolaDict = new Dictionary<string, Rola>();
            App app = null;

            string query = ProgramSettings.rolaQueryTemplate;
            List<string[]> appRolaData = dbReader.readFromDB(query).getQueryDataAsStrings();
            foreach (string[] rolaData in appRolaData)
            {
                Rola rola = new Rola();
                rola.idRola = rolaData[ProgramSettings.rolaIdIndex];
                rola.appId = rolaData[ProgramSettings.rolaAppIdIndex];
                rola.name = rolaData[ProgramSettings.rolaNameIndex];
                rola.description = rolaData[ProgramSettings.rolaDescrIndex];
                rola.appName = rolaData[ProgramSettings.rolaAppNameIndex];
                rolaDict.Add(rolaData[ProgramSettings.rolaIdIndex], rola);

                //dodaję role aplikacji w każdej aplikacji
                appDictionary.TryGetValue(rola.appId, out app);
                app.addRola(rola.idRola);
            }
        }

        //dane każdego użytkownika uzupełniam o zestawienie aplikacji do których ma uprawnienia wraz z rolami
        private void getUserApps()
        {
            DesktopUser user = null;
            
            foreach (string userId in allUsersDict.Keys)
            {
                string query = ProgramSettings.userAppsQueryTemplate + "'" + userId + "'";
                List<string[]> appList = dbReader.readFromDB(query).getQueryDataAsStrings();
                List<string> appIdList = convertColumnDataToList(appList);                  //kwerenda zwraca pojedynczą listę, tj tylko id

                allUsersDict.TryGetValue(userId, out user);
                if (appIdList.Count > 0)
                {
                    foreach (string appId in appIdList)
                    {
                        string appRolaId = getAppRola(userId, appId);
                        //if (appRolaId != "")
                        //{
                            user.addUpdateAppRola(appId, appRolaId);
                        //}
                    }
                }
            }
        }


        private string getAppRola(string userId, string appId)
        {
            string query = ProgramSettings.userAppRolaQueryTemplate.Replace("@appId", appId).Replace("@userId", userId);
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
            currentSelectedUser = userTreeView.SelectedNode;

            // resetuje ustawienia odfajkowując wszystkie checkboxy
            uncheckAllApps();
            uncheckAppRolaCheckbox();

            if (!currentSelectedUser.Name.Equals(""))          //"" oznacza, że zaznaczona zostaje nazwa gałęzi
            {
                //wyszukuję aplikacji zaznaczonego użytkownika
                DesktopUser user = null;
                allUsersDict.TryGetValue(currentSelectedUser.Name, out user);              //parametr "name" zawiera Id użytkownika
                List<string> userApps = user.getApps();

                //jeżeli user ma jakieś uprawnienia to te aplikacje zafajkowuję oraz rolę zaznaczonej
                if (userApps != null)
                {
                    foreach (string appId in userApps)
                    {
                        ListViewItem app = appListView.Items[appId];
                        app.Checked = true;
                    }
                    checkRolaCheckbox();
                }
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

        #endregion


        #region Region - lista programów
        private void populateAppListview()
        {
            App app = null;
            foreach (string appId in appDictionary.Keys)
            {
                appDictionary.TryGetValue(appId, out app);
                ListViewItem listRow = new ListViewItem(app.appDisplayName);
                listRow.Name = appId;
                appListView.Items.Add(listRow);
            }
        }


        //zaznaczenie aplikacji na liście appListView powoduje wypełnienie roli tej aplikacji w appRoleListView oraz zafajkowanie checkboxa roli, którą ma ten użytkowni
        //nie używam zdarzenia "SelectedIndexChanged" bo nie zapewnia mi żądanej funkcjonalności
        private void AppListView_Click(object sender, EventArgs e)
        {
            setCurrentSelectedApp();
            if (currentSelectedApp != previousSelectedApp)
            {
                updateRolaViewBox();
            }
            else
            {
                checkRolaCheckbox();         //jeżeli zaznaczona aplikacja się nie zmieniła to aktualizuję tylko role dla niej
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

        //zmienia kolor zaznaczonego po kliknięciu w inne okno; żeby działało parametr HideSelection musi być true
        private void AppListView_Leave(object sender, EventArgs e)
        {
            currentSelectedApp.BackColor = Color.Aqua;
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


        #region Region - lista ról aplikacji

        //widok listy ról nie jest wypełniany na starcie, lecz
        //operacja występuje, gdy kliknięta zostanie aplikacja na liście aplikacji
        private void updateRolaViewBox()
        {
            try
            {
                //resetuję ustawienia widoku listy ról aplikacji
                rolaListView.Items.Clear();
                rolaListViewLoaded = false;
                previousCheckedRola = null;
                currentCheckedRola = null;
                resetPreviousSelectedAppColour();

                //wypełniam listę rolami, jeżeli są
                populateRolaListView();

                //jeżeli zaznaczony użytkownik ma uprawnienia do zaznaczonej aplikacji, wówczas zaznacza się rola tego użytkownika w tej aplikacji
                checkRolaCheckbox();

            }
            //błędy do obsłużenia warunkami w debugu
            catch (NullReferenceException ex)
            {
#if DEBUG
                MyMessageBox.display(ex.Message + " NullReference updateRolaViewBox", MessageBoxType.Error);
#endif
            }
            catch (ArgumentOutOfRangeException exc)
            {
#if DEBUG
                MyMessageBox.display(exc.Message + " OutOfRange updateRolaViewBox", MessageBoxType.Error);
#endif
            }
        }

        private void populateRolaListView()
        {
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
                rolaListViewLoaded = true;
            }
        }

        //zafajkowuje rolę zaznaczonej aplikacji jeżeli zaznaczony użytkownik ma do niej uprawnienia
        private void checkRolaCheckbox()
        {
            if (rolaListViewLoaded)          //bez tego warunku gdy wybiorę użytkownika to wyrzuca wyjątek, bo chce zaznaczyć rolę a ich jeszcze nie ma w oknie
            {
                DesktopUser user = null;
                allUsersDict.TryGetValue(currentSelectedUser.Name, out user);
                string rolaId = user.getRola(currentSelectedApp.Name);

                ListViewItem rolaItem = null;

                if (!rolaId.Equals(""))
                {
                    rolaItem = rolaListView.Items[rolaId];

                    if (currentSelectedApp.Checked == true)     //użytkownik ma uprawnienia do zaznaczonej aplikacji
                    {
                        rolaItem.Checked = true;        //warunek, że zaznaczona może być tylko jedna rola obsługuje zdarzenie AfterChecked
                        setCurrentlyCheckedRola();      //aktualizuję zmienną currentlyCheckedRola
                    }
                    else
                    {
                        rolaItem.Checked = false;
                    }
                }
            }
        }

        private void uncheckAppRolaCheckbox()
        {
            if (currentCheckedRola != null)
            {                                                   
                currentCheckedRola.Checked = false;
                currentCheckedRola = null;
                previousCheckedRola = null;
            }
        }

        //aktualizuję zmienną currentlyCheckedRola
        private void setCurrentlyCheckedRola()
        {
            ListView.CheckedListViewItemCollection checkedRoles = rolaListView.CheckedItems;
            if (checkedRoles.Count > 0)
            {
                currentCheckedRola = checkedRoles[0];                //zawsze będzie tylko jedna, więc zawsze na pozycji 0   
            }
        }


        
        private void rolaListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (rolaListViewLoaded)      //uruchamiam kod metody tylko wtedy, gdy do listy władowane są wszystkie wpisy
            {
                string userId = currentSelectedUser.Name;
                if (!userId.Equals(""))                 //muszę tu też sprawdzić, bo odznaczanie checkboxa roli w przypadku zaznaczenia gałęzi wyzwala to zdarzenie i wywala się błąd na ID użytkownika==""
                {
                    try
                    {
                        previousCheckedRola = currentCheckedRola;
                        if (previousCheckedRola != null)
                        {
                            //upewniam się, że checkbox jest zaznaczony tylko przy jednej roli
                            previousCheckedRola.Checked = false;
                        }
                        if (currentCheckedRola != null && previousCheckedRola != null)     //oba są null tylko na początku ładowania listy, po pierwszym załadowaniu tylko previousCheckedRole jest null
                                                                                           //chyba że użytkownik nie ma uprawnień do aplikacji, ale to jest sprawdzane wcześniej
                        {
                            setCurrentlyCheckedRola();
                        }
                    }
                    catch (ArgumentNullException ex)
                    {
                        MyMessageBox.display(ex.Message + "  ArgumentNull AppRoleListView_ItemChecked", MessageBoxType.Error);
                    }
                    catch (NullReferenceException exc)
                    {
                        MyMessageBox.display(exc.Message + "  NullReference AppRoleListView_ItemChecked", MessageBoxType.Error);
                    }
                }
            }
        }


        #endregion


        #region Region : zapamiętywanie zmian i zapisywanie do bazy


        //zapisuję zmiany w uprawnieniach użytkownika po opuszczeniu okna roli
        private void RolaListView_Leave(object sender, EventArgs e)
        {
            string userId = currentSelectedUser.Name;
            updateUserPrivilages(userId);
        }


        //aktualizuje uprawnienia użytkownika
        private void updateUserPrivilages(string userId)
        {
            backupUser(userId);

            DesktopUser user = null;
            allUsersDict.TryGetValue(userId, out user);

            if (currentSelectedApp != null)
            {
               user.addUpdateAppRola(currentSelectedApp.Name, currentCheckedRola.Name);
            }
        }

        private void backupUser(string userId)
        {
            if(!userChangesDict.ContainsKey(userId))        //dodaję tylko raz, na początku, tj oryginał
            {
                DesktopUser user = new DesktopUser();
                allUsersDict.TryGetValue(userId, out user);
                userChangesDict.Add(userId, user);
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

        
        private void HelpButton_Click(object sender, EventArgs e)
        {
            string helpMessage = "Jeżeli aplikacja ma rolę, to odznaczenie roli nie spowoduje zapisania zmian ";
            MyMessageBox.display(helpMessage);
        }
    }
}
