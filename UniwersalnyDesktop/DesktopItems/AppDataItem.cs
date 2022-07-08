
namespace UniwersalnyDesktop
{

    //do przechowywania uprawnień użytkownika do aplikacji i ról użytkownika
    public class AppDataItem
    {
        public string appId { get; set; }
        public string appName { get; set; }

        public bool isEnabled { get; set; }     //true oznacza że użytkownik ma uprawnienia do aplikacji; generalnie zawsze będzie true za wyjątkiem 
                                                //gdy po edycji użytkownikowi zostaną uprawnienia zabrane
                                                //potrzebne mi jest podczas zapisywania, żeby nie porównywać zawartości dwóch słowników


        public string rolaId { get; set; }
        public string rolaName { get; set; }
        public string rolaDesc { get; set; }


        public AppDataItem(string appId)
        {
            this.appId = appId;
            this.appName = "";
            this.rolaId = "";
            this.rolaName = "";
            this.rolaDesc = "";
            this.isEnabled = true;
        }

        public object Clone()
        {
            AppDataItem other = new AppDataItem(appId);

            other.appName = this.appName;
            other.rolaDesc = this.rolaDesc;
            other.rolaId = this.rolaId;
            other.rolaName = this.rolaName;
            other.isEnabled = this.isEnabled;

            return other;
        }
    }
}
