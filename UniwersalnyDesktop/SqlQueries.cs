using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public class SqlQueries
    {

        //
        //wypełnianie okna DesktopForm
        //

        public static string getDesktopAppData = "select ap.ID_app, ap.appName, ap.appPath, ap.appDisplayName, au.Grant_app, ap.name_db from [dbo].[app_list] as ap " +
                                                    "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                    "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                    "where ap.name_db is not null and srod_app = 'Windows' and ul.login_user = ";

        public static string getDesktopUserData = "select  login_user, windows_user, imie_user, nazwisko_user from users_list " +
                                                            "where login_user = ";
  

        
        //
        // wypełnianie okna AdminForm
        //

        //wszyscy użytkownicy za wyjątkiem Administratora
        public static string getUsers = "select  login_user, windows_user, imie_user, nazwisko_user,ID_user from users_list where login_user is not null and login_user <> ";
        public static int userSqlLoginIndex = 0;
        public static int userWindowsLoginIndex = 1;
        public static int userImieIndex = 2;
        public static int userNazwiskoIndex = 3;
        public static int userIdIndex = 4;

        //lista programów do wyświetlenia w appListView lub w edytorze, w zależności od warunku
        public static string getAppList = "select ap.ID_app, ap.appDisplayName, ap.appName, ap.name_app, ap.path_app, ap.appPath, ap.name_db, ap.srod_app, ap.variant from [dbo].[app_list] as ap " +
                                                    "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                    "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                    "@filter" +
                                                    "group by ap.ID_app, ap.appName, ap.name_app, ap.path_app, ap.appPath, ap.appDisplayName, ap.name_db, ap.srod_app, ap.variant";

        public static string appFilter_DisplayNameNotNull = "where ap.appDisplayName is not null ";

        public static int getAppList_appDisplayNameIndex = 1;
        public static int getAppList_appIdIndex = 0;

 

        //role aplikacji i ich opisy i ich położenie w kwerendzie, zastosowanie w zależności od filtra
        public static string getRolaList = "select ra.ID_rola, ra.name_rola, ra.descr_rola, al.ID_app, al.appDisplayName from [dbo].[rola_app]  as ra " +
                                                "inner join app_list as al on ra.ID_app=al.ID_app " +
                                                "@filter";

        public static string getRolaList_rolaFilter_AppId = " where al.ID_app = ";

        public static int getRolaList_rolaNameIndex = 1;           //położenie name_rola w kwerendzie
        public static int getRolaList_rolaDescrIndex = 2;       //położenie opisu roli aplikacji w kwerendzie
        public static int getRolaList_rolaIdIndex = 0;          //położenie Id roli aplikacji w kwerendzie
        public static int getRolaList_rolaAppIdIndex = 3;       //położenie ID aplikacji
        public static int getRolaList_rolaAppNameIndex = 4;     //położenie nazwy aplikacji


        //moduły aplikacji do których role mają uprawnienia i poziom tych uprawnień (zapis/odczyt)
        public static string getRolaModules = "select ID_mod, Grant_app from [dbo].[rola_upr] " +
                                                "where ID_rola=";

        public static int getRolaModules_moduleId = 0;
        public static int getRolaModules_GrantApp = 1;


        //moduły aplikacji i ich opisy i ich położenie w kwerendzie
        public static string getAppModuleList = "select ID_mod, ID_app, name_mod from mod_app";

        public static int getAppModuleList_moduleIdIndex = 0;
        public static int getAppModuleList_moduleAppIdIndex = 1;
        public static int getAppModuleList_moduleNameIndex = 2;


        public static string updateAppModule = "update mod_app set name_mod = @newModName where ID_mod = @IDmod";



        //
        // uprawnienia użytkowników domenowych i sql są rozdzielone, ale przy obecnej strukturze bazy desktopu
        //jest to bez sensu, bo mają oni jedno ID a uprawnienia w desktopie przydzielane są po ID
        //

        //lista aplikacji do których dany użytkownik ma uprawnienia
        public static string getUserApps = "select ap.ID_app from [dbo].[app_list] as ap " + 
                                                                "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                            "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                        "where ap.appDisplayName is not null and au.Grant_app = 1 and ul.ID_user = ";

        //lista aplikacji do których dany użytkownik sql ma uprawnienia
        public static string getSqlUserApps = "select ap.appDisplayName from [dbo].[app_list] as ap " +
                                                                "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                            "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                        "where ap.appDisplayName is not null and au.Grant_app = 1 and ul.login_user = ";

       
        //rola danego użytkownika w danej aplikacji
        public static string getUserAppRola = "select ID_rola  from rola_app as ra " +
                                                        "inner join app_list as ap on ap.ID_app = ra.ID_app " +
                                                        "where ap.ID_app = @appId and " +
                                                        "ID_rola in (select ID_rola from rola_users where ID_user = @userId)";



        //
        // zapisywanie zmian do bazy
        //userId


        public static string updateUserRola = "update rola_users set ID_rola = @newRolaId where ID_rola = @oldRolaId and ID_User = @userId; ";

        public static string deleteUserApp = "update app_users set Grant_app=0 where ID_app = @appId and ID_user = @userId; ";

        public static string deleteUserAppAndRola = "delete from rola_users where ID_rola=@rolaId and ID_user=@userId; " +
                                                    "update app_users set Grant_app=0 where ID_app = @appId and ID_user = @userId; ";

        public static string insertUserApp = "update app_users set Grant_app=1 where ID_app = @appId and ID_user = @userId; ";

        public static string insertUserAppAndRola = "update app_users set Grant_app=1 where ID_app = @appId and ID_user = @userId; " +
                                                    "insert into rola_users(ID_rola, ID_user, descr) values(@rolaId, @userId, null); ";
    }
}
