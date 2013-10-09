using System;
using System.Windows.Forms;
using SQRL.Client;
using SQRL.Samples.DesktopClient.StorageProvider;

namespace SQRL.Samples.DesktopClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Identity.StorageProvider = new AppSettingsIdentityStorageProvider();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SqrlDesktopClient());
        }
    }
}
