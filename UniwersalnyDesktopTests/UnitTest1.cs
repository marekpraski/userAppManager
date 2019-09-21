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
            //Form1 adminForm = new Form1();
            //adminForm.populateTreeview(stringTable);
            ////adminForm.populateListview(stringList);
            ////adminForm.populateListbox(stringList);
            
            //adminForm.ShowDialog();
        }
        [TestMethod]
        public void testForm1()
        {

            string[] stringTable = { "aaa", "bbb", "cccc", "dddd" };
            List<string> stringList = new List<string>();
            foreach (string item in stringTable)
            {
                stringList.Add(item);
            }
            //Form1 fm1 = new Form1();
            //fm1.populateListview1();
            //fm1.populateListview2();


            //fm1.ShowDialog();
        }
    }
}
