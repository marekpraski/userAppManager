
using System;
using System.Collections.Generic;

namespace UniwersalnyDesktop
{
    public class DesktopProfile
    {
        public string name { get; set; }
        public string id { get; set; }
        public string domena { get; set; }
        public string ldap { get; set; }
        public Dictionary<string, IProfileItem> applications { get; } = new Dictionary<string, IProfileItem>();    //kluczem jest id aplikacji
        /// <summary>
        /// yylko te aplikacje, które spełniają kryteria ważności, m.in mają nazwę i ścieżkę wywołania
        /// </summary>
        public Dictionary<string, IProfileItem> validApplications { get => getValidApplications(); }    //kluczem jest id aplikacji

        public Dictionary<string, IProfileItem> users { get; } = new Dictionary<string, IProfileItem>();  //kluczem jest id użytkownika

        public DesktopProfile()
        {
        }

        public DesktopProfile(string profileId, string profileName )
        {
            this.name = profileName;
            this.id = profileId;
        }
        public void addAppToProfile(App aplikacja)
        {
            if (!applications.ContainsKey(aplikacja.id))
                applications.Add(aplikacja.id, aplikacja);
        }
        public void removeAppFromProfile(App aplikacja)
        {
            if (applications.ContainsKey(aplikacja.id))
                applications.Remove(aplikacja.id);
        }

        /// <summary>
        /// w parametrze id aplikacji
        /// </summary>
        internal void removeAppFromProfile(string idApp)
        {
            if (applications.ContainsKey(idApp))
                applications.Remove(idApp);
        }

        public void addUserToProfile(DesktopUser user)
        {
            if (!users.ContainsKey(user.id))
                users.Add(user.id, user);
        }

        public void removeUserFromProfile(string userId)
        {
            if (users.ContainsKey(userId))
                users.Remove(userId);
        }
        public Dictionary<string, App> getAppDictionary()
        {
            Dictionary<string, App> apps = new Dictionary<string, App>();
            foreach (string id in this.applications.Keys)
            {
                apps.Add(id, this.applications[id] as App);
            }
            return apps;
        }

        public Dictionary<string, DesktopUser> getUserDictionary()
        {
            Dictionary<string, DesktopUser> apps = new Dictionary<string, DesktopUser>();
            foreach (string id in this.users.Keys)
            {
                apps.Add(id, this.users[id] as DesktopUser);
            }
            return apps;
        }

        private Dictionary<string, IProfileItem> getValidApplications()
        {
            if (this.applications == null)
                return null;
            Dictionary<string, IProfileItem> items = new Dictionary<string, IProfileItem>();
            foreach (string id in this.applications.Keys)
            {
                if((applications[id] as App).isValid)
                    items.Add(id, applications[id]);
            }
            return items;
        }
        internal Dictionary<string, IProfileItem> convertToIProfileItemDictionary(Dictionary<string, App> appDictionary)
        {
            Dictionary<string, IProfileItem> items = new Dictionary<string, IProfileItem>();
            foreach (string id in appDictionary.Keys)
            {
                items.Add(id, appDictionary[id]);
            }
            return items;
        }

        internal Dictionary<string, IProfileItem> convertToIProfileItemDictionary(Dictionary<string, DesktopUser> userDict)
        {
            Dictionary<string, IProfileItem> items = new Dictionary<string, IProfileItem>();
            foreach (string id in userDict.Keys)
            {
                items.Add(id, userDict[id]);
            }
            return items;
        }

    }
}
