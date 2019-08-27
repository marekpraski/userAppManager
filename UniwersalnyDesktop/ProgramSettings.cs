using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public class ProgramSettings
    {
        public enum UserType { Administrator, RegularUser}

        //imię i nazwisko oznaczające administratora, login może być dowolny; po wykryciu tego imienia i nazwiska przy logowaniu desktop otwiera AdminForm
        public static string administratorName = "Desktop Administrator";

        //przykładowa linia connection string @"Data Source=laptop08\sqlexpress;Initial Catalog=dbrezerwer_test;User ID=marek;Password=root";
        //ustawienia pliku konfiguracyjnego: nazwa pliku i slowa kluczowe po których wykrywany jest serwer i baza danych
        public static string configFileName = "desktopConf.xml";
        public static string connectionStringDelimiter = "server";
        public static string databaseNameDelimiter = "db_name";

        //ścieżka względna z której znajdoany jest plik konfiguracyjny, np @"/../Conf/". Gdy jest "" plik konfiguracyjny musi być w katalogu z którego uruchomiony jest program
        public static string configFilePath = "";

        public static string desktopAppDataQueryTemplate = "select ap.ID_app, ap.appName, ap.appPath, ap.appDisplayName, au.Grant_app, ap.name_db from [dbo].[app_list] as ap " +
                                                    "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                    "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                    "where ap.name_db is not null and ul.login_user = ";

        public static string desktopUserDataQueryTemplate = "select  login_user, windows_user, imie_user, nazwisko_user from users_list " +
                                                            "where login_user = ";

        //wszyscy użytkownicy za wyjątkiem Administratora
        public static string userQueryTemplate = "select  login_user, windows_user, imie_user, nazwisko_user,ID_user from users_list where login_user is not null and login_user <> ";
        public static int userSqlLoginIndex = 0;
        public static int userWindowsLoginIndex = 1;
        public static int userImieIndex = 2;
        public static int userNazwiskoIndex = 3;
        public static int userIdIndex = 4;


        public static string appListQueryTemplate = "select ap.appDisplayName from [dbo].[app_list] as ap " +
                                                    "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                    "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                    "where ap.appName is not null " +
													"group by ap.ID_app, ap.appName, ap.appPath, ap.appDisplayName";

        //role aplikacji i ich opisy i ich położenie w kwerendzie
        public static string appRolesQueryTemplate = "select name_rola, descr_rola from [dbo].[rola_app]  as ra " +
                                                      "inner join app_list as al on ra.ID_app=al.ID_app " +
                                                      "where al.appDisplayName = ";
        public static int roleIndex = 0;           //położenie name_rola w kwerendzie
        public static int roleDescrIndex = 1;       //położenie opisu roli aplikacji w kwerendzie



        //lista aplikacji do których dany użytkownik domenowy ma uprawnienia
        public static string windowsUserAppsQueryTemplate = "select ap.appDisplayName from [dbo].[app_list] as ap " + 
                                                                "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                            "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                        "where ap.appDisplayName is not null and au.Grant_app = 1 and ul.windows_user = ";

        //lista aplikacji do których dany użytkownik sql ma uprawnienia
        public static string sqlUserAppsQueryTemplate = "select ap.appDisplayName from [dbo].[app_list] as ap " +
                                                                "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                            "inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                        "where ap.appDisplayName is not null and au.Grant_app = 1 and ul.login_user = ";

        //lista aplikacji i ról w nich danego użytkownika domenowego
        public static string userAppRolaQueryTemplate = "select name_rola  from rola_app as ra " +
                                                        "inner join[app_list] as ap on ap.ID_app = ra.ID_app " +
                                                        "where ap.appDisplayName = '@appDisplayName' and " +
                                                        "ID_rola = (select ID_rola from rola_users where ID_user = (select ID_user from users_list where @loginType = '@user'))";

    }
}
