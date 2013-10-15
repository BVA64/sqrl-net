using System;
using System.Windows.Forms;
using SQRL.Client;
using SQRL.Samples.DesktopClient.StorageProvider;

namespace SQRL.Samples.DesktopClient
{
    static class Program
    {
        public const int Success = 0;
        public const int GeneralError = -1;
        public const int Unauthorized = -2;
        public const int NotRegistered = -3;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                string action = string.Empty;
                try
                {
                    if (args[0] == "/register")
                    {
                        action = "register";
                        SqrlProtocolRegistrar.Register();
                        return Success;
                    }

                    if (args[0] == "/unregister")
                    {
                        action = "unregister";
                        SqrlProtocolRegistrar.Unregister();
                        return Success;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    return Unauthorized;
                }
                catch (ArgumentException)
                {
                    return NotRegistered;
                }
                catch
                {
                    return GeneralError;
                }
            }

            Identity.StorageProvider = new AppSettingsIdentityStorageProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var sqrlDesktopClient = new SqrlDesktopClient();
            sqrlDesktopClient.Urls.AddRange(args);

            Application.Run(sqrlDesktopClient);
            return 0;
        }
    }
}
