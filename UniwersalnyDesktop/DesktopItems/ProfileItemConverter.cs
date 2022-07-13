
using System.Collections.Generic;

namespace UniwersalnyDesktop
{
    /// <summary>
    /// przerabia słowniki <string, IProfileItem> na <string, App> lub <string, DesktopUser>; 
    /// efekt zaszłości; najpierw miałem tylko klasy App i DesktopUser, następnie dodałem interfejs IProfileItem, 
    /// żeby użyć jednego okna dodawania obu tych obiektów do profilu; ten konwerter jest po to, żeby nie trzeba było przerabiać tych typów obiektów w całej aplikacji
    /// </summary>
    public class ProfileItemConverter
    {
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
        internal Dictionary<string, App> convertToAppDictionary(Dictionary<string, IProfileItem> appDictionary)
        {
            Dictionary<string, App> apps = new Dictionary<string, App>();
            foreach (string id in appDictionary.Keys)
            {
                apps.Add(id, appDictionary[id] as App);
            }
            return apps;
        }

        internal Dictionary<string, DesktopUser> convertToDesktopUserDictionary(Dictionary<string, IProfileItem> userDict)
        {
            Dictionary<string, DesktopUser> users = new Dictionary<string, DesktopUser>();
            foreach (string id in userDict.Keys)
            {
                users.Add(id, userDict[id] as DesktopUser);
            }
            return users;
        }
    }
}
