
using System.Xml.Linq;

namespace UniwersalnyDesktop
{
    /// <summary>
    /// przechowuje parametry aplikacji specyficzne dla profilu, np serwer
    /// </summary>
    public class AppProfileParameters
    {
        public string profileId { get; }
        public string appId { get; }
        public string serverName { get; set; }
        public string databaseName { get; set; }
        public string reportPath { get; set; }
        public string odbcDriver { get; set; }

        public AppProfileParameters(string profileId, string appId)
        {
            this.profileId = profileId;
            this.appId = appId;
        }

            public AppProfileParameters(string profileId, string appId, string parametrsAsXmlString)
        {
            this.profileId = profileId;
            this.appId = appId;
            XElement appData = XElement.Parse(parametrsAsXmlString);
            this.serverName = appData.Element("server") == null ? "" : appData.Element("server").Value;
            this.databaseName = appData.Element("database") == null ? "" : appData.Element("database").Value;
            this.odbcDriver = appData.Element("driver_odbc") == null ? "" : appData.Element("driver_odbc").Value;
            this.reportPath = appData.Element("raport") == null ? "" : appData.Element("raport").Value;
        }

        /// <summary>
        /// zwracany string zawarty jest w apostrofach
        /// </summary>
        public string getParametersAsXmlDbCompatibleString()
        {
            XElement parameters =
            new XElement("Root",
                new XElement("server", this.serverName),
                new XElement("database", this.databaseName),
                new XElement("driver_odbc", this.odbcDriver),
                new XElement("raport", this.reportPath)
                );
            return  "'" + parameters.ToString() + "'";
        }

    }
}
