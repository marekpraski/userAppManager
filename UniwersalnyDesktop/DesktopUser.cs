using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniwersalnyDesktop
{
    class DesktopUser
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string id { get; set; }
        public string windowsLogin { get; set; }
        public string sqlLogin { get; set; }
        public Dictionary<string, string> appRolaDict { get; }      //klucz - id aplikacji, wartość - id roli

        public DesktopUser()
        {
            appRolaDict = new Dictionary<string, string>();
        }

        /*
         * dodaje jeżeli nie ma, aktualizuje jeżeli jest
         */
        public void addUpdateAppRola(string appId, string rolaId = "")
        {
            if (!appRolaDict.ContainsKey(appId))
            {

                appRolaDict.Add(appId, rolaId);
            }
            else
            {
                appRolaDict[appId] = rolaId;
            }
        }

        public string getRola(string appId)
        {
            string rola = "";
            if (appRolaDict.ContainsKey(appId))
            {
                appRolaDict.TryGetValue(appId, out rola);
                return rola;
            }
            else
                return "";
        }

        public List<string> getApps()
        {
            if (appRolaDict.Count > 0)
            {
                List<string> apps = new List<string>();
                foreach (string app in appRolaDict.Keys)
                {
                    apps.Add(app);
                }
                return apps;
            }
            else
                return null;
        }

       
        public void deleteApp(string appId)
        {
            if (appRolaDict.ContainsKey(appId))
            {
                appRolaDict.Remove(appId);
            }
        }
    }
}
