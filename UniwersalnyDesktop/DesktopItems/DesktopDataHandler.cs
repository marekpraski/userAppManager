using DatabaseInterface;
using System.Collections.Generic;

namespace UniwersalnyDesktop
{
    /// <summary>
    /// czyta dane elementów Desktopu z bazy danych oraz zapisuje zmiany do bazy
    /// </summary>
    public class DesktopDataHandler
    {
        private DBReader dbReader;
        //
        //słowniki danych podstawowych
        //
        public Dictionary<string, DesktopUser> allUsersDict { get; private set; }            //lista wszystkich użytkowników desktopu, kluczem jest Id
        public Dictionary<string, DesktopUser> profileUsersDict { get; private set; }             //lista użytkowników sql, kluczem jest Id, wartością nazwa użytkownika wyświetlana w drzewie
        public Dictionary<string, DesktopProfile> profileDict { get; private set; }      //słownik wszystkich profili zdefiniowanych w Desktopie, kluczem jest id
        public Dictionary<string, App> appDictionary { get; private set; }              //lista wszystkich aplikacji zdefiniowanych w desktopie, kluczem jest Id
        public Dictionary<string, Rola> rolaDict { get; private set; }                 //lista wszystkich ról aplikacji, kluczem jest Id_rola
        public Dictionary<string, AppModule> moduleDict { get; private set; }        //lista wszystkich modułów aplikacji, kluczem jest ID_mod

        private readonly string adminLogin;

        public DesktopDataHandler(string adminLogin)
        {
            this.dbReader = new DBReader(LoginForm.dbConnection);
            this.adminLogin = adminLogin;
            createDictionaries();
            readAllData();
        }
        #region metody podczas inicjalizacji klasy
        private void createDictionaries()
        {
            allUsersDict = new Dictionary<string, DesktopUser>();
            profileUsersDict = new Dictionary<string, DesktopUser>();

            profileDict = new Dictionary<string, DesktopProfile>();
            appDictionary = new Dictionary<string, App>();
            rolaDict = new Dictionary<string, Rola>();
            moduleDict = new Dictionary<string, AppModule>();
        }

        private void readAllData()
        {
            getUserData();
            getProfileData();
            getAppData();
            getAppModules();
            getRolaData();
            getProfileApps();
            getUserApps();
        } 
        #endregion

        #region czytanie użytkowników z bazy danych 
        private void getUserData()
        {
            string query = "select  ID_user, imie_user, nazwisko_user,login_user, windows_user from users_list where login_user is not null and login_user <> '" + adminLogin + "'";
            QueryData userData = dbReader.readFromDB(query);

            for (int i = 0; i < userData.dataRowsNumber; i++)
            {

                DesktopUser desktopUser = new DesktopUser();
                desktopUser.name = userData.getDataValue(i, "imie_user").ToString();
                desktopUser.surname = userData.getDataValue(i, "nazwisko_user").ToString();
                desktopUser.sqlLogin = userData.getDataValue(i, "login_user").ToString();
                desktopUser.windowsLogin = userData.getDataValue(i, "windows_user").ToString();
                desktopUser.id = userData.getDataValue(i, "ID_user").ToString();
                allUsersDict.Add(desktopUser.id, desktopUser);
            }
        }

        #endregion

        #region czytanie profili z bazy danych

        private void getProfileData()
        {
            string query = @"  select ID_profile, name_profile, domena, ldap from [profile_desktop]";
            string query2 = @"SELECT  ID_profile, lu.ID_user FROM [profile_users] pu
                                inner join 
                                users_list lu on pu.ID_user = lu.ID_user
                                where login_user is not null and login_user <> '" + adminLogin + "'";
            QueryData[] qd = new DBReader(LoginForm.dbConnection).readFromDB(new string[] { query, query2 });

            addProfilesToDict(qd[0]);
            assignUsersToProfiles(qd[1]);
        }

        private void addProfilesToDict(QueryData qd)
        {
            for (int i = 0; i < qd.dataRowsNumber; i++)
            {
                string id = qd.getDataValue(i, "ID_profile").ToString();
                string name = qd.getDataValue(i, "name_profile").ToString();
                DesktopProfile newProfile = new DesktopProfile(id, name);
                newProfile.domena = qd.getDataValue(i, "domena").ToString();
                newProfile.ldap = qd.getDataValue(i, "ldap").ToString();
                this.profileDict.Add(id, newProfile);
            }
        }

        private void assignUsersToProfiles(QueryData qd)
        {
            for (int i = 0; i < qd.dataRowsNumber; i++)
            {
                string profileId = qd.getDataValue(i, "ID_profile").ToString();
                string userId = qd.getDataValue(i, "ID_user").ToString();
                profileDict[profileId].addUserToProfile(allUsersDict[userId]);
            }
        }
        #endregion

        #region czytanie aplikacji, modułów i ról z bazy danych

        private void getAppData()
        {
            UtilityTools.NumberHandler nh = new UtilityTools.NumberHandler();
            string query = @"select ap.ID_app, ap.show_name, ap.name_app, ap.path_app, ap.name_db, ap.srod_app, ap.variant, ap.runFromDesktop from [dbo].[app_list] as ap ";
            QueryData appData = dbReader.readFromDB(query);

            for (int i = 0; i < appData.dataRowsNumber; i++)
            {

                App app = new App();
                app.id = appData.getDataValue(i, "ID_app").ToString();
                app.displayName = appData.getDataValue(i, "show_name").ToString();
                app.defaultDatabaseName = appData.getDataValue(i, "name_db").ToString();
                app.executionPath = appData.getDataValue(i, "path_app").ToString();
                app.runFromDesktop = nh.tryGetBool(appData.getDataValue(i, "runFromDesktop"));
                appDictionary.Add(app.id, app);
            }
        }

        private void getAppModules()
        {
            App app;

            string query = "select ID_mod, ID_app, name_mod from mod_app";
            QueryData moduleData = dbReader.readFromDB(query);

            for (int i = 0; i < moduleData.dataRowsNumber; i++)
            {
                AppModule module = new AppModule();
                module.id = moduleData.getDataValue(i, "ID_mod").ToString();
                module.name = moduleData.getDataValue(i, "name_mod").ToString();
                module.appId = moduleData.getDataValue(i, "ID_app").ToString();

                appDictionary.TryGetValue(module.appId, out app);
                app.addModule(module);
                moduleDict.Add(module.id, module);
            }
        }

        private void getRolaData()
        {
            App app;

            string query = @"select ra.ID_rola, ra.name_rola, ra.descr_rola, al.ID_app, al.show_name from [dbo].[rola_app]  as ra 
                            inner join app_list as al on ra.ID_app=al.ID_app";
            QueryData appRolaData = dbReader.readFromDB(query);

            for (int i = 0; i < appRolaData.dataRowsNumber; i++)
            {

                Rola rola = new Rola();
                rola.id = appRolaData.getDataValue(i, "ID_rola").ToString();
                rola.appId = appRolaData.getDataValue(i, "ID_app").ToString();
                rola.name = appRolaData.getDataValue(i, "name_rola").ToString();
                rola.description = appRolaData.getDataValue(i, "descr_rola").ToString();
                rola.appName = appRolaData.getDataValue(i, "show_name").ToString();

                getRolaAppModules(rola);

                rolaDict.Add(rola.id, rola);

                //dodaję role aplikacji w każdej aplikacji
                appDictionary.TryGetValue(rola.appId, out app);
                app.addRola(rola);
            }
        }

        private void getRolaAppModules(Rola rola)
        {
            AppModule module;
            string query = "select ID_mod, Grant_app from[dbo].[rola_upr] where ID_rola= " + rola.id;
            QueryData rolaModuleData = dbReader.readFromDB(query);
            if (rolaModuleData.dataRowsNumber > 0)
            {
                for (int i = 0; i < rolaModuleData.dataRowsNumber; i++)
                {
                    moduleDict.TryGetValue(rolaModuleData.getDataValue(i, "ID_mod").ToString(), out module);
                    string grantApp = rolaModuleData.getDataValue(i, "Grant_app").ToString();
                    rola.addModule(module, grantApp);
                }
            }
        }

        private void getProfileApps()
        {
            string query = "  select ID_profile, ID_app, app_params from [profile_app]";
            QueryData qd = dbReader.readFromDB(query);
            for (int i = 0; i < qd.dataRowsNumber; i++)
            {
                string profileId = qd.getDataValue(i, "ID_profile").ToString();
                string appId = qd.getDataValue(i, "ID_app").ToString();
                string appProfileParams = qd.getDataValue(i, "app_params").ToString();
                AppProfileParameters appParams = new AppProfileParameters(profileId, appId, appProfileParams);
                appDictionary[appId].addAppProfileParameters(appParams);
                profileDict[profileId].addAppToProfile(appDictionary[appId]);
            }
        }

        #endregion

        #region czytanie z bazy danych ról użytkowników w aplikacjach
        //dane każdego użytkownika uzupełniam o zestawienie aplikacji do których ma uprawnienia wraz z rolami
        private void getUserApps()
        {
            DesktopUser user = null;

            foreach (string userId in allUsersDict.Keys)
            {
                string query = @"select ap.ID_app from [dbo].[app_list] as ap 
                                inner join app_users as au on ap.ID_app = au.ID_app 
                                inner join users_list as ul on ul.ID_user = au.ID_user 
                                where ap.show_name is not null and au.Grant_app = 1 and ul.ID_user = '" + userId + "'";
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
            string baseQuery = @"select ID_rola  from rola_app as ra 
                                inner join app_list as ap on ap.ID_app = ra.ID_app 
                                where ap.ID_app = @appId and 
                                ID_rola in (select ID_rola from rola_users where ID_user = @userId)";
            string query = baseQuery.Replace("@appId", appId).Replace("@userId", userId);
            QueryData windowsUserData = dbReader.readFromDB(query);
            List<string> rolaList = convertColumnDataToList(windowsUserData.getQueryDataAsStrings());
            if (rolaList.Count > 0)
            {
                return rolaList[0];         //zawsze jest tylko jedna rola
            }
            return "";
        }
        #endregion

        #region Region : zapisywanie zmian do bazy

        public void saveChanges(Dictionary<string, DesktopUser> userBackupDict, Dictionary<DesktopUser, Dictionary<App, AppDataItem>> userAppChangeDict)
        {
            ChangedDataBundle changedDataBundle = new ChangedDataBundle(userAppChangeDict, userBackupDict);

            DBWriter writer = new DBWriter(LoginForm.dbConnection);
            string query = generateQuery(changedDataBundle);
            MyMessageBox.display(query);
            writer.executeQuery(query);
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
            string updateUserRola = "update rola_users set ID_rola = @newRolaId where ID_rola = @oldRolaId and ID_User = @userId; ";

            string deleteUserApp = "update app_users set Grant_app=0 where ID_app = @appId and ID_user = @userId; ";

            string deleteUserAppAndRola = @"delete from rola_users where ID_rola=@rolaId and ID_user=@userId; 
                                            update app_users set Grant_app=0 where ID_app = @appId and ID_user = @userId; ";

            string insertUserApp = @"update app_users set Grant_app=1 where ID_app = @appId and ID_user = @userId; ";

            string insertUserAppAndRola = @"update app_users set Grant_app=1 where ID_app = @appId and ID_user = @userId; 
                                            insert into rola_users(ID_rola, ID_user, descr) values(@rolaId, @userId, null); ";

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
                        query = deleteUserApp.Replace("@appId", oldAppId).Replace("@userId", user.id) + "\r\n";
                    }
                    else
                    {
                        query = deleteUserAppAndRola.Replace("@appId", oldAppId).Replace("@rolaId", oldRolaId).Replace("@userId", user.id) + "\r\n";
                    }
                    break;
                case "update":
                    query = updateUserRola.Replace("@newRolaId", newRolaId).Replace("@oldRolaId", oldRolaId).Replace("@userId", user.id) + "\r\n";
                    break;
                case "insert":
                    if (newRolaId.Equals(""))
                    {
                        query = insertUserApp.Replace("@appId", newAppId).Replace("@userId", user.id) + "\r\n";
                    }
                    else
                    {
                        query = insertUserAppAndRola.Replace("@appId", newAppId).Replace("@rolaId", newRolaId).Replace("@userId", user.id) + "\r\n";
                    }
                    break;
            }
            return query;
        }

        #endregion

        #region metody pomocnicze
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
        #endregion
    }
}
