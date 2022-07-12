
using System;
using System.Collections.Generic;

namespace UniwersalnyDesktop
{
    //aplikacja zdefiniowana w bazie desktopu
    public class App : IProfileItem
    {
        public string displayName { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string executionPath { get; set; }
        public string defaultDatabaseName { get; set; }
        public bool runFromDesktop { get; set; }
        /// <summary>
        /// zwraca true jeżeli aplikacja spełnia warunki umożliwiające jej wyświetlenie na przycisku i uruchomienie
        /// </summary>
        public bool isValid { get => assertAppDataIsValid(); }

        public List<string> rolaIdList { get; }     //zawiera ID ról
        public List<Rola> rolaList { get; }
        public Dictionary<string, Rola> rolaDict { get; }   //kluczem jest Id roli

        public List<string> moduleIdList { get; }

        public List<AppModule> moduleList { get; }
        /// <summary>
        /// kluczem jest id profilu; zawiera parametry aplikacji specyficzne dla profilu, np. nazwę serwera
        /// </summary>
        public Dictionary<string, AppProfileParameters> appProfileParamsDict { get; }


        public App()
        {
            rolaIdList = new List<string>();
            rolaList = new List<Rola>();
            moduleIdList = new List<string>();
            moduleList = new List<AppModule>();
            rolaDict = new Dictionary<string, Rola>();
            appProfileParamsDict = new Dictionary<string, AppProfileParameters>();
        }

        #region settery
        public void addRola(Rola rola)
        {
            rolaIdList.Add(rola.id);
            rolaList.Add(rola);
            rolaDict.Add(rola.id, rola);
        }

        public void addModule(AppModule module)
        {
            moduleList.Add(module);
            moduleIdList.Add(module.id);
        } 

        public void addAppProfileParameters(AppProfileParameters appProfileParameters)
        {
            if (appProfileParamsDict.ContainsKey(appProfileParameters.profileId))
                appProfileParamsDict.Remove(appProfileParameters.profileId);

            appProfileParamsDict.Add(appProfileParameters.profileId, appProfileParameters);
        }
        #endregion

        public bool hasRola()
        {
            return (rolaIdList.Count > 0);
        }

        public bool hasModules()
        {
            return (moduleIdList.Count > 0);
        }

        private bool assertAppDataIsValid()
        {
            return !String.IsNullOrEmpty(this.displayName) && !String.IsNullOrEmpty(this.executionPath) && !String.IsNullOrEmpty(this.name);
        }

        #region gettery
        public Rola getRola(string rolaId)
        {
            Rola rola = null; ;
            if (rolaDict.ContainsKey(rolaId))
            {
                rolaDict.TryGetValue(rolaId, out rola);
            }
            return rola;
        }

        public List<string> getModuleNameList()
        {
            List<string> moduleNames = new List<string>();
            if (hasModules())
            {
                foreach (AppModule module in moduleList)
                {
                    moduleNames.Add(module.name);
                }
            }
            return moduleNames;
        } 

        public string getServerName(string idProfile)
        {
            return this.appProfileParamsDict[idProfile].serverName;
        }

        public string getDatabaseName(string idProfile)
        {
            return String.IsNullOrEmpty(this.appProfileParamsDict[idProfile].databaseName) ? this.defaultDatabaseName : this.appProfileParamsDict[idProfile].databaseName;
        }
        public string getReportPath(string idProfile)
        {
            return this.appProfileParamsDict[idProfile].reportPath;
        }
        public string getAppProfileSpecificSettingsAsXml(string idProfile)
        {
            return appProfileParamsDict[idProfile].getParametersAsXmlDbCompatibleString();
        }
        #endregion

    }
}
