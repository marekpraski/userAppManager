
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
        public string serwer { get => geteSerwerName(); }

        public string configXlm { get; set; }
        public byte[] logoImageAsBytes { get; set; }
        public Dictionary<string, IProfileItem> applications { get; } = new Dictionary<string, IProfileItem>();    //kluczem jest id aplikacji
        /// <summary>
        /// yylko te aplikacje, które spełniają kryteria ważności, m.in mają nazwę i ścieżkę wywołania
        /// </summary>
        public Dictionary<string, IProfileItem> validApplications { get => getValidApplications(); }    //kluczem jest id aplikacji

        public Dictionary<string, IProfileItem> users { get; } = new Dictionary<string, IProfileItem>();  //kluczem jest id użytkownika

        #region konstruktory
        public DesktopProfile()
        {
        }

        public DesktopProfile(string profileId, string profileName)
        {
            this.name = profileName;
            this.id = profileId;
        }
        #endregion

        #region dodawanie elementów do profilu
        public void addAppToProfile(App aplikacja)
        {
            if (!applications.ContainsKey(aplikacja.id))
                applications.Add(aplikacja.id, aplikacja);
        }
        public void addUserToProfile(DesktopUser user)
        {
            if (!users.ContainsKey(user.id))
                users.Add(user.id, user);
        }
        #endregion

        #region usuwanie elementów z profilu
        /// <summary>
        /// w parametrze id aplikacji
        /// </summary>
        internal void removeAppFromProfile(string idApp)
        {
            if (applications.ContainsKey(idApp))
                applications.Remove(idApp);
        }

        public void removeUserFromProfile(string userId)
        {
            if (users.ContainsKey(userId))
                users.Remove(userId);
        } 
        #endregion


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

        private string geteSerwerName()
        {
            return "";
        }

    }
}
