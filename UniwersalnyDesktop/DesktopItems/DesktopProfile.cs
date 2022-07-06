
using System;
using System.Collections.Generic;

namespace UniwersalnyDesktop
{
    public class DesktopProfile
    {
        public string name { get; }
        public string id { get; }
        public Dictionary<string, App> applications { get; } = new Dictionary<string, App>();    //kluczem jest id aplikacji

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
    }
}
