using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //chcę zamknąć aplikację po zamknięciu okna DesktopForm, które nie jest głównym oknem
            //okno LoginForm jest cały czas otwarte i program się wywala, więc trzeba wymusić zamknięcie w obsłudze błędów
            try
            {
                //Application.Run(new Form1());      //gdy robię jakieś testy na tymczasowym oknie Form1
                Application.Run(new LoginForm());
            }
            catch (System.ObjectDisposedException exc)
            {
                Application.Exit();
            }
        }
    }
}
