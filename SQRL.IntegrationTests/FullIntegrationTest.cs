using System;
using System.Security;
using SQRL.Client;
using SQRL.Server;
using Xunit;

namespace SQRL.IntegrationTests
{
    public class FullIntegrationTest
    {
        private const string Url = "sqrl://example.com/login/sqrl?ip=0.0.0.0&sess=";
        private const string Password = "Testing123";
        private static readonly SecureString SecurePassword;
        private readonly byte[] _entropy = new byte[]
            {
                
            };

        static FullIntegrationTest()
        {
            SecurePassword = new SecureString();
            foreach (char c in Password)
            {
                SecurePassword.AppendChar(c);
            }

            SecurePassword.MakeReadOnly();
        }


        [Fact]
        public void Integration()
        {
            Identity.StorageProvider = new InMemoryStorageProvider();
            SqrlConfig.NonceLength = 16;

            var uri = new Uri(Url);

            Identity id = Identity.CreateNew("Test1", SecurePassword, _entropy);
            var client = new SqrlClient(id);

            client.Process(Url);

            var server = new SqrlAuthenticator();
            
        } 
    }
}