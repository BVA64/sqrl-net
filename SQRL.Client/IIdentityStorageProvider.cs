using System;

namespace SQRL.Client
{
    public interface IIdentityStorageProvider
    {
        void Save(string name, string value);
        string Load(string name);
    }
}