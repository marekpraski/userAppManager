using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniwersalnyDesktop
{
    public class EditableDatagridControlEventArgs : EventArgs
    {

        //zrobione przez użytkownika zmiany w celkach datagridu; kluczem jest kolejny numer zmiany, wartością zmieniona celka
        public DataGridHandler dgHandler { get; set; } 

    }
}
