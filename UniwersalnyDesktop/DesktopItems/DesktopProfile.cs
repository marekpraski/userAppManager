
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
        public Dictionary<string, App> applications { get; } = new Dictionary<string, App>();    //kluczem jest id aplikacji
        public Dictionary<string, DesktopUser> profileUsers { get; } = new Dictionary<string, DesktopUser>();  //kluczem jest id użytkownika

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
            if (!profileUsers.ContainsKey(user.id))
                profileUsers.Add(user.id, user);
        }

        public void removeUserFromProfile(string userId)
        {
            if (profileUsers.ContainsKey(userId))
                profileUsers.Remove(userId);
        }
    }
}
