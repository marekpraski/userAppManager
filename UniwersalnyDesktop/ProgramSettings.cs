
namespace UniwersalnyDesktop
{
    class ProgramSettings
    {       

        //imię i nazwisko oznaczające administratora, login może być dowolny; po wykryciu tego imienia i nazwiska przy logowaniu desktop otwiera AdminForm
        public static string administratorName = "Desktop Administrator";

        //
        // DBEditorMainForm
        //

        public static int numberOfRowsToLoad = 50;     //ile wierszy ładuje się do datagrida w jednej operacji
    }
}
