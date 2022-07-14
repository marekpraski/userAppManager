using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    class FileManipulator
    {        

        public void runProgram(string program, string arguments)
        {
            try
            {                
                System.Diagnostics.Process.Start(program, arguments); 
            }
            catch (System.ComponentModel.Win32Exception exc)
            {
                MyMessageBox.display(exc.Message + "\r\n" + program);
            }
            catch (DirectoryNotFoundException exc)
            {
                MyMessageBox.display(exc.Message, MessageBoxType.Error);                
            }
            catch(FileNotFoundException exc)
            {
                MyMessageBox.display(exc.Message, MessageBoxType.Error);
            }
        }     
    }
}
