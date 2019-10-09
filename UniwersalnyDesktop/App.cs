using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniwersalnyDesktop
{
    //aplikacja zdefiniowana w bazie desktopu
    public class App
    {
        public string appDisplayName { get; set; }
        public string Id { get; set; }
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
