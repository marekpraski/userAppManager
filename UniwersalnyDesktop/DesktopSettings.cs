
using System;
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

        private string desktopSettingsFilePath = LoginForm.mainPath + "desktopSettings.xml";    //plik w którym zapisany jest ostatnio używany profil desktopu
        private string logoFileName = LoginForm.mainPath + "logo.png";
        public static string configXmlFilePath = @"..\conf\desktopConfig.xml";      //główny plik konfiguracyjny dla wszystkich uruchamianych programów

        //imię i nazwisko oznaczające administratora, login może być dowolny; po wykryciu tego imienia i nazwiska przy logowaniu desktop otwiera AdminForm
        public static string administratorName = "Desktop Administrator";

        //
        // DBEditorMainForm
        //
        public static int numberOfRowsToLoad = 50;     //ile wierszy ładuje się do datagrida w jednej operacji

        public void saveCurrentSettings(DesktopProfile profile)
        {
            saveDesktopSettings(profile);
            saveLogoImageToDisk(profile);
            saveConfigXmlToDisk(profile);
        }

        private void saveDesktopSettings(DesktopProfile profile)
        {
            XElement settings = new XElement("Ustawienia",
                new XElement("idProfilu", profile.id));
            new XmlWriter().saveAsXmlFile(settings, desktopSettingsFilePath);
        }

        private void saveLogoImageToDisk(DesktopProfile profile)
        {
            using (MemoryStream ms = new MemoryStream(profile.logoImageAsBytes))
            {
                Image logo = Image.FromStream(ms);
                logo.Save(logoFileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        private void saveConfigXmlToDisk(DesktopProfile profile)
        {
            new TextFileTools().saveTextToFile(configXmlFilePath, profile.configXlm);
        }

        internal string readUserSettings()
        {
            return new XmlReader(desktopSettingsFilePath).getNodeValue("idProfilu");
        }
    }
}
