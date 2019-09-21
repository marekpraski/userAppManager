using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniwersalnyDesktop
{
    public class ChangedDataBundle
    {

        private Dictionary<DesktopUser, Dictionary<App, AppDataItem>> changesDict;

        private Dictionary<string, DesktopUser> userBackupDict;         //klucz: id użytkownika; wartość użytkownik


        public ChangedDataBundle(Dictionary<DesktopUser, Dictionary<App, AppDataItem>> changesDict, Dictionary<string, DesktopUser> userBackupDict)
        {
            this.changesDict = changesDict;
            this.userBackupDict = userBackupDict;
        }


        public string getAppDataStatus(DesktopUser user, App app)
        {
            DesktopUser backupUser;
            userBackupDict.TryGetValue(user.id, out backupUser);
            Dictionary<App, AppDataItem> backupUserAppDict = backupUser.userAppDict;

            AppDataItem newAppData = getNewAppData(user, app);

            if (!backupUserAppDict.ContainsKey(app))
            {
                return "insert";
            }
            else if (!newAppData.isEnabled)
            {
                return "delete";
            }
            return "update";
        }


        public List<App> getChangedUserApps(DesktopUser user)
        {
            Dictionary<App, AppDataItem> changedAppDataDict;
            if (changesDict.ContainsKey(user))
            {
                changesDict.TryGetValue(user, out changedAppDataDict);
                return changedAppDataDict.Keys.ToList();
            }
            return null;
        }


        public AppDataItem getNewAppData(DesktopUser user, App app)
        {
            AppDataItem newAppData;
            Dictionary<App, AppDataItem> changedAppDataDict;
            if (changesDict.ContainsKey(user))
            {
                changesDict.TryGetValue(user, out changedAppDataDict);
                if (changedAppDataDict.ContainsKey(app))
                {
                    changedAppDataDict.TryGetValue(app, out newAppData);
                    return newAppData;
                }
            }
            return null;
        }

        public AppDataItem getOldAppData(DesktopUser user, App app)
        {
            DesktopUser backupedUser;
            if (userBackupDict.ContainsKey(user.id))
            {
                userBackupDict.TryGetValue(user.id, out backupedUser);
                return backupedUser.getAppData(app);
            }
            return null;
        }

        public List<DesktopUser> getUsers()
        {
            return changesDict.Keys.ToList();
        }
    }
}
