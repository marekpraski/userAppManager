
using System;
using System.Windows.Forms;
using UtilityTools;

namespace UniwersalnyDesktop
{
    public partial class ConnectionConfigurator : Form
    {
        /// <summary>
        /// ConnectionConfiguration
        /// initalize components
        /// </summary>
        public ConnectionConfigurator()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ConnectionConfiguration_Load
        /// loadConnectionDataFromFile function
        /// </summary>
        private void ConnectionConfiguration_Load(object sender, EventArgs e)
        {
            loadConnectionDataFromFile();
        }

        /// <summary>
        /// loadConnectionDataFromFile function
        /// load connection from file to be able to get/put data to database
        /// </summary>
        private void loadConnectionDataFromFile()
        {
            XmlReader confReader = new XmlReader(LoginForm.mainPath + @"..\conf\config.xml");

            serwerTB.Text = confReader.getNodeValue("server");
            bazaDesktopTB.Text = confReader.getNodeValue("db_desktop");
            sterownikTB.Text = confReader.getNodeValue("driver");
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            XmlWriter xmlWriter = new XmlWriter();
            string xmlMainNodeName = "config";
            string[] nodeNames = new string[] { "server", "db_desktop", "driver" };
            string[] nodeValues = new string[] { serwerTB.Text, bazaDesktopTB.Text, sterownikTB.Text };
            xmlWriter.saveAsXmlFile(xmlMainNodeName, nodeNames, nodeValues, LoginForm.mainPath + @"..\conf\config.xml");
            this.Close();
        }
    }
}
