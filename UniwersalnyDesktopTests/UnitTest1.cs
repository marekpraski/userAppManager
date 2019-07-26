using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Drawing;
using UniwersalnyDesktop;

namespace UniwersalnyDesktopTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DesktopForm desktop = new DesktopForm("marek", "root");
            desktop.ShowDialog();
        }
    }
}
