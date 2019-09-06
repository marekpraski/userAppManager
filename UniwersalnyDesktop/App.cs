using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniwersalnyDesktop
{
    //aplikacja zdefiniowana w bazie desktopu
    class App
    {
        public string appDisplayName { get; set; }
        public string Id { get; set; }
        public List<string> rolaIdList { get; }     //zawiera ID ról

        public App()
        {
            rolaIdList = new List<string>();
        }

        public void addRola(string rolaId)
        {
            rolaIdList.Add(rolaId);
        }
    }
}
