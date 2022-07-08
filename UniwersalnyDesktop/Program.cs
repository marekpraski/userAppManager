﻿using System;
using System.Security.Principal;
using System.Windows.Forms;
using UtilityTools;

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

            try
            {
                Application.Run(new LoginForm());
            }
            //chcę zamknąć aplikację po zamknięciu okna DesktopForm, które nie jest głównym oknem
            //okno LoginForm jest cały czas otwarte i program się wywala, więc trzeba wymusić zamknięcie w obsłudze błędów, bez komunikatu
            catch (System.ObjectDisposedException exc)
            {
                Application.Exit();
            }
            catch (Exception e)
            {
                MessageBoxError.ShowBox(e);
            }
        }
    }
}
