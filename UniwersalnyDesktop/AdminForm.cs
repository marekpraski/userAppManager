using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace UniwersalnyDesktop
{
    public partial class AdminForm : Form
    {

        #region prywatne właściwości

        private DesktopUser user = LoginForm.user;                              // użytkownik, ale z założenia jest to Administrator skoro jest w tym oknie, inna nazwa bo chcę odróżnić od "user" który jest zwykłym użytkownikiem
        private DesktopDataHandler dataHandler;

        //słowniki danych podstawowych
        //
        private Dictionary<string, DesktopUser> allUsersDict;            //lista wszystkich użytkowników desktopu, kluczem jest Id

        private Dictionary<string, DesktopProfile> profileDict;     //słownik wszystkich profili zdefiniowanych w Desktopie, kluczem jest id
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

        public AdminForm()
        {
            InitializeComponent();
            initialSetup();
        }

        private void initialSetup()
        {
            this.dataHandler = new DesktopDataHandler();
            createDictionaries();
            getAllData();
            setupAdminForm();
        }

        #region metody podczas zamykania formularza AdminForm

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

        #region wczytywanie danych na starcie formularza

        private void createDictionaries()
        {
            //przygotowanie do zapisywania zmian
            userBackupDict = new Dictionary<string, DesktopUser>();
            userAppChangeDict = new Dictionary<DesktopUser, Dictionary<App, AppDataItem>>();
        }


        private void setupAdminForm()
        {
            populateProfileListView();
        }

        private void getAllData()
        {
            this.allUsersDict = this.dataHandler.allUsersDict;
            this.profileDict = this.dataHandler.profileDict;
            this.appDictionary = this.dataHandler.appDictionary;
            this.moduleDict = this.dataHandler.moduleDict;
            this.rolaDict = this.dataHandler.rolaDict;
        }

        #endregion

        #region wypełnianie drzewa użytkowników
        public void populateUserTreeview(Dictionary<string, DesktopUser> usersDict)
        {
            userTreeView.Nodes.Clear();
            string[] treeviewBranchNames = { "użytkownicy przypisani do profilu" };
            Dictionary<string, DesktopUser>[] treeviewBranchItems = { usersDict };

            for (int i = 0; i < treeviewBranchNames.Length; i++)            //nie używam foreach bo potrzebuję iteratora do znalezienia nazwy gałęzi
            {
                Dictionary<string, DesktopUser> oneBranchItems = treeviewBranchItems[i];
                TreeNode[] childNodes = populateTreviewBranch(oneBranchItems);
                TreeNode parentNode = new TreeNode(treeviewBranchNames[i], childNodes);
                parentNode.Name = "";                                                    //przypisuę pusty string, żeby mi nie wywalało błędu podczas wyciągania tekstu z taga w metodzie userTreeView_AfterSelect
                userTreeView.Nodes.Add(parentNode);
            }

            userTreeView.ExpandAll();
        }


        public TreeNode[] populateTreviewBranch(Dictionary<string, DesktopUser> items)
        {
            TreeNode[] childNodes = new TreeNode[items.Count];
            int i = 0;
            DesktopUser user = null;
            foreach (string userId in items.Keys)
            {
                items.TryGetValue(userId, out user);
                TreeNode treeNode = new TreeNode(user.displayName + "  ( " + user.sqlLogin + ",  " + user.windowsLogin + " )");
                treeNode.Name = user.id;
                childNodes[i] = treeNode;
                i++;
            }
            return childNodes;
        }

        #endregion

        #region wypełnianie listy profili i aplikacji
        private void populateProfileListView()
        {
            DesktopProfile profile = null;
            foreach (string profileId in profileDict.Keys)
            {
                profileDict.TryGetValue(profileId, out profile);
                ListViewItem listRow = new ListViewItem(profile.name);
                listRow.Name = profileId;
                profileListView.Items.Add(listRow);
            }
        }

        private void populateAppListview()
        {
            appListView.Items.Clear();
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

        #region interakcja z użytkownikiem kliknięcie labeli "Edytuj"

        private void EditAppsLabel_Click(object sender, EventArgs e)
        {
            string query = @"select ap.ID_app, ap.show_name, ap.name_app, ap.path_app, ap.name_db, ap.srod_app, ap.variant from [dbo].[app_list] as ap 
                        inner join app_users as au on ap.ID_app = au.ID_app 
                        inner join users_list as ul on ul.ID_user = au.ID_user                         
                        group by ap.ID_app, ap.name_app, ap.name_app, ap.path_app, ap.show_name, ap.name_db, ap.srod_app, ap.variant";
            AppEditorForm appEditor = new AppEditorForm(query, appDictionary);
            appEditor.ShowDialog();
        }


        private void EditRolaLabel_Click(object sender, EventArgs e)
        {
            if (currentSelectedApp != null)
            {
                App app;
                appDictionary.TryGetValue(currentSelectedApp.Name, out app);
                string query = @"select ra.ID_rola, ra.name_rola, ra.descr_rola, al.ID_app, al.appDisplayName from [dbo].[rola_app]  as ra 
                                inner join app_list as al on ra.ID_app=al.ID_app 
                                where al.ID_app = " + app.id;

                RolaEditorForm dbRolaEditor = new RolaEditorForm(query, app);
                dbRolaEditor.ShowDialog();
            }
            else
            {
                MyMessageBox.display("należy zaznaczyć aplikację");
            }
        }


        private void EditUsersLabel_Click(object sender, EventArgs e)
        {
            string query = "select  ID_user, imie_user, nazwisko_user,login_user, windows_user from users_list where login_user is not null and login_user <> '" + user.sqlLogin + "'";
            UserEditorForm userEditor = new UserEditorForm(query);
            userEditor.ShowDialog();
        }

        private void editProfileLabel_Click(object sender, EventArgs e)
        {
            ProfileEditor profileEditor = new ProfileEditor(this.profileDict, this.allUsersDict, this.appDictionary);
            profileEditor.Show();
        }

        #endregion

        #region Region - interakcja z użytkownikiem - zdarzenia na drzewie użytkowników


        //podświetla zaznaczony wiersz w drzewie na wybrany kolor gdy użytkownik kliknie w inny obiekt w formularzu
        //domyślnie ten kolor jest bladoszary, dla mnie zbyt niewidoczny
        private void userTreeView_Leave(object sender, EventArgs e)
        {
            if (currentSelectedUser == null)
                return;

            currentSelectedUser.BackColor = Color.Aqua;
            currentSelectedUser.ForeColor = Color.Black;
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
            if(currentSelectedApp != null)
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

        #region interakcja - zmiana wyboru profilu
        private void profileListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (profileListView.SelectedItems.Count == 0)
                return;
            ListViewItem selectedItem = profileListView.SelectedItems[0];
            string profileId = selectedItem.Name;
            appDictionary = profileDict[profileId].getAppDictionary();
            populateUserTreeview(profileDict[profileId].getUserDictionary());
            populateAppListview();
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
            //ale dany profil może nie zawierać tej aplikacji, więc to też sprawdzam
            if (userApps != null)
            {
                foreach (App app in userApps)
                {
                    ListViewItem appItem = appListView.Items[app.id];
                    if(appItem != null)
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
            this.dataHandler.saveChanges(userBackupDict, userAppChangeDict);
        }

        #endregion

        #region interakcja - wybór pozycji menu Ustawienia systemowe
        private void aktualizujWpisyBibliotekMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Co ja mam robić i po co???");
            //string appPath = Application.StartupPath;
            //string[] paths = { @"\..\lib\" };
            //string targetDirectory = appPath + Path.Combine(paths);

            //string[] fileEntries = Directory.GetFiles(@targetDirectory, "*.*", SearchOption.AllDirectories);
            //foreach (string lib in fileEntries)
            //{
            //    try
            //    {
            //        System.EnterpriseServices.Internal.Publish pb = new System.EnterpriseServices.Internal.Publish();
            //        pb.GacInstall(lib);
            //        MessageBox.Show("Biblioteki zostały zaktualizowane", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    catch (Exception ex)
            //    {
            //        UtilityTools.MessageBoxError.ShowBox("Biblioteki zostały zaktualizowane", "Błąd", ex.Message + ex.StackTrace);
            //    }
            //}
        }

        ///  <summary>
        ///  constant for registering and authentication of users
        ///  </summary>
        ///
        const int HWND_BROADCAST = 0xffff;
        const uint WM_SETTINGCHANGE = 0x001a;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg,
            UIntPtr wParam, string lParam);

        private void ustawZmiennaSrodowiskowaMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var envKey = Registry.LocalMachine.OpenSubKey(
                    @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    true))
                {
                    Contract.Assert(envKey != null, @"registry key is missing!");

                    string appPath = Application.StartupPath;
                    string[] paths = { @"\config.xml" };
                    string path = appPath + Path.Combine(paths);

                    envKey.SetValue("desktopConfFile", path);
                    SendNotifyMessage((IntPtr)HWND_BROADCAST, WM_SETTINGCHANGE, (UIntPtr)0, "Environment");
                }

                MessageBox.Show("Zmienna została ustawiona", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                UtilityTools.MessageBoxError.ShowBox("Zmienna nie została ustawiona", "Błąd", ex.Message + ex.StackTrace);
            }
        }

        private void ZarzadzajProfilamiMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region metody pomocnicze

        private void resetAdminForm()
        {
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

            initialSetup();
        }

        #endregion

    }
}
