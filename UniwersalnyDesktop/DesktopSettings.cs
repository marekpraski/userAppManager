
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using UtilityTools;

namespace UniwersalnyDesktop
{
    
    public class DesktopSettings
    {
        #region statyczne właściwości publiczne związane z ustawieniami formatki
        public enum ButtonType { square, rectangular }
        public enum GroupboxType { squareButtons, rectangularButtons }

        public static int numberOfSquareButtonsInOneBlock = 4;

        //rozmiary buttonów
        public static int squareButtonWidth = 110;
        public static int squareButtonHeigth = 90;
        public static int rectangularButtonWidth = 350;
        public static int rectangularButtonHeigth = 40;

        //rozlokowanie buttonów w groupboxie
        public static int horizontalButtonPadding = 10;
        public static int verticalButtonPadding = 10;

        //rozlokowanie groupboxów
        public static int firstGroupboxVerticalLocation = 0;
        public static int horizontalGroupboxPadding = 0;
        public static int verticalGroupboxPadding = 0;

        //korekta do wymiarów desktopu, żeby uwzględnić grubość obrzeży
        public static int tabCtrlHorizontalPadding = 10;
        public static int tabCtrlVerticalPadding = 30;

        //max wysokość desktopu
        public static int maxTabCtrlHeigth = 600;
        #endregion

        private string configFilePath = LoginForm.mainPath + "desktopConfig.xml";
        private string logoFileName = LoginForm.mainPath + "logo.png";
        public void saveCurrentSettings(DesktopProfile profile)
        {
            saveXmlSettings(profile);
            saveLogoImageToDisk(profile);
        }

        private void saveXmlSettings(DesktopProfile profile)
        {
            XElement settings = new XElement("Ustawienia",
                new XElement("idProfilu", profile.id));
            new XmlWriter().saveAsXmlFile(settings, configFilePath);
        }

        private void saveLogoImageToDisk(DesktopProfile profile)
        {
            using (MemoryStream ms = new MemoryStream(profile.logoImageAsBytes))
            {
                Image logo = Image.FromStream(ms);
                logo.Save(logoFileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        internal string readUserSettings()
        {
            return new XmlReader(configFilePath).getNodeValue("idProfilu");
        }
    }
}
