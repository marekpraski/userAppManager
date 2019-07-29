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
        //przykładowa linia connection string @"Data Source=laptop08\sqlexpress;Initial Catalog=dbrezerwer_test;User ID=marek;Password=root";
        //ustawienia pliku konfiguracyjnego
        public static string configFileName = "desktopConf.xml";
        public static string connectionStringDelimiter = "server";
        public static string databaseNameDelimiter = "db_name";

        //ścieżka względna z której znajdoany jest plik konfiguracyjny, np @"/../Conf/". Gdy jest "" plik konfiguracyjny musi być w katalogu z ktorego uruchomiony jest program
        public static string configFilePath = "";

        public static string desktopAppDataQueryTemplate = "select ap.ID_app, ap.appName, ap.appPath, ap.appDisplayName, au.Grant_app, ap.name_db from [dbo].[app_list] as ap " +
                                                    "inner join app_users as au on ap.ID_app = au.ID_app " +
                                                    "inner join users_list as ul on ul.ID_user = au.ID_user " + 
            "where ap.name_db is not null and ul.login_user = ";

        public static string desktopUserDataQueryTemplate = "select top 1 ul.login_user, ul.windows_user, ul.imie_user, ul.nazwisko_user from [dbo].[app_list] as ap " +
                                                    "inner join app_users as au on ap.ID_app = au.ID_app inner join users_list as ul on ul.ID_user = au.ID_user " +
                                                    "where ap.name_db is not null and ul.login_user = ";
    }
}
