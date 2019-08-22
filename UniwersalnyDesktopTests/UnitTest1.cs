using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Drawing;
using UniwersalnyDesktop;
using System.Collections.Generic;

namespace UniwersalnyDesktopTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] stringTable = { "aaa", "bbb", "cccc", "dddd" };
            List<string> stringList = new List<string>();
            foreach(string item in stringTable)
            {
                stringList.Add(item);
            }
            AdminForm adminForm = new AdminForm();
            adminForm.populateTreeview(stringList);
            adminForm.populateListview(stringList);
            adminForm.populateListbox(stringList);
            
            adminForm.ShowDialog();
        }
        [TestMethod]
        public void testForm1()
        {
            
        }
    }
}
