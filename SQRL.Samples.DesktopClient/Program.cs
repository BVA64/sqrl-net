using System;
using System.Windows.Forms;
using NDesk.Options;
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
        static void Main(string[] args)
        {
            bool register = false;
            bool unregister = false;

            var optSet = new OptionSet
                {
                    {"r|register", v => register = v != null},
                    {"u|unregister", v => unregister = v != null}
                };

            var extra = optSet.Parse(args);

            if (register)
            {
                SqrlProtocolRegistrar.Register();
            }
            else if (unregister)
            {
                SqrlProtocolRegistrar.Unregister();
            }
            else
            {
                Identity.StorageProvider = new AppSettingsIdentityStorageProvider();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var sqrlDesktopClient = new SqrlDesktopClient();
                sqrlDesktopClient.Urls.AddRange(extra);

                Application.Run(sqrlDesktopClient);
            }
        }
    }
}
