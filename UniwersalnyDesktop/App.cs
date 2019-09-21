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

        public App()
        {
            rolaIdList = new List<string>();
            rolaList = new List<Rola>();
        }

        public void addRola(Rola rola)
        {
            rolaIdList.Add(rola.idRola);
            rolaList.Add(rola);
        }

        public bool hasRola()
        {
            if (rolaIdList.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
