using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace SQRL.Samples.DesktopClient
{
    public static class SqrlProtocolRegistrar
    {
        private const string ProtocolSqrl = "sqrl";
        private const string ProtocolQrl = "qrl";

         public static void Register()
         {
             RegisterProtocol(ProtocolSqrl);
             RegisterProtocol(ProtocolQrl);
         }

        public static void Unregister()
        {
            UnregisterProtocol(ProtocolSqrl);
            UnregisterProtocol(ProtocolQrl);
        }

        private static void RegisterProtocol(string protocol)
        {
            string appPath = Assembly.GetExecutingAssembly().Location;
            using (RegistryKey proto = Registry.ClassesRoot.CreateSubKey(protocol),
                               icon = proto.CreateSubKey("DefaultIcon"),
                               cmd = proto.CreateSubKey("shell")
                                          .CreateSubKey("open")
                                          .CreateSubKey("command"))
            {
                proto.SetValue(string.Empty, "SQRL Authentication Handler", RegistryValueKind.String);
                proto.SetValue("URL Protocol", string.Empty, RegistryValueKind.String);

                string app = Path.GetFileName(appPath);
                icon.SetValue(string.Empty, string.Format("{0}, 1", app), RegistryValueKind.String);

                cmd.SetValue(string.Empty, string.Format(@"""{0}"" ""%1""", appPath), RegistryValueKind.String);
            }
        }

        private static void UnregisterProtocol(string protocol)
        {
            Registry.ClassesRoot.DeleteSubKeyTree(protocol);
        }
    }
}