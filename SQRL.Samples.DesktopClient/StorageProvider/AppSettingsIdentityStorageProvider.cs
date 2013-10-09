using System;
using SQRL.Client;
using SQRL.Samples.DesktopClient.Properties;

namespace SQRL.Samples.DesktopClient.StorageProvider
{
    public class AppSettingsIdentityStorageProvider : IIdentityStorageProvider
    {
        public void Save(string name, string value)
        {
            if(name != "Identity") throw new ArgumentOutOfRangeException("name", "Name must be \"Identity\"");

            Settings.Default.Identity = value;
            Settings.Default.Save();
        }

        public string Load(string name)
        {
            if (name != "Identity") throw new ArgumentOutOfRangeException("name", "Name must be \"Identity\"");

            string value = Settings.Default.Identity;
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new SqrlIdentityNotFoundException();
            }

            return value;
        }
    }
}