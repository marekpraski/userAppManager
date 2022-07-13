using DatabaseInterface;
using System;
using System.Collections.Generic;
using System.Data;

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
        public Dictionary<string, DesktopProfile> profileDict { get; private set; }      //słownik wszystkich profili zdefiniowanych w Desktopie, kluczem jest id
        public Dictionary<string, App> appDictionary { get; private set; }              //lista wszystkich aplikacji zdefiniowanych w desktopie, kluczem jest Id
        public Dictionary<string, Rola> rolaDict { get; private set; }                 //lista wszystkich ról aplikacji, kluczem jest Id_rola
        public Dictionary<string, AppModule> moduleDict { get; private set; }        //lista wszystkich modułów aplikacji, kluczem jest ID_mod

        private DesktopUser user = LoginForm.user;

        public DesktopDataHandler()
        {
            this.dbReader = new DBReader(LoginForm.dbConnection);
            createDictionaries();
            if (user.type == UserType.Administrator)
                readAllData();
            else if (user.type == UserType.RegularUser)
                readRegularUserData();
            else if (user.type == UserType.Undefined)
                readUndefinedUserData();
        }

        #region metody podczas inicjalizacji klasy
        private void createDictionaries()
        {
            allUsersDict = new Dictionary<string, DesktopUser>();
            profileDict = new Dictionary<string, DesktopProfile>();
            appDictionary = new Dictionary<string, App>();
            rolaDict = new Dictionary<string, Rola>();
            moduleDict = new Dictionary<string, AppModule>();
        }

        private void readAllData()
        {
            getUsers();
            getAllProfiles();
            string appQuery = @"select ap.ID_app, ap.show_name, ap.name_app, ap.path_app, ap.name_db, ap.srod_app, ap.variant, ap.runFromDesktop from [dbo].[app_list] as ap ";
            getAllApps(appQuery);
            getAppModules();
            getRola();
            getProfileApps();
            getUsersApps();
        }
        private void readRegularUserData()
        {
            setUser();
            getUserProfiles();
            string appQuery = @"select ap.ID_app, ap.name_app, ap.path_app, ap.show_name, ap.name_db, ap.runFromDesktop from [dbo].[app_list] as ap 
                            inner join app_users as au on ap.ID_app = au.ID_app 
                            where ap.name_db is not null and srod_app = 'Windows' and ap.runFromDesktop = 1 
                            and ap.show_name  is not null and au.ID_user = " + user.id;
            getAllApps(appQuery);
            getAppModules();
            getRola();
            getProfileApps();
            getUsersApps();
        }

        internal bool insertProfileToDB(DesktopProfile profile)
        {
            string query = "  insert into [profile_desktop] ([name_profile], [domena], [ldap], [logo_profile] ) VALUES ('" + profile.name +
                "', '" + profile.domena + "', '" + profile.ldap + "',  @logoImageBytes )";
            if (runParameterisedQuery(query, profile.logoImageAsBytes))
                return true;
            
            return false;
        }

        internal bool updateProfileInDB(DesktopProfile profile)
        {
            string query = "update [profile_desktop] set name_profile = '" + profile.name +
            "', domena = '" + profile.domena + "', ldap = '" + profile.ldap + "' , [logo_profile] =  @logoImageBytes where ID_profile = " + profile.id;
            if (runParameterisedQuery(query, profile.logoImageAsBytes))
                return true;
            
            return false;
        }
        /// <summary>
        /// jeżeli puszczam zwykłą kwerendę to nie chce zapisywać obrazka w formacie byte[]
        /// </summary>
        private bool runParameterisedQuery(string query, byte[] imageBytes)
        {
            DBWriter dbwriter = new DBWriter(LoginForm.dbConnection);
            dbwriter.initiateParameterizedCommand();
            dbwriter.addCommmandParameter("@logoImageBytes", SqlDbType.VarBinary, imageBytes);
            return dbwriter.executeQuery(query);
        }

        private void readUndefinedUserData()
        {
            getUsers();
            getAllProfiles();
        }

        #endregion

        #region czytanie użytkowników z bazy danych 
        private void getUsers()
        {
            string query = "select  ID_user, imie_user, nazwisko_user,login_user, windows_user from users_list where login_user is not null and login_user <> '" + user.sqlLogin + "'";
            QueryData userData = dbReader.readFromDB(query);

            for (int i = 0; i < userData.dataRowsNumber; i++)
            {
                DesktopUser desktopUser = new DesktopUser();
                desktopUser.firstName = userData.getDataValue(i, "imie_user").ToString();
                desktopUser.surname = userData.getDataValue(i, "nazwisko_user").ToString();
                desktopUser.sqlLogin = userData.getDataValue(i, "login_user").ToString();
                desktopUser.windowsLogin = userData.getDataValue(i, "windows_user").ToString();
                desktopUser.id = userData.getDataValue(i, "ID_user").ToString();
                allUsersDict.Add(desktopUser.id, desktopUser);
            }
        }

        internal DesktopProfile readProfileFromDB(string profileId)
        {
            DesktopProfile newProfile = new DesktopProfile();
            string query = "select ID_profile, name_profile, domena, ldap from [profile_desktop] where ID_profile = " + profileId;
            QueryData qd = new DBReader(LoginForm.dbConnection).readFromDB(query);
            newProfile.id = qd.getDataValue(0, "ID_profile").ToString();
            newProfile.name = qd.getDataValue(0, "name_profile").ToString();
            newProfile.domena = qd.getDataValue(0, "domena").ToString();
            newProfile.ldap = qd.getDataValue(0, "ldap").ToString();
            newProfile.logoImageAsBytes = readLogoImage(newProfile.id);

            return newProfile;
        }

        /// <summary>
        /// w przypadku konkretnie zalogowanego zwykłego użytkowonika dodaję go tylko do słownika
        /// </summary>
        private void setUser()
        {
            allUsersDict.Add(user.id, user);
        }

        #endregion

        #region czytanie profili z bazy danych
        private void getAllProfiles()
        {
            string query = @"  select ID_profile, name_profile, domena, ldap from [profile_desktop]";
            string query2 = @"SELECT  ID_profile, lu.ID_user FROM [profile_users] pu
                                inner join 
                                users_list lu on pu.ID_user = lu.ID_user
                                where login_user is not null and login_user <> '" + user.sqlLogin + "'";
            QueryData[] qd = new DBReader(LoginForm.dbConnection).readFromDB(new string[] { query, query2 });

            addProfilesToDict(qd[0]);
            assignUsersToProfiles(qd[1]);
        }
        private void getUserProfiles()
        {
            string query = @"select pd.ID_profile, pd.name_profile, pd.domena, pd.ldap from [profile_desktop] pd
                            inner join 
                            profile_users pu on pu.ID_profile = pd.ID_profile
                            where pu.ID_user = " + user.id;
            QueryData qd = new DBReader(LoginForm.dbConnection).readFromDB(query);
            addProfilesToDict(qd);
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
                newProfile.logoImageAsBytes = readLogoImage(id);
                this.profileDict.Add(id, newProfile);
            }
        }

        private byte[] readLogoImage(string id)
        {
            string query = "select logo_profile from [profile_desktop] where ID_profile = " + id;
            return new DBReader(LoginForm.dbConnection).readScalarFromDB(query) as byte[];
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

        private void getAllApps(string query)
        {
            UtilityTools.NumberHandler nh = new UtilityTools.NumberHandler();
            QueryData appData = dbReader.readFromDB(query);

            for (int i = 0; i < appData.dataRowsNumber; i++)
            {
                App app = new App();
                app.id = appData.getDataValue(i, "ID_app").ToString();
                app.name = appData.getDataValue(i, "name_app").ToString();
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

        private void getRola()
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
                if (profileDict.ContainsKey(profileId))
                {
                    string appId = qd.getDataValue(i, "ID_app").ToString();
                    if (appDictionary.ContainsKey(appId))
                    {
                        string appProfileParams = qd.getDataValue(i, "app_params").ToString();
                        AppProfileParameters appParams = new AppProfileParameters(profileId, appId, appProfileParams);
                        appDictionary[appId].addAppProfileParameters(appParams);
                        if(appDictionary[appId].isValid)
                            profileDict[profileId].addAppToProfile(appDictionary[appId]);
                    }
                }
            }
        }

        #endregion

        #region czytanie z bazy danych ról użytkowników w aplikacjach
        //dane każdego użytkownika uzupełniam o zestawienie aplikacji do których ma uprawnienia wraz z rolami
        private void getUsersApps()
        {
            DesktopUser user = null;

            foreach (string userId in allUsersDict.Keys)
            {
                string query = @"select ap.ID_app from [dbo].[app_list] as ap 
                                inner join app_users as au on ap.ID_app = au.ID_app 
                                inner join users_list as ul on ul.ID_user = au.ID_user 
                                where au.Grant_app > 0 and ul.ID_user = " + userId;
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
