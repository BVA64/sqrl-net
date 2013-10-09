using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using CryptSharp.Utility;

namespace SQRL.Client
{
    public sealed class Identity
    {
        private const int RandomEntropyBytes = 256;
        private const int ScryptIterations = 1024;
        private const int KeyWidth = 256/8;

        private readonly string _name;
        private readonly SecureString _password;
        private byte[] _encryptedMasterKey;
        private byte[] _masterKeySalt;

        private Identity(string name, SecureString password)
        {
            _name = name;
            _password = password;
        }

        public static IIdentityStorageProvider StorageProvider { get; set; }

        public static Identity CreateNew(string name, SecureString password, byte[] entropy)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (password == null) throw new ArgumentNullException("password");
            if (entropy == null) throw new ArgumentNullException("entropy");
            
            var id= new Identity(name, password);

            id.GenerateMasterKey(entropy);

            return id;
        }

        public static Identity Open(string name, SecureString password)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (password == null) throw new ArgumentNullException("password");

            var id = new Identity(name, password);

            id.LoadIdentity();

            return id;
        }

        private void GenerateMasterKey(byte[] entropy)
        {
            // TODO: Currently using SHA1-PBKDF2, should upgrade to SHA256-PBKDF2
            byte[] generatedEntropy = Sodium.Random.GetBytes(RandomEntropyBytes);
            _masterKeySalt = Sodium.Random.GetBytes(RandomEntropyBytes);

            var passwordHashBytes = GetPasswordHash();
            var master = SCrypt.ComputeDerivedKey(passwordHashBytes, entropy, ScryptIterations, 1024, 1, null, KeyWidth);

            for (int i = 0; i < master.Length; i++)
            {
                master[i] = (byte)(master[i] ^ passwordHashBytes[i]);
            }

            Array.Clear(passwordHashBytes, 0, passwordHashBytes.Length);
            _encryptedMasterKey = master;

            SaveIdentity();
        }

        private void SaveIdentity()
        {
            if (StorageProvider == null)
            {
                return;
            }

            var store = new IdentityStore
                {
                    MasterKey = _encryptedMasterKey,
                    Salt = _masterKeySalt
                };

            var serializer = new BinaryFormatter();
            var stream = new MemoryStream();
            serializer.Serialize(stream, store);

            var protectedData = ProtectedData.Protect(stream.GetBuffer(), null, DataProtectionScope.CurrentUser);
            string encryptedString = Convert.ToBase64String(protectedData);
            StorageProvider.Save(_name, encryptedString);
        }

        private void LoadIdentity()
        {
            if (StorageProvider == null)
            {
                throw new InvalidOperationException("Unable to load identity.  No Storage Provider has been specified.");
            }

            string encryptedString = StorageProvider.Load(_name);
            var protectedData = Convert.FromBase64String(encryptedString);
            var data = ProtectedData.Unprotect(protectedData, null, DataProtectionScope.CurrentUser);
            
            var serializer = new BinaryFormatter();
            var store = (IdentityStore)serializer.Deserialize(new MemoryStream(data));
            _encryptedMasterKey = store.MasterKey;
            _masterKeySalt = store.Salt;
        }

        private byte[] GetPasswordHash()
        {
            var passwordHashBytes = Encoding.ASCII.GetBytes(_password.ToString());
            return SCrypt.ComputeDerivedKey(passwordHashBytes, _masterKeySalt, ScryptIterations, 1024, 1, null, KeyWidth);
        }

        public byte[] GetSitePrivateKey(string domain)
        {
            byte[] masterKey = DecryptMasterKey();
            byte[] privateKey = Sodium.CryptoAuth.AuthHmacSha512_256(domain, masterKey);
            Array.Clear(masterKey, 0, masterKey.Length);
            return privateKey;
        }

        private byte[] DecryptMasterKey()
        {
            var passwordHash = GetPasswordHash();
            var master = new byte[passwordHash.Length];

            for (int i = 0; i < master.Length; i++)
            {
                master[i] = (byte) (_masterKeySalt[i] ^ passwordHash[i]);
            }

            return master;
        }

        [Serializable]
        private class IdentityStore
        {
            public byte[] MasterKey { get; set; }
            public byte[] Salt { get; set; }
        }
    }
}