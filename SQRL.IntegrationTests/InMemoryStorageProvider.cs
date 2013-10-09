using System;
using System.Collections.Generic;
using SQRL.Client;

namespace SQRL.IntegrationTests
{
    public class InMemoryStorageProvider :IIdentityStorageProvider
    {
        private readonly Dictionary<string, string> _store = new Dictionary<string, string>();

        public void Save(string name, string value)
        {
            _store[name] = value;
        }

        public string Load(string name)
        {
            return _store[name];
        }
    }
}