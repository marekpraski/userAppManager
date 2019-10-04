using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniwersalnyDesktop
{
    public class Rola
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string appId { get; set; }
        public string appName { get; set; }

        public List<string> moduleIdList { get; }

        public List<AppModule> moduleList { get; }
        public Dictionary<string, AppModule> moduleDict { get; }        //kluczem jest id modułu

        public Dictionary<string, string> modulePrivilageDict { get; }       // kluczem jest id modułu, wartością jest Grant_app

        public Rola()
        {
            moduleIdList = new List<string>();
            moduleList = new List<AppModule>();
            modulePrivilageDict = new Dictionary<string, string>();
            moduleDict = new Dictionary<string, AppModule>();
        }

        public bool hasModules()
        {
            return (moduleIdList.Count > 0);
        }

        public void addModule(AppModule module, string grantApp)
        {
            moduleList.Add(module);
            moduleIdList.Add(module.id);
            modulePrivilageDict.Add(module.id, grantApp);
            moduleDict.Add(module.id, module);
        }

        public string getModulePrivilages(AppModule module)
        {
            string grantApp = "";
            if (modulePrivilageDict.ContainsKey(module.id))
            {
                modulePrivilageDict.TryGetValue(module.id, out grantApp);
            }
            return grantApp;
        }
    }
}
