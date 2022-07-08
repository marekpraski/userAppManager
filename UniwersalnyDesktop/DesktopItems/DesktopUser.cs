using System;
using System.Collections.Generic;

namespace UniwersalnyDesktop
{
    public class DesktopUser : ICloneable, IProfileItem
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string id { get; set; }
        public string windowsLogin { get; set; }
        public string sqlLogin { get; set; }
        public Dictionary<App, AppDataItem> userAppDict { get; }

        public string displayName => getDisplayName();

        private string getDisplayName()
        {
            return name + " " + surname;
        }

        public DesktopUser()
        {
            userAppDict = new Dictionary<App, AppDataItem>();
        }

        /*
         * dodaje jeżeli nie ma, aktualizuje jeżeli jest
         */
        public void addUpdateApp(App app, Rola rola = null)
        {
            if (!userAppDict.ContainsKey(app))
            {
                addApp(app, rola);
            }
            else
            {
                updateApp(app, rola);
            }
        }


        private void addApp(App app, Rola rola = null)
        {
            AppDataItem appData = new AppDataItem(app.id);

            if (rola != null)
            {
                appData.rolaId = rola.id;
                appData.rolaName = rola.name;
                appData.rolaDesc = rola.description;
            }
            else
            {
                appData.rolaId = "";
            }

            appData.isEnabled = true;
            appData.appName = app.displayName;
            userAppDict.Add(app, appData);
        }


        private void updateApp(App app, Rola rola)
        {
            AppDataItem appData;
            userAppDict.TryGetValue(app, out appData);
            appData.appName = app.displayName;

            if (rola != null)
            {
                appData.rolaId = rola.id;
                appData.rolaName = rola.name;
                appData.rolaDesc = rola.description;
                appData.isEnabled = true;
            }
        }

        //też element aktualizacji, gdy aplikacja ma być usunięta
        public bool markAppDisabled(App app)
        {
            if (userAppDict.ContainsKey(app))
            {
                AppDataItem appData = null;
                userAppDict.TryGetValue(app, out appData);
                appData.isEnabled = false;
                return true;
            }
            return false;
        }

        public string getRolaId(App app)
        {
            if (userAppDict.ContainsKey(app))
            {
                AppDataItem appData = null;
                userAppDict.TryGetValue(app, out appData);
                return appData.rolaId;
            }
            else
                return "";
        }


        public AppDataItem getAppData(App app)
        {
            if (userAppDict.ContainsKey(app))
            {
                AppDataItem appData = null;
                userAppDict.TryGetValue(app, out appData);
                return appData;
            }
            else
            {
                return null;
            }
        }

        public List<App> getApps()
        {
            if (userAppDict.Count > 0)
            {
                List<App> apps = new List<App>();
                foreach (App app in userAppDict.Keys)
                {
                    apps.Add(app);
                }
                return apps;
            }
            else
                return null;
        }


        public bool hasApp(App app)
        {
            return userAppDict.ContainsKey(app);
        }

        public void deleteApp(App app)
        {
            if (userAppDict.ContainsKey(app))
            {
                userAppDict.Remove(app);
            }
        }


        public object Clone()
        {
            DesktopUser other = new DesktopUser();

            other.name = String.Copy(name);
            other.surname = String.Copy(surname);
            other.id = String.Copy(id);
            other.windowsLogin = String.Copy(windowsLogin);
            other.sqlLogin = String.Copy(sqlLogin);

            foreach (App app in this.userAppDict.Keys)
            {
                AppDataItem appData = (AppDataItem) this.getAppData(app).Clone();

                other.userAppDict.Add(app, appData);
            }

            return other;
        }
    }
}
