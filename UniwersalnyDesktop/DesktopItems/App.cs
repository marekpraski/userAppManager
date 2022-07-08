
using System.Collections.Generic;

namespace UniwersalnyDesktop
{
    //aplikacja zdefiniowana w bazie desktopu
    public class App : IProfileItem
    {
        public string displayName { get; set; }
        public string id { get; set; }
        public string serverName { get; set; }
        public string databaseName { get; set; }
        public string reportSerwerLink { get; set; }
        public string executionPath { get; set; }
        public List<string> rolaIdList { get; }     //zawiera ID ról
        public List<Rola> rolaList { get; }
        public Dictionary<string, Rola> rolaDict { get; }   //kluczem jest Id roli

        public List<string> moduleIdList { get; }

        public List<AppModule> moduleList { get; }


        public App()
        {
            rolaIdList = new List<string>();
            rolaList = new List<Rola>();
            moduleIdList = new List<string>();
            moduleList = new List<AppModule>();
            rolaDict = new Dictionary<string, Rola>();
        }

        public void addRola(Rola rola)
        {
            rolaIdList.Add(rola.id);
            rolaList.Add(rola);
            rolaDict.Add(rola.id, rola);
        }

        public bool hasRola()
        {
            return (rolaIdList.Count > 0);
        }

        public bool hasModules()
        {
            return (moduleIdList.Count > 0);
        }

        public void addModule(AppModule module)
        {
            moduleList.Add(module);
            moduleIdList.Add(module.id);
        }

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
            if(hasModules())
            {
                foreach(AppModule module in moduleList)
                {
                    moduleNames.Add(module.name);
                }
            }
            return moduleNames;
        }
    }
}
