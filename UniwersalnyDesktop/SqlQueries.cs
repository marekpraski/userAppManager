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

        public static string desktopAppDataQueryTemplate = "select ap.ID_app, ap.appName, ap.appPath, ap.appDisplayName, au.Grant_app, ap.name_db from [dbo].[app_list] as ap " +
                                                    "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                    "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                    "where ap.name_db is not null and ul.login_user = ";

        public static string desktopUserDataQueryTemplate = "select  login_user, windows_user, imie_user, nazwisko_user from users_list " +
                                                            "where login_user = ";
        //
        // wypełnianie okna AdminForm
        //

        //wszyscy użytkownicy za wyjątkiem Administratora
        public static string userQueryTemplate = "select  login_user, windows_user, imie_user, nazwisko_user,ID_user from users_list where login_user is not null and login_user <> ";
        public static int userSqlLoginIndex = 0;
        public static int userWindowsLoginIndex = 1;
        public static int userImieIndex = 2;
        public static int userNazwiskoIndex = 3;
        public static int userIdIndex = 4;

        //lista programów do wyświetlenia w appListView i ich położenie w kwerendzie
        public static string appListQueryTemplate = "select ap.appDisplayName, ap.ID_app from [dbo].[app_list] as ap " +
                                                    "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                    "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                    "where ap.appName is not null " +
													"group by ap.ID_app, ap.appName, ap.appPath, ap.appDisplayName";
        public static int appDisplayNameIndex = 0;
        public static int appIdIndex = 1;


        //role aplikacji i ich opisy i ich położenie w kwerendzie
        public static string rolaQueryTemplate = "select ra.ID_rola, ra.name_rola, ra.descr_rola, al.ID_app, al.appDisplayName from [dbo].[rola_app]  as ra " +
                                                      "inner join app_list as al on ra.ID_app=al.ID_app";

        public static int rolaNameIndex = 1;           //położenie name_rola w kwerendzie
        public static int rolaDescrIndex = 2;       //położenie opisu roli aplikacji w kwerendzie
        public static int rolaIdIndex = 0;          //położenie Id roli aplikacji w kwerendzie
        public static int rolaAppIdIndex = 3;       //położenie ID aplikacji
        public static int rolaAppNameIndex = 4;     //położenie nazwy aplikacji


        public static string rolesForEachAppQueryTemplate = "select ra.ID_rola from[dbo].[rola_app]  as ra " +
                                                            "inner join app_list as al on ra.ID_app= al.ID_app " +
                                                            " where al.appDisplayName = ";

        //
        // uprawnienia użytkowników domenowych i sql są rozdzielone, ale przy obecnej strukturze bazy desktopu
        //jest to bez sensu, bo mają oni jedno ID a uprawnienia w desktopie przydzielane są po ID
        //

        //lista aplikacji do których dany użytkownik domenowy ma uprawnienia
        public static string userAppsQueryTemplate = "select ap.ID_app from [dbo].[app_list] as ap " + 
                                                                "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                            "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                        "where ap.appDisplayName is not null and au.Grant_app = 1 and ul.ID_user = ";

        //lista aplikacji do których dany użytkownik sql ma uprawnienia
        public static string sqlUserAppsQueryTemplate = "select ap.appDisplayName from [dbo].[app_list] as ap " +
                                                                "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                            "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                        "where ap.appDisplayName is not null and au.Grant_app = 1 and ul.login_user = ";

        //lista aplikacji i ról w nich danego użytkownika domenowego / sql
        public static string userAppRolaQueryTemplate = "select ID_rola  from rola_app as ra " +
                                                        "inner join app_list as ap on ap.ID_app = ra.ID_app " +
                                                        "where ap.ID_app = @appId and " +
                                                        "ID_rola in (select ID_rola from rola_users where ID_user = @userId)";


        public static string rolaUserUpdateQuery = "update rola_users set ID_rola = @NewIdRola where ID_rola = @OldIdRola and ID_User = @IdUser";
    }
}
